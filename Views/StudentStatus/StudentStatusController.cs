using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SAT.DATA.EF;

namespace SAT.UI.MVC.Controllers
{
    #region FACILITATOR NOTES
    /********************
     * REQUIREMENTS (Student Status)
     ********************
     * - Add a table format on the Index (DataTables.net)
     * - Ability to toggle between the table and a "tiled" layout ("tiled" should be a separate action)
     * - Ensure a status cannot be removed if a student is in the status
     * - If the status cannot be removed, display a list of students in the status
     */
    #endregion

    [Authorize(Roles = "Admin")] //Admin only
    public class StudentStatusController : Controller
    {
        private SATEntities db = new SATEntities();

        // GET: StudentStatus
        public ActionResult Index()
        {
            return View(db.StudentStatuses.ToList());
        }

        // added to show another example of CSS style
        public ActionResult IndexTiles()
        {
            return View(db.StudentStatuses.ToList());
        }

        // all info shown on the Index (no need for Details)
        // GET: StudentStatus/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    StudentStatus StudentStatus = db.StudentStatus.Find(id);
        //    if (StudentStatus == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(StudentStatus);
        //}

        // GET: StudentStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SSID,SSName")] StudentStatus studentStatus)
        {
            if (ModelState.IsValid)
            {
                db.StudentStatuses.Add(studentStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(studentStatus);
        }

        // GET: StudentStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentStatus studentStatus = db.StudentStatuses.Find(id);
            if (studentStatus == null)
            {
                return HttpNotFound();
            }
            return View(studentStatus);
        }

        // POST: StudentStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SSID,SSName")] StudentStatus studentStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(studentStatus);
        }

        // GET: StudentStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentStatus studentStatus = db.StudentStatuses.Find(id);
            if (studentStatus == null)
            {
                return HttpNotFound();
            }
            return View(studentStatus);
        }

        // POST: StudentStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentStatus studentStatus = db.StudentStatuses.Find(id);

            //before removing the status, check to see if any students are using it
            //! "Count" gets the # of values (contained in an object/property)
            if (studentStatus.Students.Count < 1)
            {
                //no students use the status, allow delete
                db.StudentStatuses.Remove(studentStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //students currently using the status, message the user
            ViewBag.DeleteNotAllowed = "This status is currently assigned to students and cannot be deleted.";

            //display list of students using the status to be deleted
            string studentsToUpdate = ""; //variable for student names
            
            //create a collection of student names
            var studentsInStatus =
                db.Students.Where(s => s.SSID == id) //id is the param (DeleteConfirmed action)
                .ToList()
                .Select(s => s.FullName); //retrieve student name //! reference sql select statement

            //loop and update the variable
            foreach (var student in studentsInStatus)
            {
                studentsToUpdate += student;
                studentsToUpdate += "\n"; //line break for visualization
            }

            //pass the variable to the view
            ViewBag.StudentsInStatus = studentsToUpdate;

            return View(studentStatus);
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
