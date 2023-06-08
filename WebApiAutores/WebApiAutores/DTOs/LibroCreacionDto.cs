using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.DTOs
{
    public class LibroCreacionDto
    {
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public List<int> AutoresIds { get; set; }
    }
}
