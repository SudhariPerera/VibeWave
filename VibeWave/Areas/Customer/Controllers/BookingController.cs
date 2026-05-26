using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;
using VibeWave.Models.Constants;
using Stripe;

namespace VibeWave.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private bool CanAccessBooking(Booking booking)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return User.IsInRole("Admin") || booking.UserId == userId;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Booking> bookings;

            if (User.IsInRole("Admin"))
            {
                bookings = _unitOfWork.Booking.GetAll(includeProperties: "Concert");
            }
            else
            {
                bookings = _unitOfWork.Booking.GetAll(
                    u => u.UserId == userId,
                    includeProperties: "Concert"
                );
            }

            return View(bookings.ToList());
        }

        public IActionResult Create(int id)
        {
            var concert = _unitOfWork.Concert.Get(
                u => u.Id == id,
                includeProperties: "Category"
            );

            if (concert == null)
                return NotFound();

            return View(concert);
        }

        [HttpPost]
        public IActionResult Create(int ConcertId, string CustomerName, string Email, int NumberOfTickets)
        {
            var concert = _unitOfWork.Concert.Get(u => u.Id == ConcertId);

            if (concert == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            HttpContext.Session.SetInt32("ConcertId", ConcertId);
            HttpContext.Session.SetString("CustomerName", CustomerName);
            HttpContext.Session.SetString("Email", Email);
            HttpContext.Session.SetInt32("NumberOfTickets", NumberOfTickets);

            var booking = new Booking
            {
                ConcertId = ConcertId,
                CustomerName = CustomerName,
                Email = Email,
                NumberOfTickets = NumberOfTickets,
                TotalPrice = concert.TicketPrice * NumberOfTickets,
                BookingDate = DateTime.Now,
                IsPaid = false,
                PaymentStatus = PaymentStatuses.Pending,
                PaymentMethod = PaymentMethods.Card,
                UserId = userId
            };

            _unitOfWork.Booking.Add(booking);
            _unitOfWork.Save();

            GenerateQrForBooking(booking, concert);

            _unitOfWork.Booking.Update(booking);
            _unitOfWork.Save();

            return RedirectToAction(nameof(BookingDetails), new { id = booking.Id });
        }

        public IActionResult BookingDetails(int id)
        {
            var booking = _unitOfWork.Booking.Get(
                u => u.Id == id,
                includeProperties: "Concert"
            );

            if (booking == null)
                return NotFound();

            if (!CanAccessBooking(booking))
                return Forbid();

            return View(booking);
        }

        public IActionResult Pay(int id)
        {
            var booking = _unitOfWork.Booking.Get(
                u => u.Id == id,
                includeProperties: "Concert"
            );

            if (booking == null)
                return NotFound();

            if (!CanAccessBooking(booking))
                return Forbid();

            var domain = $"{Request.Scheme}://{Request.Host}/";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Quantity = booking.NumberOfTickets,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmount = (long)(booking.Concert.TicketPrice * 100),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = booking.Concert.ConcertName
                            }
                        }
                    }
                },
                Mode = "payment",
                SuccessUrl = domain + $"Customer/Booking/PaymentSuccess?id={booking.Id}&session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = domain + $"Customer/Booking/BookingDetails?id={booking.Id}"
            };

            var service = new SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);
        }

        public IActionResult PaymentSuccess(int id, string session_id)
        {
            var booking = _unitOfWork.Booking.Get(
                u => u.Id == id,
                includeProperties: "Concert"
            );

            if (booking == null)
                return NotFound();

            if (!CanAccessBooking(booking))
                return Forbid();

            if (!booking.IsPaid)
            {
                var sessionService = new SessionService();
                var session = sessionService.Get(session_id);

                booking.IsPaid = true;
                booking.PaymentStatus = PaymentStatuses.Paid;
                booking.PaymentMethod = PaymentMethods.Card;

                var payment = new Payment
                {
                    BookingId = booking.Id,
                    Amount = booking.TotalPrice,
                    Currency = "USD",
                    PaymentStatus = "Paid",
                    PaymentDate = DateTime.Now,
                    PaymentIntentId = session.PaymentIntentId
                };

                _unitOfWork.Payment.Add(payment);
                _unitOfWork.Booking.Update(booking);
                _unitOfWork.Save();
            }
            HttpContext.Session.Remove("ConcertId");
            HttpContext.Session.Remove("CustomerName");
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("NumberOfTickets");

            return View("PaymentSuccess", booking);
        }

        public IActionResult PayAtVenue(int id)
        {
            var booking = _unitOfWork.Booking.Get(
                u => u.Id == id,
                includeProperties: "Concert"
            );

            if (booking == null)
                return NotFound();

            if (!CanAccessBooking(booking))
                return Forbid();

            booking.PaymentStatus = PaymentStatuses.PayAtVenue;
            booking.PaymentMethod = PaymentMethods.Venue;
            booking.IsPaid = false;

            booking.QrCodeUrl = GenerateQrCode(
                $"Booking ID: {booking.Id}\n" +
                $"Customer: {booking.CustomerName}\n" +
                $"Concert: {booking.Concert?.ConcertName}\n" +
                $"Payment: PAY AT VENUE"
            );

            _unitOfWork.Booking.Update(booking);
            _unitOfWork.Save();

            HttpContext.Session.Remove("ConcerId");
            HttpContext.Session.Remove("CustomerName");
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("NumberOfTickets");

            return RedirectToAction(nameof(BookingDetails), new { id });
        }

        private void GenerateQrForBooking(Booking booking, Concert concert)
        {
            string qrText =
                $"Booking ID: {booking.Id}\n" +
                $"Customer: {booking.CustomerName}\n" +
                $"Concert: {concert.ConcertName}\n" +
                $"Location: {concert.ConcertLocation}\n" +
                $"Tickets: {booking.NumberOfTickets}\n" +
                $"Total: ${booking.TotalPrice}\n" +
                $"Payment Status: {booking.PaymentStatus}\n" +
                $"Payment Method: {booking.PaymentMethod}";

            booking.QrCodeUrl = GenerateQrCode(qrText);
        }

        private string GenerateQrCode(string text)
        {
            using (QRCodeGenerator generator = new QRCodeGenerator())
            {
                QRCodeData data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
                QRCode code = new QRCode(data);

                using (Bitmap bitmap = code.GetGraphic(20))
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public IActionResult DownloadQr(int id)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == id);

            if (booking == null || string.IsNullOrEmpty(booking.QrCodeUrl))
                return NotFound();

            if (!CanAccessBooking(booking))
                return Forbid();

            var base64 = booking.QrCodeUrl.Split(",")[1];
            var bytes = Convert.FromBase64String(base64);

            return File(bytes, "image/png", $"ticket-{id}.png");
        }

        public IActionResult Delete(int id)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == id);

            if (booking == null)
                return NotFound();

            if (!CanAccessBooking(booking))
                return Forbid();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int id)
        {
            var booking = _unitOfWork.Booking.Get(
                u => u.Id == id,
                includeProperties: "Concert"
            );

            if (booking == null)
                return NotFound();

            if (!CanAccessBooking(booking))
                return Forbid();

            if (booking.IsPaid && booking.PaymentMethod == PaymentMethods.Card)
            {
                var payment = _unitOfWork.Payment.Get(u => u.BookingId == booking.Id);

                if (payment == null || string.IsNullOrEmpty(payment.PaymentIntentId))
                {
                    TempData["error"] = "Refund failed because payment details were not found.";
                    return RedirectToAction(nameof(Index));
                }

                var refundOptions = new RefundCreateOptions
                {
                    PaymentIntent = payment.PaymentIntentId
                };

                var refundService = new RefundService();
                var refund = refundService.Create(refundOptions);

                payment.PaymentStatus = "Refunded";
                booking.PaymentStatus = "Refunded";
                booking.IsPaid = false;

                _unitOfWork.Payment.Update(payment);
                _unitOfWork.Booking.Update(booking);
                _unitOfWork.Save();
            }

            if (booking.PaymentStatus == "Refunded")
            {
                TempData["success"] = "Booking has been refunded and marked as cancelled.";
                return RedirectToAction(nameof(Index));
            }

            // Only remove unpaid / pay-at-venue bookings
            _unitOfWork.Booking.Remove(booking);
            _unitOfWork.Save();

            TempData["success"] = "Booking deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}