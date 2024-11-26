using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using CSRMGMT;
using CSRMGMT.Models;
using CSRMGMT.Repository;
using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
var connString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<AppdbContext>(options => options.UseSqlServer(connString));
builder.Services.AddSingleton<IFileProvider>(
                  new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
                  );
//builder.Services.AddIdentityApiEndpoints<AppUser>();

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<AppdbContext>();

// Register repository
builder.Services.AddScoped<ILookupRepository, LookupRepository>();

var app = builder.Build();


//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    //try
//    {
//        var userManager = services.GetRequiredService<UserManager<AppUser>>();
//        //var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

//        //var role = await roleManager.FindByNameAsync("Member");
//        //if (role != null)
//        //{
//        //    var result = await roleManager.DeleteAsync(role);
//        //}

//        //// Seed roles
//        //string[] roleNames = { "Admin", "Client", "Agency" };
//        //foreach (var roleName in roleNames)
//        //{
//        //    var roleExist = roleManager.RoleExistsAsync(roleName).Result;
//        //    if (!roleExist)
//        //    {
//        //        var roleResult = roleManager.CreateAsync(new IdentityRole(roleName)).Result;
//        //    }
//        //}

//        var user = await userManager.FindByNameAsync("subodh@v2web.in");
//        if (user!= null)
//        {
//            var result = await userManager.DeleteAsync(user);
//        }


//    }
//}
//        // Seed a default admin user
//        var adminEmail = "admin@example.com";
//        var adminUser = userManager.FindByEmailAsync(adminEmail).Result;
//        if (adminUser == null)
//        {
//            var newAdminUser = new AppUser
//            {
//                UserName = adminEmail,
//                Email = adminEmail,
//                // Add additional properties as needed
//            };
//            var result = userManager.CreateAsync(newAdminUser, "Admin123!").Result;
//            if (result.Succeeded)
//            {
//                userManager.AddToRoleAsync(newAdminUser, "Admin").Wait();
//            }
//        }
//    }
//    //catch (Exception ex)
//    //{
//    //    var logger = services.GetRequiredService<ILogger<Program>>();
//    //    logger.LogError(ex, "An error occurred while seeding the database.");
//    //}
//}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//pattern: "{controller=Account}/{action=Login}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
    );
    //endpoints.MapControllerRoute(
    //name: "Cms",
    //pattern: "pages/{title?}/{id?}",
    //defaults: new { controller = "Home", action = "Page" });
});
app.Run();
