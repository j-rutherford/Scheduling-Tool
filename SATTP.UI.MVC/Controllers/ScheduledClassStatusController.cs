using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SATTP.DATA.EF.Models;

namespace SATTP.UI.MVC.Controllers
{
    public class ScheduledClassStatusController : Controller
    {
        private readonly SATTPContext _context;

        public ScheduledClassStatusController(SATTPContext context)
        {
            _context = context;
        }

        // GET: ScheduledClassStatus
        public async Task<IActionResult> Index()
        {
              return _context.ScheduledClassStatuses != null ? 
                          View(await _context.ScheduledClassStatuses.ToListAsync()) :
                          Problem("Entity set 'sattpContext.ScheduledClassStatuses'  is null.");
        }

        // GET: ScheduledClassStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ScheduledClassStatuses == null)
            {
                return NotFound();
            }

            var scheduledClassStatus = await _context.ScheduledClassStatuses
                .FirstOrDefaultAsync(m => m.SCSID == id);
            if (scheduledClassStatus == null)
            {
                return NotFound();
            }

            return View(scheduledClassStatus);
        }

        // GET: ScheduledClassStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScheduledClassStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SCSID,SCSName")] ScheduledClassStatus scheduledClassStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scheduledClassStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scheduledClassStatus);
        }

        // GET: ScheduledClassStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ScheduledClassStatuses == null)
            {
                return NotFound();
            }

            var scheduledClassStatus = await _context.ScheduledClassStatuses.FindAsync(id);
            if (scheduledClassStatus == null)
            {
                return NotFound();
            }
            return View(scheduledClassStatus);
        }

        // POST: ScheduledClassStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SCSID,SCSName")] ScheduledClassStatus scheduledClassStatus)
        {
            if (id != scheduledClassStatus.SCSID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scheduledClassStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduledClassStatusExists(scheduledClassStatus.SCSID))
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
            return View(scheduledClassStatus);
        }

        // GET: ScheduledClassStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ScheduledClassStatuses == null)
            {
                return NotFound();
            }

            var scheduledClassStatus = await _context.ScheduledClassStatuses
                .FirstOrDefaultAsync(m => m.SCSID == id);
            if (scheduledClassStatus == null)
            {
                return NotFound();
            }

            return View(scheduledClassStatus);
        }

        // POST: ScheduledClassStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ScheduledClassStatuses == null)
            {
                return Problem("Entity set 'sattpContext.ScheduledClassStatuses'  is null.");
            }
            var scheduledClassStatus = await _context.ScheduledClassStatuses.FindAsync(id);
            if (scheduledClassStatus != null)
            {
                _context.ScheduledClassStatuses.Remove(scheduledClassStatus);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduledClassStatusExists(int id)
        {
          return (_context.ScheduledClassStatuses?.Any(e => e.SCSID == id)).GetValueOrDefault();
        }
    }
}
