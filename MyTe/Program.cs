using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyTe.Models.Contexts;
using MyTe.Models.Startup;
using MyTe.Services;
using MyTe.Models.Common;
using MyTe.Controllers;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager config = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<MyTeContext>(options => 
    options.UseSqlServer(config.GetConnectionString("MyTeConnection")));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("IdentityMyTeConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// opcional, mas importante
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Autenticacao/Login";
    options.LogoutPath = "/Autenticacao/Logout";
    options.AccessDeniedPath = "/Autenticacao/AccessDenied";
});

//Habilitando o serviço WBSService para injeção de dependência
builder.Services.AddScoped<WBSsService>();
builder.Services.AddScoped<HorasService>();
builder.Services.AddScoped<FuncionariosService>();
builder.Services.AddScoped<AutenticacaoService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

var scope = app.Services.CreateScope();
var provider = scope.ServiceProvider;
var context = provider.GetRequiredService<MyTeContext>();

//Sincronizando o context com o banco de dados
DbInitializer.Initialize(context);
Utils.CreateRoles(provider).Wait();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Autenticacao}/{action=Login}/{id?}");

app.Run();
