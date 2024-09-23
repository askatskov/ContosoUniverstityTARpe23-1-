using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniverstity.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        [StringLength(50, MinimumLength = 3)]

        public string Name { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "Money")]

        public decimal Budget { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime StartDate { get; set; }


        public Student? Status {  get; set; }
        [Display(Name = "Ancient scrolls which tell the tale of creation of this damned place.")]

        public string Aadress { get; set; }

        public int? InstructorID { get; set; }
        [Timestamp]

        public byte? RowVersion { get; set; }

        public Instructor? Administrator { get; set; }

        public ICollection<Course>? Courses { get; set; }
    }
}
