using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using Venda_de_Frutas.Data;
using Venda_de_Frutas.Models;

namespace VendaDeFrutas.Pages_Frutas
{
    [Authorize(Policy = "Administrador")] // Restringe o acesso apenas a administradores
    public class CreateModel : PageModel
    {
        private readonly DataContext _context;

        public CreateModel(DataContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page(); // Exibe a página de cadastro
        }

        [BindProperty]
        public Fruta Fruta { get; set; } = default!; // Propriedade para armazenar os dados da fruta

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Se houver erros de validação, retorna a página com as mensagens de erro
            }

            // Converte o valor do campo "Valor" para decimal
            string valorTexto = Request.Form["Fruta.Valor"];
            if (decimal.TryParse(valorTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal valorConvertido))
            {
                Fruta.Valor = valorConvertido;
            }
            else
            {
                ModelState.AddModelError("Fruta.Valor", "Formato inválido. Use ponto (.) como separador decimal.");
                return Page();
            }

            // Adiciona a fruta ao banco de dados
            _context.Frutas.Add(Fruta);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index"); // Redireciona para a lista de frutas após o cadastro
        }
    }
}