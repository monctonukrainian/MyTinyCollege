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

namespace MyTinyCollege.Controllers
{

    //jkhalack: adding authorization for admin users only 
    //[Authorize] anybody who logs in
    [Authorize(Roles = "admin")]
    public class CourseController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Course
        //jkhalack: added Selected Department filter
        public ActionResult Index(int? SelectedDepartment)
        {
            //Selected Department filter
            

            IQueryable<Course> courses = GetCourses(SelectedDepartment);

            //var courses = db.Courses.Include(c => c.Department);
            return View(courses.ToList());
        }

        private IQueryable<Course> GetCourses(int? selectedDepartment)
        {
            var departments = db.Departments.OrderBy(d => d.Name).ToList();
            ViewBag.SelectedDepartment = new SelectList(departments,
                "DepartmentID", "Name", selectedDepartment);
            //Get the selected department's value (id = 1, 2, 3, etc...)
            int departmentID = selectedDepartment.GetValueOrDefault();

            //for example if the user selected engineering (departmentID = 1)
            //build a query to return all courses for department id =1
            IQueryable<Course> courses = db.Courses
                .Where(c => !selectedDepartment.HasValue || c.DepartmentID == departmentID)
                .OrderBy(d => d.CourseID)
                .Include(d => d.Department);
            return courses;
        }

        [AllowAnonymous]
        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name");
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits,DepartmentID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Title,Credits,DepartmentID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateCourseCredits()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateCourseCredits(int? multiplier)
        {
            if (multiplier!=null)
            {
                /*
                 *Raw SQL query
                 * Use the DataBase.SqlQuery method for queries that return types that are not entties.
                 * the returned data isn't tracked by the database context object 
                 */
                ViewBag.RowsAffected = db.Database.ExecuteSqlCommand( 
                    "UPDATE Course SET credits = credits * {0}", multiplier);
                var test = db.Database.SqlQuery<MyTinyCollege.Models.Course>("Select top 3 * FROM Course ORDER by 2");
            }
            return View();
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
