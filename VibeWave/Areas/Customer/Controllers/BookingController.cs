using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;
using Stripe.Checkout;
using VibeWave.Models.Constants;

namespace VibeWave.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // INDEX
        // =========================
        public IActionResult Index()
        {
            var bookings = _unitOfWork.Booking
                .GetAll(includeProperties: "Concert")
                .ToList();

            return View(bookings);
        }

        // =========================
        // CREATE (GET)
        // =========================
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

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        public IActionResult Create(int ConcertId, string CustomerName, string Email, int NumberOfTickets)
        {
            var concert = _unitOfWork.Concert.Get(u => u.Id == ConcertId);

            if (concert == null)
                return NotFound();

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
                PaymentMethod = PaymentMethods.Card // or "Not Selected" if you prefer
            };

            _unitOfWork.Booking.Add(booking);
            _unitOfWork.Save();

            GenerateQrForBooking(booking, concert, "PENDING");

            _unitOfWork.Booking.Update(booking);
            _unitOfWork.Save();

            return RedirectToAction(nameof(BookingDetails), new { id = booking.Id });
        }

        // =========================
        // BOOKING DETAILS
        // =========================
        public IActionResult BookingDetails(int id)
        {
            var booking = _unitOfWork.Booking.Get(
                u => u.Id == id,
                includeProperties: "Concert"
            );

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // =========================
        // STRIPE PAYMENT
        // =========================
        public IActionResult Pay(int id)
        {
            var booking = _unitOfWork.Booking.Get(
                u => u.Id == id,
                includeProperties: "Concert"
            );

            if (booking == null)
                return NotFound();

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
                SuccessUrl = domain + $"Customer/Booking/PaymentSuccess?id={booking.Id}",
                CancelUrl = domain + $"Customer/Booking/BookingDetails?id={booking.Id}"
            };

            var service = new SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);
        }

        // =========================
        // PAYMENT SUCCESS
        // =========================
        public IActionResult PaymentSuccess(int id)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == id, includeProperties: "Concert");

            if (booking == null)
                return NotFound();

            if (!booking.IsPaid)
            {
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
                    PaymentIntentId = Guid.NewGuid().ToString()
                };

                _unitOfWork.Payment.Add(payment);
                _unitOfWork.Booking.Update(booking);
                _unitOfWork.Save();
            }

            return View("PaymentSuccess", booking);
        }

        // =========================
        // PAY AT VENUE
        // =========================
        public IActionResult PayAtVenue(int id)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == id, includeProperties: "Concert");

            if (booking == null)
                return NotFound();

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

            return RedirectToAction(nameof(BookingDetails), new { id });
        }

        // =========================
        // QR GENERATOR (REUSABLE)
        // =========================
        private void GenerateQrForBooking(Booking booking, Concert concert, string paymentStatus)
        {
            string qrText =
                $"Booking ID: {booking.Id}\n" +
                $"Customer: {booking.CustomerName}\n" +
                $"Concert: {booking.Concert?.ConcertName}\n" +
                $"Location: {booking.Concert?.ConcertLocation}\n" +
                $"Tickets: {booking.NumberOfTickets}\n" +
                $"Total: ${booking.TotalPrice}\n" +
                $"Payment Status: {booking.PaymentStatus}\n" +
                $"Payment Method: {booking.PaymentMethod}";

            booking.QrCodeUrl = GenerateQrCode(qrText);
        }

        // =========================
        // QR CODE GENERATION
        // =========================
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

        // =========================
        // DOWNLOAD QR
        // =========================
        public IActionResult DownloadQr(int id)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == id);

            if (booking == null || string.IsNullOrEmpty(booking.QrCodeUrl))
                return NotFound();

            var base64 = booking.QrCodeUrl.Split(",")[1];
            var bytes = Convert.FromBase64String(base64);

            return File(bytes, "image/png", $"ticket-{id}.png");
        }

        // =========================
        // DELETE
        // =========================
        public IActionResult Delete(int id)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int id)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == id);

            if (booking == null)
                return NotFound();

            _unitOfWork.Booking.Remove(booking);
            _unitOfWork.Save();

            TempData["success"] = "Booking deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}