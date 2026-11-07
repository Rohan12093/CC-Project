using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBS_API.Models;

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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var doctorSchedule = await _context.TblDoctorSchedules.ToListAsync();

            return Ok(doctorSchedule);
        }
    }
}
