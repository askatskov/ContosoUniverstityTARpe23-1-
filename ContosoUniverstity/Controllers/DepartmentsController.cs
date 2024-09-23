using Microsoft.AspNetCore.Mvc;
using ContosoUniverstity.Data;

namespace ContosoUniverstity.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly SchoolContext _context;

        public DepartmentsController(SchoolContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
