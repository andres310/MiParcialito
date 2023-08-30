using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MiParcialito.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser<Int32>, IdentityRole<Int32>, Int32>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}