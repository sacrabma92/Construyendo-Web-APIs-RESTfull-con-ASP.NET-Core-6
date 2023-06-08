using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.DTOs
{
    public class AutorCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "El Campo {0} no debe tener mas de {1} carácter")]
        public string Nombre { get; set; }
    }
}
