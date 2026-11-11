using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBS_API.Models;
using System.Net.Http.Json;
using Microsoft.Identity.Client.NativeInterop;

namespace DBS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        private readonly DBS_Context _context;
        private readonly HttpClient _httpClient;

        public BookingController(DBS_Context context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient("FeeCalcService");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var booking = await _context.TblBookings.ToListAsync();

            return Ok(booking);
        }

        [HttpGet("CalculateFee/{PatientId}")]
        public async Task<IActionResult> CalculateFee(int PatientId)
        {
            var patient = await _context.TblPatientDetails.FindAsync(PatientId);
            if (patient == null)
            {
                return NotFound("Patient record could not be found");
            }

            string url = $"FeeService?PatientDob={patient.PatientDob:yyyy-MM-dd}" +
                $"&PatientEnrolStat={patient.PatientEnrolStat.ToString().ToLower()}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error occurred while calculating fee.");
            }

            var result = await response.Content.ReadFromJsonAsync<FeeResult>();
            return Ok(result);

        }


    }
}

public class FeeResult
{
    public decimal BookingFee { get; set; }
}
