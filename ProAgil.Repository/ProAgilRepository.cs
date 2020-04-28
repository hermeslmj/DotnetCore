using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        public ProAgilContext _context { get; }

        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public void Add<T>(T entity) where T : class
        {
            this._context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            this._context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            this._context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entityArray) where T: class {
            this._context.RemoveRange(entityArray);
        }
        public async Task<bool> SaveChangesAsyns()
        {
            return (await this._context.SaveChangesAsync()) > 0;
        }

        //EVENTO
        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrante = false)
        {
            IQueryable<Evento> query = this._context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedesSociais);
            if(includePalestrante)
            {
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Palestrante);
            }

            query = query.OrderByDescending(c => c.DataEvento);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrante = false)
        {
            IQueryable<Evento> query = this._context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedesSociais);
            if(includePalestrante)
            {
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Palestrante);
            }

            query = query.OrderByDescending(c => c.DataEvento)
            .Where(c => c.Tema.ToLower() == tema.ToLower());

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrante = false)
        {
            IQueryable<Evento> query = this._context.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedesSociais);
            if(includePalestrante)
            {
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Palestrante);
            }

            query = query.OrderByDescending(c => c.DataEvento)
            .Where(c => c.Id == EventoId);

            return await query.FirstOrDefaultAsync();
        }

        //PALESTRANTE
        public async Task<Palestrante> GetPalestranteAsyncById(int PalestranteId, bool includeEvento = false)
        {
            IQueryable<Palestrante> query = this._context.Palestrantes
                .Include(c => c.RedesSociais);

            if(includeEvento)
            {
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(e => e.Evento);
            }

            query = query.OrderBy(p => p.Nome).Where(p => p.Id == PalestranteId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Palestrante> GetPalestranteAsyncByName(string nome, bool includeEvento = false)
        {
                 IQueryable<Palestrante> query = this._context.Palestrantes
                .Include(c => c.RedesSociais);

            if(includeEvento)
            {
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(e => e.Evento);
            }

            query = query.OrderBy(p => p.Nome).Where(p => p.Nome.ToLower() == nome.ToLower());

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEvento = false)
        {
            IQueryable<Palestrante> query = this._context.Palestrantes
                .Include(p => p.RedesSociais);

            if(includeEvento)
            {
                query = query.Include(pe => pe.PalestrantesEventos)
                .ThenInclude(e => e.Evento);
            }

            return await query.ToArrayAsync();
        }


    }
}