using System.ComponentModel.DataAnnotations;

namespace ContosoUniverstity.Models
{
    public class CourseAssignment
    {
        [Key]
        public int Id { get; set; }
        public int InstructorId { get; set; }
        public int CourseId { get; set; }
        public InstructorExists Instructor { get; set; }
        public Course Course { get; set; }
    }
}
