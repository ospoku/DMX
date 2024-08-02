using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using DMX.Authorization;
using DMX.Data;
using DMX.Models;
using DMX.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string? settingsMail = builder.Configuration["Settings:AppEmail"];
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<SMSService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Specify the login page URL
    options.AccessDeniedPath = "/Account/AccessDenied"; // Specify the access denied page URL
});


builder.Services.AddSingleton<HttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IAuthorizationHandler, OwnerAuthorizationHandler>();
IServiceCollection serviceCollection = builder.Services.AddAuthorization(options => options.AddPolicy("OwnerPolicy", policy => policy.AddRequirements(new OwnerRequirement())));
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

// Add services to the container.
builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(10));
builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });
builder.Services.AddDbContext<XContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ONLINE")));

builder.Services.AddIdentity<AppUser,AppRole>()
    .AddEntityFrameworkStores<XContext>();


builder.Services.AddControllersWithViews();
//builder.Services.AddScoped<DBInitializer>();
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
app.UseSession();
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
//var db = scope.ServiceProvider.GetRequiredService<XContext>();
//db.Database.EnsureCreatedAsync().Wait();

//var init = scope.ServiceProvider.GetRequiredService<DBInitializer>();
//await init.Initialize();

app.Run();
