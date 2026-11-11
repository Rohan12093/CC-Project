using DBS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorScheduleController : Controller
    {
        private readonly DBS_Context _context;

        public DoctorScheduleController(DBS_Context context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var doctorSchedule = await _context.TblDoctorSchedules.ToListAsync();

            return Ok(doctorSchedule);
        }

        //--Kailash 9/11/2026--//
        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet("{docscheid}")]
        public async Task<ActionResult<TblDoctorSchedule>> GetDoctorSchedule(int docscheid)
        {
            var docsched = await _context.TblDoctorSchedules.FirstOrDefaultAsync(p => p.DocScheId == docscheid);

            if (docsched == null)
            {
                return NotFound("Doctor Shedule not found");
            }

            return docsched;
        }

        //---PUT---/
        [Authorize(Roles = "Admin,Doctor")]
        [HttpPut("{docscheid}")]
        public async Task<IActionResult> PutDocSchedule(int? docscheid, TblDoctorSchedule docSchedule)
        {
            if (docscheid != docSchedule.DocScheId)
            {
                return BadRequest();
            }

            _context.Entry(docSchedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocSheduleExists(docscheid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok($"Doctor Shedule {docscheid} has been successfully updated");
        }

        //--POST--//
        [Authorize(Roles = "Admin,Doctor")]
        [HttpPost]
        public async Task<ActionResult<TblDoctorSchedule>> PostDocShedule(TblDoctorSchedule docSchedule)
        {
            _context.TblDoctorSchedules.Add(docSchedule);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDoctorSchedule", new { docscheid = docSchedule.DocScheId }, docSchedule);
        }

        //--DELETE--//
        [Authorize(Roles = "Admin,Doctor")]
        [HttpDelete("{docscheid}")]
        public async Task<IActionResult> DeleteDocShedule(int? docscheid)
        {
            var docsched = await _context.TblDoctorSchedules.FindAsync(docscheid);
            if (docsched == null)
            {
                return NotFound();
            }

            _context.TblDoctorSchedules.Remove(docsched);
            await _context.SaveChangesAsync();

            return Ok($"Doctor Schedule has been successfully removed");
        }

        private bool DocSheduleExists(int? docscheid)
        {
            return _context.TblDoctorSchedules.Any(e => e.DocScheId == docscheid);
        }

    }
}
