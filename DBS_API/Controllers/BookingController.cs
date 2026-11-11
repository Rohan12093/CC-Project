using DBS_API.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin,Doctor,Patient")]
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


        //--Kailash 9/11/2026--//
        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet("{bookingid}")]
        public async Task<ActionResult<TblBooking>> GetBooking(int bookingid)
        {
            var appointment = await _context.TblBookings.FirstOrDefaultAsync(p => p.BookingId == bookingid);

            if (appointment == null)
            {
                return NotFound("Appointment not found");
            }

            return appointment;
        }

        //---PUT---/
        [Authorize(Roles = "Admin,Doctor")]
        [HttpPut("{bookingid}")]
        public async Task<IActionResult> PutBooking(int? bookingid, TblBooking appointment)
        {
            if (bookingid != appointment.BookingId)
            {
                return BadRequest();
            }

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(bookingid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok($"Booking {bookingid} has been successfully updated");
        }

        //--POST--//
        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpPost]
        public async Task<ActionResult<TblBooking>> PostBooking(TblBooking appointment)
        {
            _context.TblBookings.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { bookingid = appointment.BookingId }, appointment);
        }

        //--DELETE--//
        [Authorize(Roles = "Admin")]
        [HttpDelete("{bookingid}")]
        public async Task<IActionResult> DeleteBooking(int? bookingid)
        {
            var appointment = await _context.TblBookings.FindAsync(bookingid);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.TblBookings.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok($"Booking {bookingid} has been successfully removed");
        }

        private bool BookingExists(int? bookingid)
        {
            return _context.TblBookings.Any(e => e.BookingId == bookingid);
        }
    }
}

public class FeeResult
{
    public decimal BookingFee { get; set; }
}
