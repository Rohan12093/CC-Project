using Microsoft.AspNetCore.Mvc;
using DBS_MVC_Client.Models;

namespace DBS_MVC_Client.Controllers
{
    public class DoctorDetailController : Controller
    {
        private readonly HttpClient _httpClient;
        public DoctorDetailController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IActionResult> Index()
        {
            var doctorDetail = await _httpClient.GetFromJsonAsync<List<DoctorDetail>>(
                "https://localhost:7171/api/DoctorDetail");

            return View(doctorDetail);
        }
    }
}
