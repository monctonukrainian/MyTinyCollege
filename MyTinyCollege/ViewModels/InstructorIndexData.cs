using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyTinyCollege.Models;
using System.Collections;

namespace MyTinyCollege.ViewModels
{
    public class InstructorIndexData
    {
        //For reading Related data: Instructors-Courses-Enrollment
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}