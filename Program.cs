using Venda_de_Frutas.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Configurações
ConfigurarConnection(builder); 
ConfigurarAutenticacao(builder);
ConfigurarAutorizacao(builder); 
ConfigurarCultura(); 

builder.Services.AddRazorPages(); 
var app = builder.Build();

// Middleware
app.UseHttpsRedirection(); 
app.UseStaticFiles(); 
app.UseRouting(); 
app.UseAuthentication(); 
app.UseAuthorization(); 
app.MapRazorPages(); 

app.Run(); // Inicia a aplicação

// Método para configurar a conexão com o banco de dados
void ConfigurarConnection(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(connectionString));
}

// Método para configurar a autenticação baseada em cookies
void ConfigurarAutenticacao(WebApplicationBuilder builder)
{
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Login"; // Página de login
            options.AccessDeniedPath = "/AcessoNegado"; // Página de acesso negado
        });
}

// Método para configurar as políticas de autorização
void ConfigurarAutorizacao(WebApplicationBuilder builder)
{
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Administrador", policy => policy.RequireClaim("Perfil", "Administrador")); // Política para administradores
        options.AddPolicy("Vendedor", policy => policy.RequireClaim("Perfil", "Vendedor")); // Política para vendedores
    });
}

// Método para configurar a cultura (formatação de números)
void ConfigurarCultura()
{
    var cultureInfo = new CultureInfo("en-US"); 
    cultureInfo.NumberFormat.NumberDecimalSeparator = "."; 

    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
}