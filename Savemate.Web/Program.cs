
using Microsoft.AspNetCore.Identity;
using Savemate.Infrastructure;
using Savemate.Infrastructure.IoC;
 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
DependencyInjection.IdentityOption(builder.Services);  
DependencyInjection.AddingDbContext(builder.Services, builder.Configuration);
DependencyInjection.Registering(builder.Services);
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<SaveMateDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(opt => {
    opt.LoginPath = "/Authentication/Login";
    opt.Cookie.Name = ".AspNetCore.Identity.Application";
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    opt.SlidingExpiration = true;


        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
