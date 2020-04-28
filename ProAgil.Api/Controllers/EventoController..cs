using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using AutoMapper;
using ProAgil.Api.Dtos;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProAgil.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private IProAgilRepository _repo { get; }
        public IMapper _mapper { get; }

        public EventoController(IProAgilRepository repo, IMapper mapper)
        {
            this._repo = repo;
            this._mapper = mapper;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources","img");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if(file.Length > 0)
                {
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, filename.Replace("\""," ").Trim());
                    using(var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return Ok();
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao efetuar upload { ex.Message }");
            }

            return BadRequest("Erro ao efetuar Upload");
            
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await this._repo.GetAllEventoAsync(true);
                var results = this._mapper.Map<IEnumerable<EventoDto>>(eventos);
                return Ok(results);
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao buscar Eventos no Banco de Dados { ex.Message }");
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var evento = await this._repo.GetEventoAsyncById(id, true);
                var results = this._mapper.Map<EventoDto>(evento);
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
                var eventos = await this._repo.GetAllEventoAsyncByTema(tema, true);
                var results = this._mapper.Map<EventoDto[]>(eventos);
                return Ok(results);
            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao buscar Eventos no Banco de Dados");
            }
             
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = this._mapper.Map<Evento>(model);
                this._repo.Add(evento);

                if(await this._repo.SaveChangesAsyns())
                {
                    return Created($"/api/evento/{evento.Id}", model);
                }

            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao buscar gravar no Banco de Dados");
            }
            return BadRequest();            
        }

        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId, EventoDto model)
        {
            try
            {
                var evento = await this._repo.GetEventoAsyncById(EventoId, false);

                if(evento == null)
                {
                    return NotFound();
                }

                var idLotes = new List<int>();
                var idRedesSociais = new List<int>();

                model.Lotes.ForEach(lote => idLotes.Add(lote.Id));
                model.RedesSociais.ForEach(rede => idRedesSociais.Add(rede.Id));

                var lotes = evento.Lotes.Where(
                    lote => !idLotes.Contains(lote.Id)
                ).ToArray();

                var redesSociais = evento.RedesSociais.Where(
                    rede => !idRedesSociais.Contains(rede.Id)
                ).ToArray();

                if(lotes.Length > 0) _repo.DeleteRange(lotes);
                if(redesSociais.Length > 0) _repo.DeleteRange(redesSociais);

                this._mapper.Map(model, evento);

                this._repo.Update(evento);

                if(await this._repo.SaveChangesAsyns())
                {
                    return Created($"/api/evento/{evento.Id}", model);
                }

            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao buscar Eventos no Banco de Dados");
            }
            return BadRequest();            
        }

        [HttpDelete("{EventoId}")]
        public async Task<IActionResult> Delete(int EventoId)
        {
            try
            {
                var evento = await this._repo.GetEventoAsyncById(EventoId, false);

                if(evento == null)
                {
                    return NotFound();
                }

                this._repo.Delete(evento);

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
