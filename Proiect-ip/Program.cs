using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proiect_ip.Data;
using Proiect_ip.Areas.Identity.Data;
using Proiect_ip.Services;
using Proiect_ip.Services.DataCache;
using static System.Formats.Asn1.AsnWriter;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Proiect_ipContextConnection") ?? throw new InvalidOperationException("Connection string 'Proiect_ipContextConnection' not found.");

builder.Services.AddDbContext<Proiect_ipContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<Proiect_ipUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // folosire roluri
    .AddEntityFrameworkStores<Proiect_ipContext>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<PointsService>();
builder.Services.AddScoped<ProductSearchService>();
builder.Services.AddScoped<ShoppingCartService>();
builder.Services.AddScoped<CategoriiPartialService>();
builder.Services.AddScoped<OrdersManagerService>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnSigningIn = async context =>
    {
        var userId = context.Principal?.FindFirst("sub")?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            using var scope = context.HttpContext.RequestServices.CreateScope();
            var pointsService = scope.ServiceProvider.GetRequiredService<PointsService>();
            var cartService = scope.ServiceProvider.GetRequiredService<ShoppingCartService>();
            await pointsService.CacheUserPointsAsync(userId);
            await cartService.CreateCartAsync(userId);
        }
    };
});

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
