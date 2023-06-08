using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ComentariosController(ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> GetComentario(int libroId)
        {
            var existeLibro = await context.Libros.AnyAsync(libroBD => libroBD.Id == libroId);

            if (!existeLibro)
            {
                return NotFound();
            }

            var comentarios = await context.Comnetarios.Where(comentarioDB => comentarioDB.Id == libroId).ToListAsync();

            return mapper.Map<List<ComentarioDTO>>(comentarios);
        }

        // Guardar los comentario a un libro
        [HttpPost]
        public async Task<ActionResult> PostComentario(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var existeLibro = await context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);

            if(!existeLibro)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            context.Add(comentario);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int libroId, int id, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var existelibro = await context.Libros.AnyAsync(libroBD => libroBD.Id == libroId);

            if(!existelibro)
            {
                return NotFound();
            }

            var exitesComentario = await context.Comnetarios.AnyAsync(comentarioDB => comentarioDB.Id == id);

            if(!exitesComentario)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.Id = id;
            comentario.LibroId = libroId;

            context.Update(comentario);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
