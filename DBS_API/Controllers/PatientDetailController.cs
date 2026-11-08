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
    }
}
