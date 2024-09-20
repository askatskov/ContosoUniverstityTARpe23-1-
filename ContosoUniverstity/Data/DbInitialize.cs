using ContosoUniverstity.Models;
using System;
using System.Linq;

namespace ContosoUniverstity.Data
{
    public static class DbInitialize
    {
        public static void Initialize(SchoolContext context)
        {
            // Ensures that the database is created or already exists
            context.Database.EnsureCreated();

            // If there are any students in the database, return immediately
            if (context.Students.Any())
            {
                return; // Database has already been seeded
            }

            // Create an array of students to be added if the table is empty
            var students = new Student[]
            {
                new Student {FirstMidName = "Artur", LastName = "Petrovski", EnrollmentDate = DateTime.Parse("2069-04-20")},
                new Student {FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2009-05-21")},
                new Student {FirstMidName = "Marko", LastName = "Vasiljev", EnrollmentDate = DateTime.Parse("2007-09-25")},
                new Student {FirstMidName = "Allan", LastName = "Lond", EnrollmentDate = DateTime.Parse("2054-12-31")},
                new Student {FirstMidName = "James", LastName = "Bond", EnrollmentDate = DateTime.Parse("2007-09-30")}, // Corrected invalid date
                new Student {FirstMidName = "John", LastName = "Wick", EnrollmentDate = DateTime.Parse("2002-09-25")},
                new Student {FirstMidName = "Vasya", LastName = "Pupkin", EnrollmentDate = DateTime.Parse("2021-05-01")}, // Corrected invalid year
                new Student {FirstMidName = "Caesar", LastName = "Salatov", EnrollmentDate = DateTime.Parse("2012-01-23")},
                new Student {FirstMidName = "Playboi", LastName = "Carti", EnrollmentDate = DateTime.Parse("2010-02-07")},
                new Student {FirstMidName = "Cristiano", LastName = "Ronaldo", EnrollmentDate = DateTime.Parse("1999-03-16")},
            };

            // Add students to the context and save the changes
            context.Students.AddRange(students);
            context.SaveChanges();

            // Create an array of courses
            var courses = new Course[]
            {
                new Course {CourseID = 1050, Title = "Keemia", Credits = 3},
                new Course {CourseID = 3212, Title = "Inglise Keel", Credits = 3},
                new Course {CourseID = 4041, Title = "Vene Keel", Credits = 1},
                new Course {CourseID = 1056, Title = "Matemaatika", Credits = 2},
                new Course {CourseID = 7544, Title = "Calculus", Credits = 2},
                new Course {CourseID = 8264, Title = "Trigonomeetria", Credits = 3},
                new Course {CourseID = 7467, Title = "Muusika", Credits = 3},
                new Course {CourseID = 1111, Title = "Kirjandus", Credits = 4},
            };

            // Add courses to the context and save the changes
            context.Courses.AddRange(courses);
            context.SaveChanges();

            // Create an array of enrollments
            var enrollments = new Enrollment[]
            {
                new Enrollment {StudentID = 1, CourseID = 1050, Grade = Grade.A},
                new Enrollment {StudentID = 1, CourseID = 3212, Grade = Grade.C},
                new Enrollment {StudentID = 1, CourseID = 4041, Grade = Grade.B},
                new Enrollment {StudentID = 2, CourseID = 1056, Grade = Grade.B},
                new Enrollment {StudentID = 2, CourseID = 7544, Grade = Grade.F},
                new Enrollment {StudentID = 3, CourseID = 1111},
                new Enrollment {StudentID = 4, CourseID = 1111},
                new Enrollment {StudentID = 4, CourseID = 7467, Grade = Grade.F},
                new Enrollment {StudentID = 5, CourseID = 8264, Grade = Grade.C},
                new Enrollment {StudentID = 6, CourseID = 1111},
                new Enrollment {StudentID = 7, CourseID = 1050, Grade = Grade.A},
                new Enrollment {StudentID = 10, CourseID = 3212, Grade = Grade.A},
            };

            // Add enrollments to the context and save the changes
            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();
        }
    }
}
