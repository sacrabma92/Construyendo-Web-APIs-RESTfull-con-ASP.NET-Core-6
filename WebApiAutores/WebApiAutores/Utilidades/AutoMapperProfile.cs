using AutoMapper;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Utilidades
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(AutorDTO => AutorDTO.Libros, opciones => opciones.MapFrom(MapAutorDTOLibros));

            CreateMap<LibroCreacionDto, Libro>()
                .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));
            CreateMap<Libro, LibroDTO>();
            CreateMap<Libro, LibroDTOConAutores>()
                .ForMember(LibroDTO => LibroDTO.Autores, opciones =>  opciones.MapFrom(MapLibroDTOAutores));
            CreateMap<LibroPatchDTO, Libro>().ReverseMap();

            CreateMap<ComentarioCreacionDTO,  Comentario>();
            CreateMap<Comentario, ComentarioDTO>();
        }

        private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDTO)
        {
            var resultado = new List<LibroDTO>();

            if(autor.AutoresLibros == null) { return resultado; }

            foreach (var autorLibro in autor.AutoresLibros)
            {
                resultado.Add(new LibroDTO()
                {
                    Id = autorLibro.LibroId,
                    Titulo = autorLibro.Libro.Titulo
                });
            }

            return resultado;
        }

        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTO libroDTO)
        {
            var resultado = new List<AutorDTO>();

            if(libro.AutoresLibros == null) { return resultado; }

            foreach (var autorlibro in libro.AutoresLibros)
            {
                resultado.Add(new AutorDTO
                {
                    Id = autorlibro.AutorId,
                    Nombre = autorlibro.Autor.Nombre
                });
            }        

            return resultado;
        }

        private List<AutorLibro> MapAutoresLibros(LibroCreacionDto libroCreacionDto, Libro libro)
        {
            var resultado = new List<AutorLibro>();

            if(libroCreacionDto.AutoresIds == null) { return resultado; }

            foreach(var autorId in libroCreacionDto.AutoresIds)
            {
                resultado.Add(new AutorLibro() { AutorId = autorId });
            }

            return resultado;
        }
    }
}
