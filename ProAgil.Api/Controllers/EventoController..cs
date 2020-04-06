using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private IProAgilRepository _repo { get; }

        public EventoController(IProAgilRepository repo)
        {
            this._repo = repo;
        }

        

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await this._repo.GetAllEventoAsync(true);
                return Ok(results);
            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao buscar Eventos no Banco de Dados");
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var results = await this._repo.GetEventoAsyncById(id, true);
                return Ok(results);
            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao buscar Eventos no Banco de Dados");
            }
             
        }

        [HttpGet("getBytema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                var results = await this._repo.GetAllEventoAsyncByTema(tema, true);
                return Ok(results);
            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao buscar Eventos no Banco de Dados");
            }
             
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
            try
            {
                this._repo.Add(model);

                if(await this._repo.SaveChangesAsyns())
                {
                    return Created($"/api/evento/{model.Id}", model);
                }

            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao buscar gravar no Banco de Dados");
            }
            return BadRequest();            
        }

        [HttpPut]
        public async Task<IActionResult> Put(int EventoId, Evento model)
        {
            try
            {
                var evento = await this._repo.GetEventoAsyncById(EventoId, false);

                if(evento == null)
                {
                    return NotFound();
                }

                this._repo.Update(model);

                if(await this._repo.SaveChangesAsyns())
                {
                    return Created($"/api/evento/{model.Id}", model);
                }

            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao buscar Eventos no Banco de Dados");
            }
            return BadRequest();            
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int EventoId, Evento model)
        {
            try
            {
                var evento = await this._repo.GetEventoAsyncById(EventoId, false);

                if(evento == null)
                {
                    return NotFound();
                }

                this._repo.Delete(model);

                if(await this._repo.SaveChangesAsyns())
                {
                    return Ok();
                }

            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao buscar Eventos no Banco de Dados");
            }
            return BadRequest();            
        }
    }
}
