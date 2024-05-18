using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MycoManager.Data;
using MycoManager.Models;

namespace MycoManager.Controllers
{
    public class EdgeSensorsController : Controller
    {
        private readonly MycoDbContext _context;

        public EdgeSensorsController(MycoDbContext context)
        {
            _context = context;
        }

        // GET: EdgeSensors
        public async Task<IActionResult> Index()
        {
            return View(await _context.EdgeSensors.ToListAsync());
        }

        // GET: EdgeSensors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var edgeSensor = await _context.EdgeSensors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (edgeSensor == null)
            {
                return NotFound();
            }

            return View(edgeSensor);
        }

        // GET: EdgeSensors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EdgeSensors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Uri,SetTemperature,SetHumidity,SetCO2PPM")] EdgeSensor edgeSensor)
        {
            if (ModelState.IsValid)
            {
                await LambdaService.PublishLambdaFunctionFromS3(edgeSensor.Name);
                edgeSensor.Uri = await ApiGatewayService.IntegrateFunctionToApi(edgeSensor.Name);
                _context.Add(edgeSensor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(edgeSensor);
        }

        // GET: EdgeSensors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var edgeSensor = await _context.EdgeSensors.FindAsync(id);
            if (edgeSensor == null)
            {
                return NotFound();
            }
            return View(edgeSensor);
        }

        // POST: EdgeSensors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Uri,SetTemperature,SetHumidity,SetCO2PPM")] EdgeSensor edgeSensor)
        {
            if (id != edgeSensor.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(edgeSensor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EdgeSensorExists(edgeSensor.ID))
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
            return View(edgeSensor);
        }

        // GET: EdgeSensors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var edgeSensor = await _context.EdgeSensors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (edgeSensor == null)
            {
                return NotFound();
            }

            return View(edgeSensor);
        }

        // POST: EdgeSensors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var edgeSensor = await _context.EdgeSensors.FindAsync(id);
            if (edgeSensor != null)
            {
                await ApiGatewayService.RemoveFunctionFromApi(edgeSensor.Name);
                await LambdaService.DeleteLambdaFunction(edgeSensor.Name);
                _context.EdgeSensors.Remove(edgeSensor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EdgeSensorExists(int id)
        {
            return _context.EdgeSensors.Any(e => e.ID == id);
        }
    }
}
