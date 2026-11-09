using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBS_API.Models;

namespace DBS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorDetailController : Controller
    {
        private readonly DBS_Context _context;

        public DoctorDetailController(DBS_Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var doctorDetail = await _context.TblDoctorDetails.ToListAsync();

            return Ok(doctorDetail);
        }

        //--Kailash 9/11/2026--//
        [HttpGet("{doctorid}")]
        public async Task<ActionResult<TblDoctorDetail>> GetDoctor(int doctorid)
        {
            var doctor = await _context.TblDoctorDetails.FirstOrDefaultAsync(p => p.DoctorId == doctorid);

            if (doctor == null)
            {
                return BadRequest("Doctor not found");
            }

            return doctor;
        }

        //---PUT---/
        [HttpPut("{doctorid}")]
        public async Task<IActionResult> PutDoctor(int? doctorid, TblDoctorDetail doctor)
        {
            if (doctorid != doctor.DoctorId)
            {
                return BadRequest();
            }

            _context.Entry(doctor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(doctorid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok($"Doctor {doctorid} has been successfully updated");
        }

        //--POST--//
        [HttpPost]
        public async Task<ActionResult<TblDoctorDetail>> PostDoctor(TblDoctorDetail doctor)
        {
            _context.TblDoctorDetails.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDoctor", new { doctorId = doctor.DoctorId }, doctor);
        }

        //--DELETE--//
        [HttpDelete("{doctorid}")]
        public async Task<IActionResult> DeleteDoctor(int? doctorid)
        {
            var doctor = await _context.TblDoctorDetails.FindAsync(doctorid);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.TblDoctorDetails.Remove(doctor);
            await _context.SaveChangesAsync();

            return Ok($"Doctor {doctorid} has been successfully removed");
        }

        //--To check if the patients exits--//
        private bool DoctorExists(int? doctorid)
        {
            return _context.TblDoctorDetails.Any(e => e.DoctorId == doctorid);
        }
    }
}
