using DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
//using Tracker_UI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<TournamentsContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnectionString"))
);

string saveLocation = builder.Configuration.GetValue<string>("AppSettings:SaveLocation");

if (saveLocation == "sql")
{
    builder.Services.AddScoped<IDataConnection, SqlConnector>();
} else if (saveLocation == "text")
{
    builder.Services.AddScoped<IDataConnection, TextConnector>();
}


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
    //pattern: "{controller=Team}/{action=Create}/{id?}");
pattern: "{controller=Tournament}/{action=Create}/{id?}");

app.MapRazorPages();

app.Run();

