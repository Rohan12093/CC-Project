using Microsoft.AspNetCore.Mvc;
using DBS_MVC_Client.Models;

namespace DBS_MVC_Client.Controllers
{
    public class BookingController : Controller
    {
        private readonly HttpClient _httpClient;
        public BookingController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IActionResult> Index()
        {
            var booking = await _httpClient.GetFromJsonAsync<List<Booking>>(
                "https://localhost:7171/api/Booking");

            return View(booking);
        }
    }
}
