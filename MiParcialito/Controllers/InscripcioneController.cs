using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiParcialito.Models;
using MiParcialito.Models.ViewModel;

namespace MiParcialito.Controllers
{
    [Authorize(Roles = "Estudiante, Admin")]
    public class InscripcioneController : Controller
    {
        private readonly MyDbContext _context;

        public InscripcioneController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Inscripcione
        public async Task<IActionResult> Index()
        {
            //var myDbContext = _context.Inscripciones.Include(i => i.IdCursoNavigation).Include(i => i.IdEstudianteNavigation);
            var inscripcionesCurso = await _context.Inscripciones
                .OrderBy(i => i.Id)
                .Select(i => new InscripcionCursoViewModel
                {
                    Id = i.IdCurso,
                    Curso = i.IdCursoNavigation.Nombre,
                    Docente = i.IdCursoNavigation.IdDocenteNavigation.IdUsuarioNavigation.UserName
                }).ToListAsync();
            return View(inscripcionesCurso);
        }

        // GET: Inscripcione/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Inscripciones == null)
            {
                return NotFound();
            }

            var inscripcione = await _context.Inscripciones
                .Include(i => i.IdCursoNavigation)
                .Include(i => i.IdEstudianteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inscripcione == null)
            {
                return NotFound();
            }

            return View(inscripcione);
        }

        // GET: Inscripcione/Create
        public IActionResult Create()
        {
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id");
            ViewData["IdEstudiante"] = new SelectList(_context.Estudiantes, "Id", "Id");
            return View();
        }

        // POST: Inscripcione/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdEstudiante,IdCurso")] Inscripcione inscripcione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inscripcione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", inscripcione.IdCurso);
            ViewData["IdEstudiante"] = new SelectList(_context.Estudiantes, "Id", "Id", inscripcione.IdEstudiante);
            return View(inscripcione);
        }

        // GET: Inscripcione/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Inscripciones == null)
            {
                return NotFound();
            }

            var inscripcione = await _context.Inscripciones.FindAsync(id);
            if (inscripcione == null)
            {
                return NotFound();
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", inscripcione.IdCurso);
            ViewData["IdEstudiante"] = new SelectList(_context.Estudiantes, "Id", "Id", inscripcione.IdEstudiante);
            return View(inscripcione);
        }

        // POST: Inscripcione/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEstudiante,IdCurso")] Inscripcione inscripcione)
        {
            if (id != inscripcione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inscripcione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InscripcioneExists(inscripcione.Id))
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
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "Id", "Id", inscripcione.IdCurso);
            ViewData["IdEstudiante"] = new SelectList(_context.Estudiantes, "Id", "Id", inscripcione.IdEstudiante);
            return View(inscripcione);
        }

        // GET: Inscripcione/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Inscripciones == null)
            {
                return NotFound();
            }

            var inscripcione = await _context.Inscripciones
                .Include(i => i.IdCursoNavigation)
                .Include(i => i.IdEstudianteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inscripcione == null)
            {
                return NotFound();
            }

            return View(inscripcione);
        }

        // POST: Inscripcione/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Inscripciones == null)
            {
                return Problem("Entity set 'MyDbContext.Inscripciones'  is null.");
            }
            var inscripcione = await _context.Inscripciones.FindAsync(id);
            if (inscripcione != null)
            {
                _context.Inscripciones.Remove(inscripcione);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InscripcioneExists(int id)
        {
          return (_context.Inscripciones?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
