using System;
using System.ComponentModel.DataAnnotations;

namespace MeuErp.Models
{
    public class StockMovement
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O produto é obrigatório.")]
        [Display(Name = "Produto")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required(ErrorMessage = "O tipo de movimentação é obrigatório.")]
        [Display(Name = "Tipo")]
        public MovementType Type { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        [Display(Name = "Quantidade")]
        public int Quantity { get; set; }

        [Display(Name = "Data e Hora")]
        public DateTime MovementDate { get; set; } = DateTime.Now;

        [Display(Name = "Observações")]
        public string? Description { get; set; }
    }
}
