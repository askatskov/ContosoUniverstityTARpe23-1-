using System.ComponentModel.DataAnnotations;

namespace ContosoUniverstity.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }

        public string Name { get; set; }

        public decimal Budget { get; set; }

        public DateTime StartDate { get; set; }


        public Student?  DogStudent {  get; set; }

        public string EepilineLord { get; set; }

        public int? InstructorID { get; set; }

        public byte? RowVersion { get; set; }

        public Instructor? Administrator { get; set; }

        public ICollection<Course>? Courses { get; set; }
    }
}
