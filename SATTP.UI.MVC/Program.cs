using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SATTP.DATA.EF.Models;
using SATTP.UI.MVC.Data;

namespace SATTP.UI.MVC;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDbContext<SATTPContext>(options => options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddRoleManager<RoleManager<IdentityRole>>().AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();
        // Added the Session Service
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(20);//time a session is stored in memory 
            options.Cookie.HttpOnly = true;//allow us to set cookie options over unsecure connections
            options.Cookie.IsEssential = true;//can't be declined
        });

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

        app.UseSession();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}