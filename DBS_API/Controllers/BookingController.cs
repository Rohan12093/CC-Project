using DBS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        private readonly DBS_Context _context;

        public BookingController(DBS_Context context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var booking = await _context.TblBookings.ToListAsync();

            return Ok(booking);
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
