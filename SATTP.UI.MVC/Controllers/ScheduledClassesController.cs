using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SATTP.DATA.EF.Models;

namespace SATTP.UI.MVC.Controllers
{
    public class ScheduledClassesController : Controller
    {
        private readonly SATTPContext _context;

        public ScheduledClassesController(SATTPContext context)
        {
            _context = context;
        }

        //status change for scheduled courses
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int? id, int scsid)
        {
            var scheduledClass = await _context.ScheduledClasses.FindAsync(id);
            if (scheduledClass != null)
            {
                scheduledClass.SCSID = scsid;
                _context.Update(scheduledClass);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Disenroll(int scid, int studentId)
        {
            //find the course, make sure it exists.
            var scheduledClass = await _context.ScheduledClasses.FindAsync(scid);
            if (scheduledClass != null)
            {
                //find the enrollment to remove.
                var remove = _context.Enrollments.FirstOrDefault(x => x.StudentId == studentId && x.ScheduledClassID == scid);

                //remove the enrollment.
                _context.Enrollments.Remove(remove);

            }
            //update the db
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = scid });
        }

        // GET: ScheduledClasses
        public async Task<IActionResult> Index()
        {
            var scheduledClasses = _context.ScheduledClasses
                .Include(s => s.Course)
                .Include(s => s.ScheduledClassStatus)
                .OrderBy(x=>x.SCSID)
                .ThenByDescending(x=>x.StartDate);
            ViewBag.Statuses = new SelectList(_context.ScheduledClassStatuses, "SCSID", "SCSName", "Update");
            return View(await scheduledClasses.ToListAsync());
        }

        // GET: ScheduledClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ScheduledClasses == null)
            {
                return NotFound();
            }

            var scheduledClass = await _context.ScheduledClasses
                .Include(s => s.Course)
                .Include(s => s.ScheduledClassStatus)
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(m => m.ScheduledClassID == id);
            if (scheduledClass == null)
            {
                return NotFound();
            }

            return View(scheduledClass);
        }

        // GET: ScheduledClasses/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseName");
            ViewData["SCSID"] = new SelectList(_context.ScheduledClassStatuses, "SCSID", "SCSName");
            return View();
        }

        // POST: ScheduledClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduledClassID,CourseId,StartDate,EndDate,InstructorName,Location,SCSID")] ScheduledClass scheduledClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scheduledClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseDescription", scheduledClass.CourseId);
            ViewData["SCSID"] = new SelectList(_context.ScheduledClassStatuses, "SCSID", "SCSName", scheduledClass.SCSID);
            return View(scheduledClass);
        }

        // GET: ScheduledClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ScheduledClasses == null)
            {
                return NotFound();
            }

            var scheduledClass = await _context.ScheduledClasses.FindAsync(id);
            if (scheduledClass == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseDescription", scheduledClass.CourseId);
            ViewData["SCSID"] = new SelectList(_context.ScheduledClassStatuses, "SCSID", "SCSName", scheduledClass.SCSID);
            return View(scheduledClass);
        }

        // POST: ScheduledClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScheduledClassID,CourseId,StartDate,EndDate,InstructorName,Location,SCSID")] ScheduledClass scheduledClass)
        {
            if (id != scheduledClass.ScheduledClassID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scheduledClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduledClassExists(scheduledClass.ScheduledClassID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseDescription", scheduledClass.CourseId);
            ViewData["SCSID"] = new SelectList(_context.ScheduledClassStatuses, "SCSID", "SCSName", scheduledClass.SCSID);
            return View(scheduledClass);
        }

        // GET: ScheduledClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ScheduledClasses == null)
            {
                return NotFound();
            }

            var scheduledClass = await _context.ScheduledClasses
                .Include(s => s.Course)
                .Include(s => s.ScheduledClassStatus)
                .FirstOrDefaultAsync(m => m.ScheduledClassID == id);
            if (scheduledClass == null)
            {
                return NotFound();
            }

            return View(scheduledClass);
        }

        // POST: ScheduledClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ScheduledClasses == null)
            {
                return Problem("Entity set 'sattpContext.ScheduledClasses'  is null.");
            }
            var scheduledClass = await _context.ScheduledClasses.FindAsync(id);
            if (scheduledClass != null)
            {
                _context.ScheduledClasses.Remove(scheduledClass);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduledClassExists(int id)
        {
            return (_context.ScheduledClasses?.Any(e => e.ScheduledClassID == id)).GetValueOrDefault();
        }
    }
}
