using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//string sqlConnectionString = builder.Configuration.GetConnectionString("SqlEmpleados");
//string oracleConnectionString = builder.Configuration.GetConnectionString("OracleEmpleados");
string mySqlConnectionString = builder.Configuration.GetConnectionString("MySqlEmpleados");

builder.Services.AddDbContext<EmpleadosContext>(options =>
{
    //options.UseSqlServer(sqlConnectionString);
    //options.UseOracle(oracleConnectionString);
    options.UseMySQL(mySqlConnectionString);
});

//builder.Services.AddTransient<IRepositoryEmpleados, RepositoryEmpleadosSqlServer>();

//builder.Services.AddTransient<IRepositoryEmpleados, RepositoryEmpleadosOracle>();

builder.Services.AddTransient<IRepositoryEmpleados, RepositoryEmpleadosMySql>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.UseCookiePolicy();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
