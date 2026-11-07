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
    }
}
