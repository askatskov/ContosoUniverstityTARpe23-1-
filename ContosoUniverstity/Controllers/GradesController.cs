using ContosoUniverstity.Data;
using ContosoUniverstity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniverstity.Controllers
{
    public class GradesController : Controller
    {
        private readonly SchoolContext _context;

        public GradesController(SchoolContext context)
        {
            _context = context;
        }

        // Display courses for a specific student
        public async Task<IActionResult> StudentCourses(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                return NotFound();
            }

            return View(student); // Views/Grades/StudentCourses.cshtml
        }

        // Edit grade for a specific course enrollment
        public async Task<IActionResult> EditGrade(int enrollmentId)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(e => e.EnrollmentID == enrollmentId);

            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment); // Views/Grades/EditGrade.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGrade(int enrollmentId, Grade? grade)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
            if (enrollment == null)
            {
                return NotFound();
            }

            enrollment.Grade = grade;
            await _context.SaveChangesAsync();

            return RedirectToAction("StudentCourses", new { studentId = enrollment.StudentID });
        }
    }
}
