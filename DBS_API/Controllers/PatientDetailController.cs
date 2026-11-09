using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBS_API.Models;


namespace DBS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientDetailController : Controller
    {
        private readonly DBS_Context _context;

        public PatientDetailController(DBS_Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var patientDetails = await _context.TblPatientDetails.ToListAsync();

            return Ok(patientDetails);
        }

        //--Kailash 9/11/2026--//
        [HttpGet("{patientid}")]
        public async Task<ActionResult<TblPatientDetail>> GetPatient(int patientid)
        {
            var patient = await _context.TblPatientDetails.FirstOrDefaultAsync(p => p.PatientId == patientid);

            if (patient == null)
            {
                return BadRequest("Patient not found");
            }

            return patient;
        }

        //---PUT---/
        [HttpPut("{patientid}")]
        public async Task<IActionResult> PutPatient(int? patientid, TblPatientDetail patient)
        {
            if (patientid != patient.PatientId)
            {
                return BadRequest();
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(patientid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok($"Patient {patientid} has been successfully updated");
        }

        //--POST--//
        [HttpPost]
        public async Task<ActionResult<TblPatientDetail>> PostPatient(TblPatientDetail patient)
        {
            _context.TblPatientDetails.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { patientId = patient.PatientId }, patient);
        }

        //--DELETE--//
        [HttpDelete("{patientid}")]
        public async Task<IActionResult> DeletePatient(int? patientid)
        {
            var patient = await _context.TblPatientDetails.FindAsync(patientid);
            if (patient == null)
            {
                return NotFound();
            }

            _context.TblPatientDetails.Remove(patient);
            await _context.SaveChangesAsync();

            return Ok($"Patient {patientid} has been successfully removed");
        }

        //--To check if the patients exits--//
        private bool PatientExists(int? patientid)
        {
            return _context.TblPatientDetails.Any(e => e.PatientId == patientid);
        }
    }
}
