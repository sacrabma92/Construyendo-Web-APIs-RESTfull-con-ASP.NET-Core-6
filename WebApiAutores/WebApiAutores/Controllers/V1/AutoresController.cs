using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;
using WebApiAutores.Utilidades;

namespace WebApiAutores.Controllers.V1
{
    [ApiController]
    //[Route("api/v1/autores")]
    [Route("api/v1/autores")]
    //[CabecearaEstaPresenteAtribute("x-version", "1")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public IMapper mapper { get; }

        public AutoresController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {

            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        // Obtener todos los autores
        [HttpGet(Name = "ObtenerAutores")]
        [AllowAnonymous]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Autores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var autores = await queryable.OrderBy(autor => autor.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }

        //Buscar por id
        [HttpGet("{id:int}", Name = "ObtenerAutorPorID")]
        public async Task<ActionResult<AutorDTOConLibros>> Get(int id)
        {
            var autor = await context.Autores
                .Include(autorDB => autorDB.AutoresLibros)
                .ThenInclude(autorDB => autorDB.Libro)
                .FirstOrDefaultAsync(autorBD => autorBD.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<AutorDTOConLibros>(autor);
            GenerarEnlaces(dto);
            return dto;
        }

        private void GenerarEnlaces(AutorDTO autorDTO)
        {
            autorDTO.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerAutores", new { id = autorDTO.Id }),
                descripcion: "self", metodo: "GET"));
            autorDTO.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("ActualizarAutor", new { id = autorDTO.Id }),
                descripcion: "autor-actualizar", metodo: "PUT"));
            autorDTO.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("BorrarAutor", new { id = autorDTO.Id }),
                descripcion: "borrar-autor", metodo: "DELETE"));
        }

        //Buscar por nombre
        [HttpGet("nombre", Name = "obtenerAutorPorNombre")]
        public async Task<ActionResult<List<AutorDTO>>> Get(string nombre)
        {
            var autores = await context.Autores.Where(x => x.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpPost(Name = "CrearAutor")]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {
            var existeAutorConElMismoNombre = await context.Autores.AnyAsync(x => x.Nombre == autorCreacionDTO.Nombre);

            if (existeAutorConElMismoNombre)
            {
                return BadRequest($"Ya existe un autor con el nombre {autorCreacionDTO.Nombre}");
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);

            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}", Name = "ActualizarAutor")]
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Borra un autor
        /// </summary>
        /// <param name="id">Id del autor a borrar</param>
        /// <returns></returns>

        [HttpDelete("{id:int}", Name = "BorrarAutor")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
