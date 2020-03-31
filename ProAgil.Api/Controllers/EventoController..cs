using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProAgil.Api.Data;
using ProAgil.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ProAgil.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventoController : ControllerBase
    {

        public DataContext _context { get; }

        public EventoController(DataContext context)
        {
            this._context = context;

        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _context.Eventos.ToListAsync();
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
                var results = await this._context.Eventos.FirstOrDefaultAsync((x => x.EventoId == id));

                return Ok(results);
            }
            catch(System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao buscar Eventos no Banco de Dados");
            }
             
        }
    }
}
