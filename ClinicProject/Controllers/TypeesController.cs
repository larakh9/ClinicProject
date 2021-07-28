using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicProject.Data;
using ClinicProject.Models;

namespace ClinicProject.Controllers
{
    public class TypeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Typees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Types.ToListAsync());
        }

        // GET: Typees/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typee = await _context.Types
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typee == null)
            {
                return NotFound();
            }

            return View(typee);
        }

        // GET: Typees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Typees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppointmentType")] Typee typee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typee);
        }

        // GET: Typees/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typee = await _context.Types.FindAsync(id);
            if (typee == null)
            {
                return NotFound();
            }
            return View(typee);
        }

        // POST: Typees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,AppointmentType")] Typee typee)
        {
            if (id != typee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeeExists(typee.Id))
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
            return View(typee);
        }

        // GET: Typees/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typee = await _context.Types
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typee == null)
            {
                return NotFound();
            }

            return View(typee);
        }

        // POST: Typees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var typee = await _context.Types.FindAsync(id);
            _context.Types.Remove(typee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeeExists(long id)
        {
            return _context.Types.Any(e => e.Id == id);
        }
    }
}
