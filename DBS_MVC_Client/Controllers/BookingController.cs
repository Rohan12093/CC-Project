using DBS_MVC_Client.Models;
using Humanizer.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client.NativeInterop;

namespace DBS_MVC_Client.Controllers
{
    public class BookingController : Controller
    {
        private readonly HttpClient _httpClient;
        public BookingController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("DBS_API");
        }
        [Authorize(Roles ="Patient, Docter")]
        public async Task<IActionResult> Index()
        {
            var booking = await _httpClient.GetFromJsonAsync<List<Booking>>(
                "api/BookingController");

            return View(booking);
        }
        [Authorize(Roles = "Patient, Docter")]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/Booking/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();
            if (!response.IsSuccessStatusCode)
                return NotFound();

            return View(await response.Content.ReadFromJsonAsync<Booking>());

        }

        [Authorize(Roles = "Patient, Docter")]
        public async Task<IActionResult> Create()
        {
            var docSchedule = await _httpClient.GetFromJsonAsync<List<Booking>>("api/Booking/");

            ViewBag.docSchedule = new SelectList(
                docSchedule,
                "DocScheId",
                "DocSchIsBooked"
                );

            return View();
        }
        [Authorize(Roles = "Patient, Docter")]
        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("api/Booking", booking);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View(booking);
            }
            return View(booking);
        }
        [Authorize(Roles = "Patient, Docter")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/Booking/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var booking = await response.Content.ReadFromJsonAsync<Booking>();

            if (booking == null)
                return NotFound();

            //---------------------
            var docSchedule = await _httpClient.GetFromJsonAsync<List<DoctorSchedule>>("api/DoctorSchedule");

            ViewBag.docSchedule = new SelectList(
                docSchedule,
                "DocScheId",
                "DocSchIsBooked",
                booking.DoctorId
                );

            return View(booking);

        }
        [Authorize(Roles = "Patient, Docter")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingId)
                return BadRequest();

            var response = await _httpClient.PutAsJsonAsync($"api/Booking/{id}", booking);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(booking);

        }
        [Authorize(Roles = "Patient, Docter")]
        public async Task<IActionResult> Delete(int id)
        {
            //Find Patient with the ID
            var response = await _httpClient.GetAsync($"api/Booking/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var booking = (await response.Content.ReadFromJsonAsync<Booking>());
            return View(booking);
        }
        [Authorize(Roles = "Patient, Docter")]
        [HttpPost]
        public async Task<IActionResult> Delete(Booking booking)
        {
            //Find Patient with the ID
            var response = await _httpClient.DeleteAsync($"api/Booking/{booking.BookingId}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(booking);
        }

    }
}