using Impact.DAL;
using Impact.Models;
using Impact.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt=>opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<AppUser, IdentityRole>(opt => {

	opt.Password.RequireNonAlphanumeric = false;

	opt.User.RequireUniqueEmail = true;

	opt.Lockout.MaxFailedAccessAttempts = 3;
	opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);

}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureExternalCookie(cfg =>
{
	cfg.LoginPath = $"/Admin/Account/Login/{cfg.ReturnUrlParameter}";
});
builder.Services.AddScoped<LayoutService>();




var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();



app.MapControllerRoute(

    "default",
    "{area:exists}/{controller=Home}/{action=Index}/{id?}"

    );

app.MapControllerRoute(

	"default",
	"{controller=Home}/{action=Index}/{id?}"

	);

app.Run();
