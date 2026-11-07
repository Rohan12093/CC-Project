using Microsoft.AspNetCore.Mvc;
using DBS_MVC_Client.Models;

namespace DBS_MVC_Client.Controllers
{
    public class PatientDetailController : Controller
    {
        private readonly HttpClient _httpClient;
        public PatientDetailController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IActionResult> Index()
        {
            var patientDetail = await _httpClient.GetFromJsonAsync<List<PatientDetail>>(
                "https://localhost:7171/api/PatientDetail");

            return View(patientDetail);
        }
    }
}
