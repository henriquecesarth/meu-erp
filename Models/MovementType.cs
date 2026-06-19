using System.ComponentModel.DataAnnotations;

namespace MeuErp.Models
{
    public enum MovementType
    {
        [Display(Name = "Entrada")]
        Entry,
        [Display(Name = "Saída")]
        Exit
    }
}
