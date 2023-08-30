using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebProject.Areas.Identity.Data;
using WebProject.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AuthDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AuthDbContextConnection' not found.");

builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<AuthDbContext>()
	.AddDefaultUI()
	.AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireUppercase = false;
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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
