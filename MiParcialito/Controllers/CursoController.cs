using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiParcialito.Data;
using MiParcialito.Models;
using MiParcialito.Models.ViewModel;

namespace MiParcialito.Controllers
{
    public class CursoController : Controller
    {
        private readonly MyDbContext _context;
        private readonly ApplicationDbContext _applicationDbContext;
        //private readonly UserManager<IdentityUser> _userManager;

        public CursoController(MyDbContext context, /*UserManager<IdentityUser> userManager,*/ ApplicationDbContext applicationDbContext)
        {
            _context = context;
            //_userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        // GET: Curso
        public async Task<IActionResult> Index()
        {
            //var myDbContext = _context.Cursos.Include(c => c.IdDocenteNavigation);
            var myDbContext = await _context.Cursos
                .OrderBy(c => c.Nombre)
                .Select(c => new CursoDocenteViewModel
                {
                    Id = c.Id,
                    NombreDocente = c.IdDocenteNavigation.IdUsuarioNavigation.UserName,
                    NombreCurso = c.Nombre
                }).ToListAsync();

            return View(myDbContext);
        }

        [HttpGet]
        public IActionResult Inscribir(int idCurso)
        {
            Curso model = _context.Cursos.Find(idCurso);
            return View(model);
        }

        // Recibe id del curso que se desea inscribir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inscribir(int? usuarioId, Curso curso)
        {
            // Obtiene usuario de la sesion
            Estudiante estudiante = _context.Estudiantes
                .First(estudiante => estudiante.IdUsuarioNavigation.Id.Equals(usuarioId));

            //Curso curso = await _context.Cursos.FindAsync();
            
            var nuevaInscripcion = new Inscripcione
            {
                IdCurso = curso.Id,
                IdEstudiante = estudiante.Id
            };
            _context.Add(nuevaInscripcion);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Inscripcione");
        }

        // GET: Curso/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .Include(c => c.IdDocenteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // GET: Curso/Create
        public IActionResult Create()
        {
            ViewData["IdDocente"] = new SelectList(_context.Docentes, "Id", "Id");
            return View();
        }

        // POST: Curso/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdDocente,Nombre")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(curso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDocente"] = new SelectList(_context.Docentes, "Id", "Id", curso.IdDocente);
            return View(curso);
        }

        // GET: Curso/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }
            ViewData["IdDocente"] = new SelectList(_context.Docentes, "Id", "Id", curso.IdDocente);
            return View(curso);
        }

        // POST: Curso/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdDocente,Nombre")] Curso curso)
        {
            if (id != curso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CursoExists(curso.Id))
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
            ViewData["IdDocente"] = new SelectList(_context.Docentes, "Id", "Id", curso.IdDocente);
            return View(curso);
        }

        // GET: Curso/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .Include(c => c.IdDocenteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // POST: Curso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cursos == null)
            {
                return Problem("Entity set 'MyDbContext.Cursos'  is null.");
            }
            var curso = await _context.Cursos.FindAsync(id);
            if (curso != null)
            {
                _context.Cursos.Remove(curso);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CursoExists(int id)
        {
          return (_context.Cursos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
