using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
        void Add<T>(T entity) where T: class;
        void Update<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;

        Task<bool> SaveChangesAsyns();
         
        Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrante);

        Task<Evento[]> GetAllEventoAsync(bool includePalestrante);

        Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrante);

        Task<Palestrante> GetPalestranteAsyncByName(string nome, bool includeEvento);
        Task<Palestrante> GetPalestranteAsyncById(int PalestranteId, bool includeEvento);
        Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEvento);

    }
}