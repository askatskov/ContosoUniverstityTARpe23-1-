using Microsoft.AspNetCore.Mvc;
using ContosoUniverstity.Data;
using Microsoft.EntityFrameworkCore;
using ContosoUniverstity.Migrations;

namespace ContosoUniverstity.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly SchoolContext _context;

        public DepartmentsController(SchoolContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Departments.Include(d => d.Administrator);
            return View(await schoolContext.ToListAsync());
        }
    }
}
