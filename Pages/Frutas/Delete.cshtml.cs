using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Venda_de_Frutas.Data;
using Venda_de_Frutas.Models;

namespace VendaDeFrutas.Pages_Frutas;

[Authorize(Policy = "Administrador")] // Restringe o acesso apenas a administradores
public class DeleteModel : PageModel
{
    private readonly DataContext _context;

    public DeleteModel(DataContext context)
    {
        _context = context;
    }

    [BindProperty]
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
        return Page(); // Exibe a página de confirmação de exclusão
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound(); // Se o ID for nulo, retorna um erro 404
        }

        var fruta = await _context.Frutas.FindAsync(id);

        if (fruta != null)
        {
            Fruta = fruta;
            _context.Frutas.Remove(Fruta); // Remove a fruta do banco de dados
            await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
        }

        return RedirectToPage("./Index"); // Redireciona para a lista de frutas após a exclusão
    }
}
