using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(maximumLength:120, ErrorMessage = "El Campo {0} no debe tener mas de {1} carácter")]
        public string Nombre { get; set; }
        public List<Libro> Libros{ get; set; }
    }
}
