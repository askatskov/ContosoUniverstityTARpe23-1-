using ContosoUniverstity.Data;
using ContosoUniverstity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniverstity.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly SchoolContext _context;

        public InstructorsController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id, int? courseId)
        {
            var vm = new InstructorIndexData();
            vm.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .ThenInclude(ca => ca.Course)
                .ThenInclude(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .AsNoTracking()
                .OrderBy(i => i.LastName)
                .ToListAsync();

            if (id.HasValue)
            {
                ViewData["InstructorId"] = id.Value;
                var instructor = vm.Instructors
                    .FirstOrDefault(i => i.Id == id.Value);

                if (instructor != null)
                {
                    vm.Courses = instructor.CourseAssignments
                        .Select(ca => ca.Course)
                        .ToList();
                }
            }

            if (courseId.HasValue)
            {
                ViewData["CourseID"] = courseId.Value;
                var course = vm.Courses
                    .FirstOrDefault(c => c.CourseID == courseId.Value);

                if (course != null)
                {
                    vm.Enrollments = course.Enrollments.ToList();
                }
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PopulateAssignedCourseData(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Instructor instructor, int[] selectedCourses)
        {
            if (ModelState.IsValid)
            {
                if (selectedCourses != null)
                {
                    instructor.CourseAssignments = new List<CourseAssignment>();
                    foreach (var courseId in selectedCourses)
                    {
                        var courseAssignment = new CourseAssignment
                        {
                            InstructorId = instructor.Id,
                            CourseId = courseId
                        };
                        instructor.CourseAssignments.Add(courseAssignment);
                    }
                }
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = _context.Courses.ToList();
            var instructorCourses = new HashSet<int>(instructor?.CourseAssignments.Select(ca => ca.CourseId) ?? Enumerable.Empty<int>());
            var vm = allCourses.Select(course => new AssignedCourseData
            {
                CourseId = course.CourseID,
                Title = course.Title,
                Assigned = instructorCourses.Contains(course.CourseID)
            }).ToList();

            ViewData["Courses"] = vm;
        }

        // GET: Instructors/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.CourseAssignments)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instructor == null)
            {
                return NotFound();
            }

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
