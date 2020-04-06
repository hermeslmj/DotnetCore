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

    public class PalestranteController: ControllerBase
    {
        private IProAgilRepository _repo { get; }

        public PalestranteController(IProAgilRepository repo)
        {
            this._repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await this._repo.GetAllPalestrantesAsync(true);
                return Ok(results);
            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao buscar Palestrante no Banco de Dados");
            }
            
        }    
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var results = await this._repo.GetPalestranteAsyncById(id, true);
                return Ok(results);
            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao buscar Palestrante no Banco de Dados");
            }
             
        }

        [HttpGet("getByNome/{nome}")]
        public async Task<IActionResult> Get(string nome)
        {
            try
            {
                var results = await this._repo.GetPalestranteAsyncByName(nome, true);
                return Ok(results);
            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao buscar Palestrante no Banco de Dados");
            }
             
        }

        [HttpPost]
        public async Task<IActionResult> Post(Palestrante model)
        {
            try
            {
                this._repo.Add(model);

                if(await this._repo.SaveChangesAsyns())
                {
                    return Created($"/api/palestrante/{model.Id}", model);
                }

            }
            catch(System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao gravar Palestrante no Banco de Dados" + e.Message);
            }
            return BadRequest();            
        }

        [HttpPut]
        public async Task<IActionResult> Put(int PalestranteId, Palestrante model)
        {
            try
            {
                var palestrante = await this._repo.GetPalestranteAsyncById(PalestranteId, false);

                if(palestrante == null)
                {
                    return NotFound();
                }

                this._repo.Update(model);

                if(await this._repo.SaveChangesAsyns())
                {
                    return Created($"/api/palestrante/{model.Id}", model);
                }

            }
            catch(System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao editar palestrante no Banco de Dados" +  e.Message);
            }
            return BadRequest();            
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int PalestranteId, Palestrante model)
        {
            try
            {
                var palestrante = await this._repo.GetEventoAsyncById(PalestranteId, false);

                if(palestrante == null)
                {
                    return NotFound();
                }

                this._repo.Delete(model);

                if(await this._repo.SaveChangesAsyns())
                {
                    return Ok();
                }

            }
            catch(System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao apagar palestrante no Banco de Dados" + e.Message);
            }
            return BadRequest();            
        }





        
    }
}