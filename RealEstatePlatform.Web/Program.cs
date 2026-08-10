using Microsoft.EntityFrameworkCore;
using RealEstatePlatform.DataAccess.Contexts;
using RealEstatePlatform.Core.Interfaces;
using RealEstatePlatform.DataAccess.Repositories;
using RealEstatePlatform.Business.Interfaces;
using RealEstatePlatform.Business.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Generic Repository Enjeksiyonu
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Servis Enjeksiyonları
builder.Services.AddScoped<IPropertyService, PropertyService>();

builder.Services.AddDbContext<RealEstateDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Property}/{action=Index}/{id?}");


app.Run();
