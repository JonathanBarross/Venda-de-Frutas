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
public class DetailsModel : PageModel
{
    private readonly DataContext _context;

    public DetailsModel(DataContext context)
    {
        _context = context;
    }

    public Fruta Fruta { get; set; } = default!; // Propriedade para armazenar os dados da fruta

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound(); // Se o ID for nulo, retorna um erro 404
        }

        var fruta = await _context.Frutas.FirstOrDefaultAsync(m => m.Id == id);

        if (fruta == null)
        {
            return NotFound(); // Se a fruta não for encontrada, retorna um erro 404
        }

        Fruta = fruta;
        return Page(); // Exibe a página de detalhes
    }
}
