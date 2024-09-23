using System.ComponentModel.DataAnnotations;

namespace ContosoUniverstity.Models
{
    public class OfficeAssignment
    {
        [Key]
        public int InstructorId { get; set; }
        public InstructorExists? Instructor { get; set; }
        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set;}
    }
}
