using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Venda_de_Frutas.Data;
using Venda_de_Frutas.Models;

namespace VendaDeFrutas.Pages_Frutas;

public class IndexModel : PageModel
{
    private readonly DataContext _context;

    public IndexModel(DataContext context)
    {
        _context = context;
    }

    public IList<Fruta> Fruta { get; set; } = default!; // Lista de frutas

    [BindProperty(SupportsGet = true)]
    public string? ProcurarString { get; set; } // Filtro por nome

    [BindProperty(SupportsGet = true)]
    public string? Classificacao { get; set; } // Filtro por classificação

    [BindProperty(SupportsGet = true)]
    public bool? Fresca { get; set; } // Filtro por frescura

    public async Task OnGetAsync()
    {
        var frutas = from f in _context.Frutas select f;

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

        Fruta = await frutas.ToListAsync(); // Carrega a lista de frutas filtrada
    }
}
