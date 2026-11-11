using DBS_MVC_Client.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Doctor, Admin")]
        public async Task<IActionResult> Index()
        {
            var booking = await _httpClient.GetFromJsonAsync<List<DoctorDetail>>(
                "api/DoctorDetailController");

            return View(booking);
        }
        [Authorize(Roles = "Doctor, Admin")]
        public async Task<IActionResult> DocDetail(int id)
        {
            var response = await _httpClient.GetAsync($"api/DoctorDetail/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();
            if (!response.IsSuccessStatusCode)
                return NotFound();

            return View(await response.Content.ReadFromJsonAsync<DoctorDetail>());

        }
        [Authorize(Roles = "Doctor, Admin")]
        public async Task<IActionResult> Create()
        {
            var docSchedule = await _httpClient.GetFromJsonAsync<List<DoctorSchedule>>("api/DoctorSchedule");

            ViewBag.docSchedule = new SelectList(
                docSchedule,
                "DocScheId",
                "DocSchIsDate",
                "DocSchStime"
                );

            return View();
        }
        [Authorize(Roles = "Doctor, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(DoctorDetail docDetail)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("api/DoctorDetail", docDetail);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View(docDetail);
            }
            return BadRequest();
        }
        [Authorize(Roles = "Doctor, Admin")]
        public async Task<IActionResult> EditDetails(int id)
        {
            var response = await _httpClient.GetAsync($"api/DoctorSchedule/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var bookingSch = await response.Content.ReadFromJsonAsync<DoctorDetail>();

            if (bookingSch == null)
                return NotFound();

            //---------------------
            var docSchedule = await _httpClient.GetFromJsonAsync<List<DoctorSchedule>>("api/DoctorSchedule");

            ViewBag.docSchedule = new SelectList(
                docSchedule,
                "DocSchIsDate",
                "DocSchStime",
                bookingSch.DoctorId
                );
            //----------------------

            return View(bookingSch);

        }
        [Authorize(Roles = "Doctor, Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, DoctorDetail doctorDetail)
        {
            if (id != doctorDetail.DoctorId)
                return BadRequest();

            var response = await _httpClient.PutAsJsonAsync($"api/DoctorDetail/{id}", doctorDetail);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(doctorDetail);

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {

            var response = await _httpClient.GetAsync($"api/DoctorDetail/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var booking = (await response.Content.ReadFromJsonAsync<DoctorDetail>());
            return View(booking);
        }
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ConfirmDelete(DoctorDetail doctorDetail)
        {
            var response = await _httpClient.DeleteAsync($"api/DoctorDetail/{doctorDetail.DoctorId}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(doctorDetail);
        }
    
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var response = await _httpClient.GetAsync($"api/PatientDetail/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var booking = (await response.Content.ReadFromJsonAsync<PatientDetail>());
            return View(booking);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(PatientDetail patientDetail)
        {
            var response = await _httpClient.DeleteAsync($"api/PatientDetail/{patientDetail.PatientId}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(patientDetail);
        }

    }
}
