using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyTinyCollege.DAL;
using MyTinyCollege.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyTinyCollege.ViewModels;

namespace MyTinyCollege.Controllers
{
    [Authorize(Roles ="instructor")]
    public class InstructorCourseController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: InstructorCourse
        public ActionResult Index(int? courseID)
        {
            string userId = User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                //Found the user who is currently logged in
                //but we need their email to associate back to our
                //Person instructor entity
                var manager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(ApplicationDbContext.Create())
                    );
                //the username is email (user registers and logs in with email)
                var currentInstructor = manager.FindByEmail(User.Identity.GetUserName());
                var viewModel = new InstructorIndexData();

                viewModel.Instructors = db.Instructors
                    .Include(i => i.Courses)
                    .Where(i => i.Email == currentInstructor.Email);

                //get a single instructor
                var instructor = viewModel.Instructors
                    .Where(i => i.Email == currentInstructor.Email).Single();

                //get all courses assigned to this single instructor
                viewModel.Courses = viewModel.Instructors
                    .Where(i => i.ID == instructor.ID).Single().Courses;

                //get all students enrolled in selected course
                if (courseID!=null)
                {
                    var selectedCourse = viewModel.Courses.Where(x => x.CourseID == courseID).Single();
                    //explicit loading
                    db.Entry(selectedCourse).Collection(x => x.Enrollments).Load();
                    foreach (Enrollment enrollment in selectedCourse.Enrollments)
                    {
                        db.Entry(enrollment).Reference(x => x.student).Load();
                    }
                    viewModel.Enrollments = selectedCourse.Enrollments;
                }

                return View(viewModel);
            }
            else
            {
                return HttpNotFound();
            }            
        }

   


        // GET: InstructorCourse/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollemnts.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: InstructorCourse/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, int? courseID )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var gradeToUpdate = db.Enrollemnts.Find(id);
            if(TryUpdateModel(gradeToUpdate,"",new string[] { "Grade"}))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index", "InstructorCourse", new { courseID = courseID });
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Unable to save changes. Try again later!");
                }
            }

            return View(gradeToUpdate);
        }

    
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
