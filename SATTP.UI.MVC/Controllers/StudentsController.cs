using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SATTP.DATA.EF.Models;
using SATTP.UI.MVC.Utilities;

namespace SATTP.UI.MVC.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SATTPContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public StudentsController(SATTPContext context, IWebHostEnvironment webHostEnvironment )
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;//added
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var students = _context.Students
                .Include(s => s.StudentStatus)
                .Include(s=>s.Enrollments);
            ViewBag.Warning = HttpContext.Session.GetString("warning");
            HttpContext.Session.Remove("warning");
            return View(await students.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.StudentStatus)
                .Include(s => s.Enrollments)
                .ThenInclude(e=> e.ScheduledClass)
                .ThenInclude(sc=>sc.Course)                
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["SSID"] = new SelectList(_context.StudentStatuses, "SSID", "SSName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,FirstName,LastName,Major,Address,City,State,ZipCode,Phone,Email,PhotoUrl,SSID,Image")] Student student)
        {
            if (ModelState.IsValid)
            {
                #region File Upload - CREATE
                //Check to see if a file was uploaded
                if (student.Image != null)
                {
                    //Check the file type 
                    //- retrieve the extension of the uploaded file
                    string ext = Path.GetExtension(student.Image.FileName);

                    //- Create a list of valid extensions to check against
                    string[] validExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    //- verify the uploaded file has an extension matching one of the extensions in the list above
                    //- AND verify file size will work with our .NET app
                    if (validExts.Contains(ext.ToLower()) && student.Image.Length < 4_194_303)//underscores don't change the number, they just make it easier to read
                    {
                        //Generate a unique filename
                        student.PhotoUrl = Guid.NewGuid() + ext;

                        //Save the file to the web server (here, saving to wwwroot/images)
                        //To access wwwroot, add a property to the controller for the _webHostEnvironment (see the top of this class for our example)
                        //Retrieve the path to wwwroot
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        //variable for the full image path --> this is where we will save the image
                        string fullImagePath = webRootPath + "/StudentImages/";

                        //Create a MemoryStream to read the image into the server memory
                        using (var memoryStream = new MemoryStream())
                        {
                            await student.Image.CopyToAsync(memoryStream);//transfer file from the request to server memory
                            using (var img = Image.FromStream(memoryStream))//add a using statement for the Image class (using System.Drawing)
                            {
                                //now, send the image to the ImageUtility for resizing and thumbnail creation
                                //items needed for the ImageUtility.ResizeImage()
                                //1) (int) maximum image size
                                //2) (int) maximum thumbnail image size
                                //3) (string) full path where the file will be saved
                                //4) (Image) an image
                                //5) (string) filename
                                int maxImageSize = 500;//in pixels
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(fullImagePath, student.PhotoUrl, img, maxImageSize, maxThumbSize);
                                //myFile.Save("path/to/folder", "filename"); - how to save something that's NOT an image

                            }
                        }
                    }
                }
                else
                {
                    //If no image was uploaded, assign a default filename
                    //Will also need to download a default image and name it 'noimage.png' -> copy it to the /images folder
                    student.PhotoUrl = "noimage.png";
                }

                #endregion


                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SSID"] = new SelectList(_context.StudentStatuses, "SSID", "SSName", student.SSID);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["SSID"] = new SelectList(_context.StudentStatuses, "SSID", "SSName", student.SSID);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FirstName,LastName,Major,Address,City,State,ZipCode,Phone,Email,PhotoUrl,SSID,Image")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                #region EDIT File Upload
                //retain old image file name so we can delete if a new file was uploaded
                string oldImageName = student.PhotoUrl;

                //Check if the user uploaded a file
                if (student.Image != null)
                {
                    //get the file's extension
                    string ext = Path.GetExtension(student.Image.FileName);

                    //list valid extensions
                    string[] validExts = { ".jpeg", ".jpg", ".png", ".gif" };

                    //check the file's extension against the list of valid extensions
                    if (validExts.Contains(ext.ToLower()) && student.Image.Length < 4_194_303)
                    {
                        //generate a unique file name
                        student.PhotoUrl = Guid.NewGuid() + ext;
                        //build our file path to save the image
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string fullPath = webRootPath + "/StudentImages/";

                        //Delete the old image
                        if (oldImageName != "noimage.png")
                        {
                            ImageUtility.Delete(fullPath, oldImageName);
                        }

                        //Save the new image to webroot
                        using var memoryStream = new MemoryStream();
                        await student.Image.CopyToAsync(memoryStream);
                        using var img = Image.FromStream(memoryStream);
                        int maxImageSize = 500;
                        int maxThumbSize = 100;
                        ImageUtility.ResizeImage(fullPath, student.PhotoUrl, img, maxImageSize, maxThumbSize);

                    }
                }
                #endregion


                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
            ViewData["SSID"] = new SelectList(_context.StudentStatuses, "SSID", "SSName", student.SSID);
            return View(student);
        }


        //Soft delete
        public async Task<IActionResult> Toggle(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.StudentStatus)
                .Include(s => s.Enrollments)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            if (student.SSID == 2)
            {
                if (!student.Enrollments.Any())
                {
                    student.SSID = 3;
                }
                //If the front-end is locked appropriately, this else should never run. The "Withdraw" button is disabled for any students with active enrollments.
                else
                {
                    int nbrClasses = student.Enrollments.Count;
                    HttpContext.Session.SetString("warning", $"{student.FullName} is actively enrolled in {nbrClasses} class(es).  Unable to change the status.");
                    return RedirectToAction("Index");
                }
            }
            else if (student.SSID == 3)
            {
                student.SSID = 2;
            }
            else
            {
                ViewData["SSID"] = new SelectList(_context.StudentStatuses, "SSID", "SSName", student.SSID);
                return View("Edit", student);
            }
            _context.Update(student);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.StudentStatus)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'sattpContext.Students'  is null.");
            }
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return (_context.Students?.Any(e => e.StudentId == id)).GetValueOrDefault();
        }
    }
}
