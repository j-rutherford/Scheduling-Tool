using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SATTP.DATA.EF.Models;

namespace SATTP.UI.MVC.Controllers;

[Authorize(Roles = "Admin")]
public class CoursesController : Controller
{
    private readonly SATTPContext _context;

    public CoursesController(SATTPContext context)
    {
        _context = context;
    }


    // GET: Courses
    public async Task<IActionResult> Index(bool? isActive)
    {
        //if (isActive == null)
        //{
        //    return View(await _context.Courses.ToListAsync());
        //}

        //return View(await _context.Courses.Where(x=>x.IsActive == isActive).ToListAsync());
        return RedirectToAction("Active");
    }
    public async Task<IActionResult> Active()
    {
        HttpContext.Session.SetString("previous", "Active");
        return View(await _context.Courses.Where(x => x.IsActive).ToListAsync());
    }

    public async Task<IActionResult> Retired()
    {
        HttpContext.Session.SetString("previous", "Retired");
        return View(await _context.Courses.Where(x => !x.IsActive).ToListAsync());
    }


    // GET: Courses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        
        if (id == null || _context.Courses == null)
        {
            return NotFound();
        }

        var course = await _context.Courses
            .Include(c => c.ScheduledClasses)
            .ThenInclude(sc => sc.Enrollments)
            .ThenInclude(e => e.Student)
            .FirstOrDefaultAsync(m => m.CourseId == id);
            
        if (course == null)
        {
            return NotFound();
        }

        ViewBag.Previous = HttpContext.Session.GetString("previous");
        return View(course);
    }

    // GET: Courses/Create
    public IActionResult Create()
    {
        ViewBag.Previous = HttpContext.Session.GetString("previous");
        return View();
    }

    // POST: Courses/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CourseId,CourseName,CourseDescription,CreditHours,Curriculum,Notes,IsActive")] Course course)
    {
        var previous = HttpContext.Session.GetString("previous");
        if (ModelState.IsValid)
        {
            course.IsActive = true; //default value
            _context.Add(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(previous);
        }
        ViewBag.Previous = previous;
        return View(course);
    }

    // GET: Courses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Courses == null)
        {
            return NotFound();
        }

        var course = await _context.Courses.FindAsync(id);
        if (course == null)
        {
            return NotFound();
        }
        ViewBag.Previous = HttpContext.Session.GetString("previous");
        return View(course);
    }

    // POST: Courses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseName,CourseDescription,CreditHours,Curriculum,Notes,IsActive")] Course course)
    {
        var previous = HttpContext.Session.GetString("previous");

        if (id != course.CourseId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(course);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(course.CourseId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(previous);
        }
        ViewBag.Previous = previous;
        return View(course);
    }

    //bit flip
    public async Task<IActionResult> Toggle(int? id)
    {
        if (id == null || _context.Courses == null)
        {
            return NotFound();
        }

        var course = await _context.Courses
            .FirstOrDefaultAsync(m => m.CourseId == id);
        if (course == null)
        {
            return NotFound();
        }

        if (course.IsActive)
        {
            course.IsActive = false;
        }
        else
        {
            course.IsActive = true;
        }

        _context.Update(course);
        await _context.SaveChangesAsync();

        //v1
        //return RedirectToAction("Index");

        //v2
        string previous = HttpContext.Session.GetString("previous");
        return RedirectToAction(previous);
    }


    // GET: Courses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Courses == null)
        {
            return NotFound();
        }

        var course = await _context.Courses
            .FirstOrDefaultAsync(m => m.CourseId == id);
        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }

    // POST: Courses/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Courses == null)
        {
            return Problem("Entity set 'sattpContext.Courses'  is null.");
        }
        var course = await _context.Courses.FindAsync(id);
        if (course != null)
        {
            _context.Courses.Remove(course);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CourseExists(int id)
    {
        return (_context.Courses?.Any(e => e.CourseId == id)).GetValueOrDefault();
    }
}
