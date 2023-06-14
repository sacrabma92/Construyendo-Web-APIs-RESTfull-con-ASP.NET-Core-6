namespace PeliculasAPI.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int cantidadRegistroPorPagina = 10;
        private readonly int cantidadMaximaRegistroPorPagina = 50;

        public int CantidadRegistrosPorPagina
        {
            get => cantidadRegistroPorPagina;
            set
            {
                cantidadRegistroPorPagina = (value > cantidadMaximaRegistroPorPagina) ? cantidadMaximaRegistroPorPagina : value;
            }
        }
    }
}
