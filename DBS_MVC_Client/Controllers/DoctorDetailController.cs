using DBS_MVC_Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DBS_MVC_Client.Controllers
{
    public class DoctorDetailController : Controller
    {
        private readonly HttpClient _httpClient;
        public DoctorDetailController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("DBS_API");
        }


        public async Task<IActionResult> Index()
        {
            var booking = await _httpClient.GetFromJsonAsync<List<DoctorDetail>>(
                "api/[DoctorDetailController]");

            return View(booking);
        }
        public async Task<IActionResult> DocDetail(int id)
        {
            var response = await _httpClient.GetAsync($"api/DoctorDetail/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();
            if (!response.IsSuccessStatusCode)
                return NotFound();

            return View(await response.Content.ReadFromJsonAsync<DoctorDetail>());

        }
        public async Task<IActionResult> CreateAppointment()
        {
            var docSchedule = await _httpClient.GetFromJsonAsync<List<DoctorSchedule>>("api/Booking");

            ViewBag.docSchedule = new SelectList(
                docSchedule,
                "DocScheId",
                "DocSchIsBooked"
                );

            return View();
        }
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
            return BadRequest();
        }
        public async Task<IActionResult> EditDetails(int id)
        {
            var response = await _httpClient.GetAsync($"api/DoctorDetail/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var booking = await response.Content.ReadFromJsonAsync<DoctorDetail>();

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
            //----------------------

            return View(booking);

        }
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
        public async Task<IActionResult> Delete(int id)
        {

            var response = await _httpClient.GetAsync($"api/Booking/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var booking = (await response.Content.ReadFromJsonAsync<Booking>());
            return View(booking);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(Booking booking)
        {
            var response = await _httpClient.DeleteAsync($"api/Booking/{booking.BookingId}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(booking);
        }
    }
}
