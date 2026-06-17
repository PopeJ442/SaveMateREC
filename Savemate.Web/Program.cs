using Microsoft.AspNetCore.Identity;
using Savemate.Application.Common;
using Savemate.Infrastructure;
using Savemate.Infrastructure.IoC;
using Savemate.Web.Middleware;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/savemate-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Savemate application starting up");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddControllersWithViews(); 

    DependencyInjection.AddingDbContext(builder.Services, builder.Configuration);
    DependencyInjection.Registering(builder.Services);

    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<SaveMateDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

    builder.Services.ConfigureApplicationCookie(opt =>
    {
        opt.LoginPath = "/Authentication/Login";
        opt.LogoutPath = "/Authentication/Logout";
        opt.Cookie.Name = ".AspNetCore.Identity.Application";
        opt.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        opt.SlidingExpiration = true;
    });

    builder.Services.AddAuthentication()
        .AddGoogle(opts =>
        {
            opts.ClientId = "849398442796-kff7i6gb0sa754k8mhpktsqbrhbmguj0.apps.googleusercontent.com";
                    opts.ClientSecret = "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";
                      opts.SignInScheme = IdentityConstants.ExternalScheme;
        });

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseMiddleware<ExceptionalLoggingMiddleware>();
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    Log.Information("Savemate application started successfully");

    app.Run();
}

catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}