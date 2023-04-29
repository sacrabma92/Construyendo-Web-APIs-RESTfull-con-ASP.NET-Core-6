using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [Route("api/autores")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Autor>> Get()
        {
            return new List<Autor>()
            { 
                new Autor() { Id = 1 , Nombre = "Carlos"},
                new Autor() { Id = 2 , Nombre = "Angelica"},
            };
        }
    }
}
