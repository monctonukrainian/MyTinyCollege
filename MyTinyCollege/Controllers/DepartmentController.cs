using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyTinyCollege.DAL;
using MyTinyCollege.Models;
using MyTinyCollege.Helpers;

namespace MyTinyCollege.Controllers
{
    [Authorize(Roles = "admin")]
    public class DepartmentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Department
        public async Task<ActionResult> Index()
        {
            var departments = db.Departments.Include(d => d.Administrator);
            return View(await departments.ToListAsync());
        }

        // GET: Department/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName");
            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DepartmentID,Name,Budget,StartDate,InstructorID")] Department department, HttpPostedFileBase ImageName)
        {
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "LastName", department.InstructorID);

            if (ModelState.IsValid)
            {
                //jkhalack: added image upload
                //Note the HttpPostedFileBase ImageName ars in method
                //Also the form needs a enctype="multipart/form-data" and
                //<input type="file" id="ImageName" name="ImageName" accept="image/*" class="form-control" />

                if (ImageName !=null && ImageName.ContentLength>0)
                {
                    //do we have anything to upload - yes 
                    var validImageTypes = new string[]
                    {
                        //"image/gif",
                        //"image/jpg",
                        //"image/jpeg",
                        "image/png"
                    };
                    if (!validImageTypes.Contains(ImageName.ContentType))
                    {
                        //file being uploaded is not a png - dusplay error
                        ModelState.AddModelError("", "Please choose a PNG image.");
                        return View(department);
                    }
                    //save new department to dataBase
                    db.Departments.Add(department);
                    await db.SaveChangesAsync();
                    //retrieve the IDENTITY from SQL server
                    string pictureName = department.DepartmentID.ToString();

                    //Rename, scale, and upload image
                    ImageUpload imageUpload = new ImageUpload { Width = 128 };
                    ImageResult imageResult = imageUpload.RenameUploadFile(ImageName,pictureName);

                    return RedirectToAction("Index");
                }
                else
                {
                    //nothing to upload - display error
                    ModelState.AddModelError("", "You have not selected an image file to upload");
                    return View(department);
                }


            }

            return View(department);
        }

        // GET: Department/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DepartmentID,Name,Budget,StartDate,InstructorID")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        // GET: Department/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Department department = await db.Departments.FindAsync(id);
            db.Departments.Remove(department);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
