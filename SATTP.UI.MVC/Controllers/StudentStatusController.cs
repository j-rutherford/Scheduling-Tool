using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SATTP.DATA.EF.Models;

namespace SATTP.UI.MVC.Controllers
{
    public class StudentStatusController : Controller
    {
        private readonly SATTPContext _context;

        public StudentStatusController(SATTPContext context)
        {
            _context = context;
        }

        // GET: StudentStatus
        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("previous", "Index");
            return _context.StudentStatuses != null ?
                        View(await _context.StudentStatuses.Include(ss => ss.Students).ToListAsync()) :
                        Problem("Entity set 'SATTPContext.StudentStatuses'  is null.");
        }
        public async Task<IActionResult> IndexTiles()
        {
            HttpContext.Session.SetString("previous", "IndexTiles");

            return _context.StudentStatuses != null ?
                    View(await _context.StudentStatuses.Include(ss => ss.Students).ToListAsync()) :
                    Problem("Entity set 'SATTPContext.StudentStatuses' is null.");

        }
        // GET: StudentStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StudentStatuses == null)
            {
                return NotFound();
            }

            var studentStatus = await _context.StudentStatuses
                .FirstOrDefaultAsync(m => m.SSID == id);
            if (studentStatus == null)
            {
                return NotFound();
            }
            ViewBag.Previous = HttpContext.Session.GetString("previous");
            return View(studentStatus);
        }

        // GET: StudentStatus/Create
        public IActionResult Create()
        {
            ViewBag.Previous = HttpContext.Session.GetString("previous");
            return View();
        }

        // POST: StudentStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SSID,SSName,SSDescription")] StudentStatus studentStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(HttpContext.Session.GetString("previous"));
            }
            ViewBag.Previous = HttpContext.Session.GetString("previous");
            return View(studentStatus);
        }

        // GET: StudentStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StudentStatuses == null)
            {
                return NotFound();
            }

            var studentStatus = await _context.StudentStatuses.FindAsync(id);
            if (studentStatus == null)
            {
                return NotFound();
            }
            ViewBag.Previous = HttpContext.Session.GetString("previous");
            return View(studentStatus);
        }

        // POST: StudentStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SSID,SSName,SSDescription")] StudentStatus studentStatus)
        {
            if (id != studentStatus.SSID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentStatusExists(studentStatus.SSID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(HttpContext.Session.GetString("previous"));
            }

            ViewBag.Previous = HttpContext.Session.GetString("previous");
            return View(studentStatus);            
        }

        // GET: StudentStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StudentStatuses == null)
            {
                return NotFound();
            }

            var studentStatus = await _context.StudentStatuses
                .FirstOrDefaultAsync(m => m.SSID == id);
            if (studentStatus == null)
            {
                return NotFound();
            }

            return View(studentStatus);
        }

        // POST: StudentStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StudentStatuses == null)
            {
                return Problem("Entity set 'sattpContext.StudentStatuses'  is null.");
            }
            var studentStatus = await _context.StudentStatuses.FindAsync(id);
            if (studentStatus != null)
            {
                _context.StudentStatuses.Remove(studentStatus);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(HttpContext.Session.GetString("previous"));
        }

        private bool StudentStatusExists(int id)
        {
          return (_context.StudentStatuses?.Any(e => e.SSID == id)).GetValueOrDefault();
        }
    }
}
