using Data_Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//tao duong dan
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 2;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAntiforgery(options => options.HeaderName = "XSRF-TOKEN");



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

app.Run();


   //< PackageReference Include = "ErikEJ.EntityFrameworkCore.SqlServer.DateOnlyTimeOnly" Version = "7.0.8" />


   //   < PackageReference Include = "Microsoft.AspNetCore.Identity" Version = "2.2.0" />

   //   < PackageReference Include = "Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version = "7.0.14" />

   //   < PackageReference Include = "Microsoft.EntityFrameworkCore" Version = "7.0.14" />

   //   < PackageReference Include = "Microsoft.EntityFrameworkCore.SqlServer" Version = "7.0.14" />

   //   < PackageReference Include = "Microsoft.EntityFrameworkCore.Tools" Version = "7.0.14" />
