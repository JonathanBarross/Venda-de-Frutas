using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Venda_de_Frutas.Data;

namespace VendadeFrutas.Pages.Login;

public class LoginModel : PageModel
{
    private readonly DataContext _context;

    public LoginModel(DataContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string Nome { get; set; }

    [BindProperty]
    public string Senha { get; set; }

    public void OnGet()
    {
        // Método chamado quando a página é carregada
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Verifica se o usuário existe no banco de dados
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Nome == Nome && u.Senha == Senha);

        if (usuario == null)
        {
            ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
            return Page();
        }

        // Cria a identidade do usuário com base no perfil
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim("Perfil", usuario.Perfil)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // Cria o cookie de autenticação
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

        // Redireciona com base no perfil do usuário
        if (usuario.Perfil == "Administrador")
        {
            return RedirectToPage("/Frutas/Create");
        }
        else if (usuario.Perfil == "Vendedor")
        {
            return RedirectToPage("/Frutas/VenderFrutas");
        }

        return RedirectToPage("/Index");
    }
}
