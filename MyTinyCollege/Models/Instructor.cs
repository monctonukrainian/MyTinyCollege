using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyTinyCollege.Models
{
    public class Instructor:Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        //1 instructor to many courses
        public virtual ICollection<Course> Courses { get; set; }

        // 1 instructor to office assignment
        public virtual OfficeAssignment OfficeAssignment { get; set; }
    }
}