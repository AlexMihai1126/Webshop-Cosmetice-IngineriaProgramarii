using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Data;
using Proiect_ip.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Proiect_ipContextConnection") ?? throw new InvalidOperationException("Connection string 'Proiect_ipContextConnection' not found.");

builder.Services.AddDbContext<Proiect_ipContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<Proiect_ipUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<Proiect_ipContext>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
