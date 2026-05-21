using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;

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

        // INDEX
        public IActionResult Index()
        {
            var bookings = _unitOfWork.Booking
                .GetAll(includeProperties: "Concert")
                .ToList();

            return View(bookings);
        }

        // GET: Create
        public IActionResult Create(int id)
        {
            var concert = _unitOfWork.Concert.Get(
                u => u.Id == id,
                includeProperties: "Category"
            );

            if (concert == null)
            {
                return NotFound();
            }

            return View(concert);
        }

        // POST: Create Booking
        [HttpPost]
        public IActionResult Create(int ConcertId, string CustomerName, string Email, int NumberOfTickets)
        {
            try
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
                    BookingDate = DateTime.Now
                };

                _unitOfWork.Booking.Add(booking);
                _unitOfWork.Save();

                var qrData = new
                {
                    BookingId = booking.Id,
                    Concert = concert.ConcertName,
                    Location = concert.ConcertLocation,
                    Date = concert.DisplayDate,
                    Time = concert.DisplayTime,
                    Customer = booking.CustomerName,
                    Tickets = booking.NumberOfTickets,
                    Total = booking.TotalPrice,
                    Paid = false
                };

                string qrText = System.Text.Json.JsonSerializer.Serialize(qrData);

                booking.QrCodeUrl = GenerateQrCode(qrText);

                _unitOfWork.Save();

                return RedirectToAction(nameof(BookingDetails), new { id = booking.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Booking failed: " + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
        }

        public IActionResult Verify(int id)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == id, includeProperties: "Concert");
            if (booking == null) return NotFound();

            return View(booking);
        }

        // BOOKING DETAILS
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

        // QR CODE
        private string GenerateQrCode(string text)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                using (Bitmap qrImage = qrCode.GetGraphic(20))
                using (MemoryStream ms = new MemoryStream())
                {
                    qrImage.Save(ms, ImageFormat.Png);
                    return "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public IActionResult DownloadQr(int id)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == id);
            if (booking == null || string.IsNullOrEmpty(booking.QrCodeUrl))
                return NotFound();

            var base64 = booking.QrCodeUrl.Split(",")[1];
            var bytes = Convert.FromBase64String(base64);

            return File(bytes, "image/png", $"booking-{id}-qr.png");
        }

        public IActionResult MarkAsPaid(int id)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == id);
            if (booking == null) return NotFound();

            booking.IsPaid = true;
            _unitOfWork.Save();

            return RedirectToAction("BookingDetails", new { id });
        }

        // DELETE GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var booking = _unitOfWork.Booking.Get(u => u.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == id);

            if (booking == null)
                return NotFound();

            _unitOfWork.Booking.Remove(booking);
            _unitOfWork.Save();

            TempData["success"] = "Booking deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        //Add Payment Action
        public IActionResult Pay(int id)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == id, includeProperties: "Concert");

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        [HttpPost]
        public IActionResult PayConfirm(int id)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == id);

            if (booking == null)
                return NotFound();

            booking.IsPaid = true;

            _unitOfWork.Save();

            return RedirectToAction("PaymentSuccess", new { id });
        }

        public IActionResult PaymentSuccess(int id)
        {
            var booking = _unitOfWork.Booking.Get(u => u.Id == id, includeProperties: "Concert");

            if (booking == null)
                return NotFound();

            return View(booking);
        }
    }
}