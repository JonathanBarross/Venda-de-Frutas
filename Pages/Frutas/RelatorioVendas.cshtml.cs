using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Venda_de_Frutas.Data;
using Venda_de_Frutas.Models;

namespace VendadeFrutas.Pages
{
    [Authorize(Policy = "Vendedor")] // Restringe o acesso apenas a vendedores
    public class RelatorioVendasModel : PageModel
    {
        private readonly DataContext _context;

        public RelatorioVendasModel(DataContext context)
        {
            _context = context;
        }

        public List<Venda> Vendas { get; set; } = new List<Venda>(); // Lista de vendas

        public async Task OnGetAsync()
        {
            // Carrega as vendas ordenadas por data (da mais recente para a mais antiga)
            Vendas = await _context.Vendas
                .OrderByDescending(x => x.DataHora)
                .ToListAsync();
        }
    }
}