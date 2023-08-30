using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiParcialito.Data;
using MiParcialito.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MyConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configurando inyeccion de dependencias
builder.Services.AddScoped<MyDbContext>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser<Int32>>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireDigit = false; 
        options.Password.RequiredLength = 3; 
        options.Password.RequireNonAlphanumeric = false; 
        options.Password.RequireUppercase = false; 
        options.Password.RequireLowercase = false; 
    })
    .AddRoles<IdentityRole<Int32>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();

/*builder.Services.AddIdentity<AspNetUser, AspNetRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = false; 
    options.Password.RequiredLength = 3; 
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequireUppercase = false; 
    options.Password.RequireLowercase = false; 
}).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();*/

//builder.Services.AddScoped<UserManager<IdentityUser<Int32>>>();

// Configurando Inyeccion de dependencias para HttpContext
builder.Services.AddHttpContextAccessor();

// Configurando para permitir el uso de sesion
builder.Services.AddSession();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();