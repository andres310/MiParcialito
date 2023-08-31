using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiParcialito.Models;
using MiParcialito.Models.ViewModel;

namespace MiParcialito.Controllers;

[Authorize(Roles = "Admin")]
public class MyUserRoleController : Controller
{
    private readonly MyDbContext _context;

    public MyUserRoleController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // TODO: Quiza mostrar los usuarios y su rol osea dos columnas?
        return _context.AspNetUsers != null ? 
            View(await _context.AspNetUsers.ToListAsync()) :
            Problem("Entity set 'MyDbContext.AspNetUsers'  is null.");
    }
    
    // Get Details
    [HttpGet]
    public async Task<IActionResult> Details()
    {
        // TODO: Otro todo bien hecho xd
        return null;
    }
    
    // Get del create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdUsuario, IdRol")] UserRoleViewModel userRoleViewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Encuentra segun los ids
                AspNetUser user = await _context.AspNetUsers.FindAsync(userRoleViewModel.IdUsuario);
                AspNetRole rol = await _context.AspNetRoles.FindAsync(userRoleViewModel.IdRol);
                // Persiste el usuario con su rol
                user.Roles.Add(rol);
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        return View(userRoleViewModel);
    }
}