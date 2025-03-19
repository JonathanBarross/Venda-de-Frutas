using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Venda_de_Frutas.Data;
using Venda_de_Frutas.Models;

namespace VendaDeFrutas.Pages_Frutas;

[Authorize(Policy = "Administrador")] // Restringe o acesso apenas a administradores
public class EditModel : PageModel
{
    private readonly DataContext _context;

    public EditModel(DataContext context)
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
        return Page(); // Exibe a página de edição
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page(); // Se houver erros de validação, retorna a página com as mensagens de erro
        }

        _context.Attach(Fruta).State = EntityState.Modified; // Marca a fruta como modificada

        try
        {
            await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FrutaExists(Fruta.Id))
            {
                return NotFound(); // Se a fruta não existir, retorna um erro 404
            }
            else
            {
                throw; // Lança a exceção para ser tratada em outro lugar
            }
        }

        return RedirectToPage("./Index"); // Redireciona para a lista de frutas após a edição
    }

    private bool FrutaExists(int id)
    {
        return _context.Frutas.Any(e => e.Id == id); // Verifica se a fruta existe no banco de dados
    }
}
