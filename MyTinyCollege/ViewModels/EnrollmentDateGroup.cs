using System;
using System.ComponentModel.DataAnnotations;

namespace MyTinyCollege.ViewModels
{
    public class EnrollmentDateGroup
    {
        //this will be used to show student body stats report
        //Counting how many students enrolled on a particular
        //EnrollmentDate


        // without this annotation we would get a date time
        //9/1/2016 12:00:00 AM
        [DataType(DataType.Date)]
        public DateTime? EnrollmentDate { get; set; }
        public int StudentCount { get; set; }
    }
}