using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Venda_de_Frutas.Data;
using Venda_de_Frutas.Models;

namespace VendadeFrutas.Pages;

[Authorize(Policy = "Vendedor")] 
public class VenderFrutaModel : PageModel
{
    private readonly DataContext _context;

    public VenderFrutaModel(DataContext context)
    {
        _context = context;
    }

    public List<Fruta> FrutasDisponiveis { get; set; } = new List<Fruta>(); 

    [BindProperty]
    public List<int> FrutasSelecionadas { get; set; } = new List<int>(); 

    [BindProperty]
    public int Desconto { get; set; } = 0; // Desconto em porcentagem

    [BindProperty(SupportsGet = true)]
    public string? ProcurarString { get; set; } // Filtro por nome

    [BindProperty(SupportsGet = true)]
    public string? Classificacao { get; set; } // Filtro por classificação

    [BindProperty(SupportsGet = true)]
    public bool? Fresca { get; set; } // Filtro por frescura

    public async Task OnGetAsync()
    {
        var frutas = from f in _context.Frutas
            where f.Quantidade > 0 // Apenas frutas com quantidade disponível
            select f;

        // Aplica os filtros
        if (!string.IsNullOrEmpty(ProcurarString))
        {
            frutas = frutas.Where(f => f.Nome.Contains(ProcurarString));
        }

        if (!string.IsNullOrEmpty(Classificacao))
        {
            frutas = frutas.Where(f => f.Classificacao == Classificacao);
        }

        if (Fresca.HasValue)
        {
            frutas = frutas.Where(f => f.Fresca == Fresca.Value);
        }

        FrutasDisponiveis = await frutas.ToListAsync(); // Carrega a lista de frutas filtrada
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var frutasVendidas = await _context.Frutas
            .Where(x => FrutasSelecionadas.Contains(x.Id)) // Seleciona as frutas vendidas
            .ToListAsync();

        if (!frutasVendidas.Any())
        {
            ModelState.AddModelError(string.Empty, "Nenhuma fruta selecionada.");
            return Page();
        }

        // Calcula o valor total com desconto
        decimal valorTotal = frutasVendidas.Sum(x => x.Valor);
        valorTotal -= valorTotal * (Desconto / 100m);

        // Cria a venda
        var venda = new Venda
        {
            DataHora = DateTime.Now,
            ValorTotal = valorTotal,
            ItensVendidos = string.Join(", ", frutasVendidas.Select(x => x.Nome)),
        };

        // Atualiza a quantidade disponível das frutas
        foreach (var fruta in frutasVendidas)
        {
            fruta.Quantidade--;
        }

        _context.Vendas.Add(venda);
        await _context.SaveChangesAsync();

        return RedirectToPage("RelatorioVendas"); // Redireciona para o relatório de vendas
    }
}
