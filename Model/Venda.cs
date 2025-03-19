using System;
using System.ComponentModel.DataAnnotations;
namespace Venda_de_Frutas.Models;

public class Venda
{
    public int Id { get; set; }

    [Required]
    public DateTime DataHora { get; set; } = DateTime.Now; // Horário da venda

    [Required]
    public decimal ValorTotal { get; set; } // Valor total da venda

    [Required]
    public string ItensVendidos { get; set; } // Lista de itens vendidos (pode ser JSON ou string formatada)

}