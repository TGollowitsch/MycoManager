using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MycoManager.Data;
using MycoManager.Models;

namespace MycoManager.Controllers
{
    public class MycoStrainsController : Controller
    {
        private readonly MycoDbContext _context;

        public MycoStrainsController(MycoDbContext context)
        {
            _context = context; 
        }

        // GET: MycoStrains
        public async Task<IActionResult> Index()
        {
            return View(await _context.MycoStrains.ToListAsync());
        }

        // GET: MycoStrains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mycoStrain = await _context.MycoStrains
                .FirstOrDefaultAsync(m => m.ID == id);
            if (mycoStrain == null)
            {
                return NotFound();
            }

            return View(mycoStrain);
        }

        // GET: MycoStrains/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MycoStrains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MycoStrain mycoStrain, IFormFile imageFile)
        {
            if (await S3Service.UploadFile(imageFile.OpenReadStream(), mycoStrain.S3ObjectName))
            {
                _context.Add(mycoStrain);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: MycoStrains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mycoStrain = await _context.MycoStrains.FindAsync(id);
            if (mycoStrain == null)
            {
                return NotFound();
            }
            return View(mycoStrain);
        }

        // POST: MycoStrains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Species,Name,S3ObjectName")] MycoStrain mycoStrain, IFormFile imageFile)
        {
            if (id != mycoStrain.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mycoStrain);
                    if (imageFile != null)
                    {
                        await S3Service.UploadFile(imageFile.OpenReadStream(), mycoStrain.S3ObjectName);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MycoStrainExists(mycoStrain.ID))
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
            return View(mycoStrain);
        }

        // GET: MycoStrains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mycoStrain = await _context.MycoStrains
                .FirstOrDefaultAsync(m => m.ID == id);
            if (mycoStrain == null)
            {
                return NotFound();
            }

            return View(mycoStrain);
        }

        // POST: MycoStrains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mycoStrain = await _context.MycoStrains.FindAsync(id);
            if (mycoStrain != null)
            {
                _context.MycoStrains.Remove(mycoStrain);
                await S3Service.DeleteObject(mycoStrain.S3ObjectName);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MycoStrainExists(int id)
        {
            return _context.MycoStrains.Any(e => e.ID == id);
        }
    }
}
