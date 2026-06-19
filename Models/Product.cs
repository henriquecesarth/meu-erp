using System.ComponentModel.DataAnnotations;

namespace MeuErp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [Display(Name = "Nome")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Descrição")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Preço")]
        public decimal Price { get; set; }

        [Display(Name = "Quantidade Atual")]
        public int Quantity { get; set; } // Managed automatically by movements

        [Required(ErrorMessage = "A quantidade mínima é obrigatória.")]
        [Display(Name = "Qtd. Mínima")]
        public int MinQuantity { get; set; }

        [Required(ErrorMessage = "A quantidade máxima é obrigatória.")]
        [Display(Name = "Qtd. Máxima")]
        public int MaxQuantity { get; set; }
    }
}
