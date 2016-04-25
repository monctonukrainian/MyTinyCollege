using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTinyCollege.Models
{
    public class OfficeAssignment
    {
        [Key]//Forces the InstructorID to become the PK for OfficeAssignment
        [ForeignKey("Instructor")]
        public int InstructorID { get; set; }

        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        public virtual Instructor Instructor { get; set; }


    }

}