
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

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .ThenInclude(ca => ca.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (instructor == null)
            {
                return NotFound();
            }

            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Instructor instructor, int[] selectedCourses)
        {
            if (id != instructor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var instructorToUpdate = await _context.Instructors
                        .Include(i => i.CourseAssignments)
                        .ThenInclude(ca => ca.Course)
                        .Include(i => i.OfficeAssignment)
                        .FirstOrDefaultAsync(i => i.Id == id);

                    if (instructorToUpdate == null)
                    {
                        return NotFound();
                    }

                    instructorToUpdate.FirstMidName = instructor.FirstMidName;
                    instructorToUpdate.LastName = instructor.LastName;
                    instructorToUpdate.HireDate = instructor.HireDate;
                    instructorToUpdate.OfficeAssignment = instructor.OfficeAssignment;

                    if (selectedCourses != null)
                    {
                        var existingCourses = new HashSet<int>(instructorToUpdate.CourseAssignments.Select(ca => ca.CourseId));
                        foreach (var course in _context.Courses)
                        {
                            if (selectedCourses.Contains(course.CourseID))
                            {
                                if (!existingCourses.Contains(course.CourseID))
                                {
                                    instructorToUpdate.CourseAssignments.Add(new CourseAssignment { InstructorId = instructor.Id, CourseId = course.CourseID });
                                }
                            }
                            else
                            {
                                if (existingCourses.Contains(course.CourseID))
                                {
                                    var courseAssignment = instructorToUpdate.CourseAssignments.Single(ca => ca.CourseId == course.CourseID);
                                    instructorToUpdate.CourseAssignments.Remove(courseAssignment);
                                }
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

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

        [HttpGet]
        public async Task<IActionResult> Clone(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .ThenInclude(ca => ca.Course)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instructor == null)
            {
                return NotFound();
            }

            var newInstructor = new Instructor
            {
                FirstMidName = instructor.FirstMidName,
                LastName = instructor.LastName,
                HireDate = instructor.HireDate,
                OfficeAssignment = instructor.OfficeAssignment != null
                    ? new OfficeAssignment { Location = instructor.OfficeAssignment.Location }
                    : null,
                CourseAssignments = instructor.CourseAssignments
                    .Select(ca => new CourseAssignment
                    {
                        CourseId = ca.CourseId
                    }).ToList()
            };

            _context.Add(newInstructor);
            await _context.SaveChangesAsync();

            foreach (var courseAssignment in instructor.CourseAssignments)
            {
                _context.CourseAssignments.Add(new CourseAssignment
                {
                    InstructorId = newInstructor.Id,
                    CourseId = courseAssignment.CourseId
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.Id == id);
        }
    }
}