using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using MyTinyCollege.DAL;
using MyTinyCollege.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyTinyCollege.Controllers
{
    [Authorize(Roles ="student")]
    public class StudentEnrollmentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: StudentEnrollment
        public ActionResult Index()
        {
            //find out which student is currently logged in
            string userId = User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                var manager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(ApplicationDbContext.Create())
                    );
                var currentStudent = manager.FindByEmail(User.Identity.GetUserName());

                //get the student entity for this logged in user
                Student student = db.Students
                    .Include(i => i.Enrollments)
                    .Where(i => i.Email == currentStudent.Email).Single();
                //create and execute a SQL Raw query: get all courses not enrolled for this student
                string query = "SELECT CourseID, Title FROM Course WHERE CourseID NOT IN (SELECT DISTINCT CourseID FROM Enrollment WHERE StudentID=@p0)";
                IEnumerable<ViewModels.AssignedCourseData> data = db.Database.SqlQuery<ViewModels.AssignedCourseData>(query, student.ID);
                ViewBag.Courses = data.ToList();

                //get all enrollments
                var studentEnrollments = db.Enrollemnts
                    .Include(e => e.course)
                    .Include(e => e.student)
                    .Where(e => e.student.Email == currentStudent.Email);
                return View(studentEnrollments.ToList());
            }
            else
            {
                return HttpNotFound();
            }

                
        }//end of index

        public ActionResult Enroll(int? courseID)
        {
            if(courseID==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string userId = User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                var manager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(ApplicationDbContext.Create())
                    );
                var currentStudent = manager.FindByEmail(User.Identity.GetUserName());

                //get student entity with enrollments for this student (email)
                Student student = db.Students
                    .Include(i => i.Enrollments)
                    .Where(i => i.Email == currentStudent.Email).Single();

                ViewBag.StudentID = student.ID; //for hidden form field in enroll view


                //create hash set of enrollments 
                var studentEnrollments =
                    new HashSet<int>(db.Enrollemnts
                    .Include(e => e.course)
                    .Include(e => e.student)
                    .Where(e => e.student.Email == currentStudent.Email)
                    .Select(e => e.CourseID));
                //fix for dealing with conversion int? to int
                int currentCourseID;
                if (courseID.HasValue)
                {
                    currentCourseID = (int)courseID;
                }
                else
                {
                    currentCourseID = 0;
                }
                if (studentEnrollments.Contains(currentCourseID))
                {
                    //if it is true (student already enrolled in this course
                    //return model error
                    ModelState.AddModelError("AlreadyEnrolled", "You are already enrolled in this course");
                }             

            }
            Course course = db.Courses.Find(courseID);
                if (course==null)
                {
                    return HttpNotFound();
                }

                return View(course);
        }

        //POST: SttudentEnrollment/Enroll
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enroll([Bind(Include ="CourseID, StudentID")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollemnts.Add(enrollment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}