using System.ComponentModel.DataAnnotations;

namespace Venda_de_Frutas.Models;
public class Fruta
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome da fruta é obrigatório.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "A classificação é obrigatória.")]
    [RegularExpression(@"^(Extra|de primeira|de segunda|de terceira)$"
    , ErrorMessage = "A classificação deve ser 'Extra', 'de primeira', 'de segunda' ou 'de terceira'.")]
    public string Classificacao { get; set; }

    public bool Fresca { get; set; }

    [Range(1, 1000, ErrorMessage = "A quantidade deve estar entre 1 e 1000.")]
    public int Quantidade { get; set; }

    [Range(0.01, 1000.00, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Valor { get; set; }
}
