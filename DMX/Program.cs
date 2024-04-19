using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using DMX.Data;
using DMX.Models;
using DMX.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddAuthentication();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<SMSService>();
builder.Services.AddSingleton<HttpContextAccessor, HttpContextAccessor>();
//  builder.  Services.AddSingleton<IAuthorizationHandler, IncidentAuthorizationHandler>();
//    builder.Services.AddAuthorization(options => options.AddPolicy("sameAuthorPolicy",
//policy =>
//policy.AddRequirements(
//    new SameAuthorRequirement()
//)));
//builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
//builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

// Add services to the container.
builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(10));
builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });
builder.Services.AddDbContext<XContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ONLINE")));

builder.Services.AddDefaultIdentity<AppUser>().AddRoles<AppRole>()
    .AddEntityFrameworkStores<XContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<DBInitializer>();
builder.Services.AddSignalR();
builder.Services.AddDataProtection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}
app.UseNotyf();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapRazorPages();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();
//app.MapHub<NotificationHub>("/notificationHub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<XContext>();
db.Database.EnsureCreated();
var init = scope.ServiceProvider.GetRequiredService<DBInitializer>();
//init.RoleCreation(scope.ServiceProvider).Wait();
//init.UserCreation(scope.ServiceProvider).Wait();

app.Run();
