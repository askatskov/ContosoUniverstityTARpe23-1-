using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniverstity.Models
{
    public class InstructorExists
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName 
        { get
            { return LastName + ", " + FirstMidName; } 
        }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Hired on:")]
        public DateTime HireDate { get; set; }

        public ICollection<CourseAssignment>? CourseAssignments { get; set; }
        public  OfficeAssignment? OfficeAssignment { get; set; }
        public int? Birthday { get; set; }
        [Display(Name = "Sünnipäev :")]
        public WantsToEat? WantsToEat { get; set; }
        public FavoriteClass? FavoriteClass { get; set; }
    }

    public enum FavoriteClass
    {
        No, Yes
    }

    public enum WantsToEat
    {
        VeryHungry, Hungry, NotHungry 
    }
}   


