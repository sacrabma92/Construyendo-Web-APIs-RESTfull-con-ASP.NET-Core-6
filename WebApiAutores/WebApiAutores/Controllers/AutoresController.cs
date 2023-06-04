using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;
using WebApiAutores.Filtros;
using WebApiAutores.Servicios;

namespace WebApiAutores.Controllers
{
    [Route("api/autores")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicio servicio;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScope;
        private readonly ServicioSingleton servicioSingleton;
        private readonly ILogger<AutoresController> logger;

        public AutoresController(ApplicationDbContext context, 
            IServicio servicio,
            ServicioTransient servicioTransient,
            ServicioScoped servicioScope,
            ServicioSingleton servicioSingleton,
            ILogger<AutoresController> logger
            )
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.context = context;
            this.servicio = servicio;
            this.servicioTransient = servicioTransient;
            this.servicioScope = servicioScope;
            this.servicioSingleton = servicioSingleton;
            this.logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(MiFiltroDeAccion))]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            logger.LogInformation("Estamos listando todos los autores");
            return await context.Autores.Include(x => x.Libros).ToListAsync();
        }

        [HttpGet("GUID")]
        //[ResponseCache(Duration = 10)]
        public ActionResult obtenerGuids()
        {
            return Ok(new
            {
                AutoresController_Transient = servicioTransient.Guid,
                ServicioA_Transient = servicio.ObtenerTransient(),
                AutoresController_Scope = servicioScope.Guid,
                ServicioA_Scoped = servicio.ObtenerScoped(),
                AutoresController_Singleton = servicioSingleton.Guid,
                ServicioA_Singleton = servicio.ObtenerSingleton()
            });
        }

        // Primer registro en la tabla autores
        [HttpGet("primerAutor")]
        public async Task<ActionResult<Autor>> PrimerAutor()
        {
            return await context.Autores.FirstOrDefaultAsync();
        }

        //Buscar por id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> Get(int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if(autor == null)
            {
                return NotFound();
            }

            return autor;
        }

        //Buscar por nombre
        [HttpGet("nombre")]
        public async Task<ActionResult<Autor>> Get(string nombre)
        {
            var autor= await context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));

            if (autor == null) return NotFound();

            return autor;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            var existeAutorConElMismoNombre = await context.Autores.AnyAsync(x => x.Nombre == autor.Nombre);

            if(existeAutorConElMismoNombre)
            {
                return BadRequest($"Ya existe un autor con el nombre {autor.Nombre}");
            }

            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if(autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id de la URL");
            }

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);
            
            if(!existe) 
            { 
                return NotFound();
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
