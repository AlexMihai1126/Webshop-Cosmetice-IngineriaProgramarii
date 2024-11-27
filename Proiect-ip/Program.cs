using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Data;
using Proiect_ip.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Proiect_ipContextConnection") ?? throw new InvalidOperationException("Connection string 'Proiect_ipContextConnection' not found.");

builder.Services.AddDbContext<Proiect_ipContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<Proiect_ipUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // folosire roluri
    .AddEntityFrameworkStores<Proiect_ipContext>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Initializare roluri la pornirea aplicatiei
using (var scope = app.Services.CreateScope())
{
    await SeedRolesAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

static async Task SeedRolesAsync(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Client", "Admin" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
