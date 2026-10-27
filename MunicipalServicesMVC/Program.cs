using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Infrastructure.Data;
using MunicipalServicesMVC.Infrastructure.Repositories;
using MunicipalServicesMVC.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Database
// =========================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultDatabase")
    )
);

// =========================
// Repository
// =========================
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

// =========================
// Unit Of Work
// =========================
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// =========================
// Services
// =========================
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IServiceRequestService, ServiceRequestService>();

// =========================
// Authentication
// =========================
builder.Services
    .AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme
    )
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// =========================
// MVC
// =========================
builder.Services.AddControllersWithViews();

var app = builder.Build();

// =========================
// Configure HTTP Pipeline
// =========================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// =========================
// Authentication + Authorization
// =========================
app.UseAuthentication();

app.UseAuthorization();

// =========================
// Default Route
// =========================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
);

app.Run();