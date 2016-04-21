namespace MyTinyCollege.Models
{
    public class OfficeAssignment
    {
        public int InstructorID { get; set; }
        public string Location { get; set; }

        public virtual Instructor Instructor { get; set; }


    }

}