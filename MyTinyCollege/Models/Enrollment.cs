using System.ComponentModel.DataAnnotations;

namespace MyTinyCollege.Models
{

    //grade enum
    public enum Grade
    {
        A, B, C, D, F
    }
    public class Enrollment
    {
        public int enrollmentID { get; set; }//PK
        public int CourseID { get; set; }//FK to course
        public int StudentID { get; set; }//FK to student

        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }//? in this case means optional (nullable)
        public virtual Student student { get; set; }//Many enrollments to 1 student

        public virtual Course course { get; set; }//many enrollments to 1 course
    }
}