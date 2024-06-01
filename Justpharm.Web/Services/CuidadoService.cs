using Justpharm.Web.Models;
using Justpharm.Web.Repository;

namespace Justpharm.Web.Services
{
    public interface ICuidadoService
    {
        Task<List<Cuidado>> GetCuidadosNoLeidosAsync();
        Task MarcarComoLeidoAsync(Cuidado cuidado);
    }

    public class CuidadoService : ICuidadoService
    {
        private readonly DbQry _dbQry;

        public CuidadoService(DbQry dbQry)
        {
            _dbQry = dbQry;
        }
        // Implementa los métodos para obtener y actualizar los cuidados desde la base de datos o API.
        public async Task<List<Cuidado>> GetCuidadosNoLeidosAsync()
        {
            while (true)
            {
                // Usa el método All de DbQry para obtener los cuidados no leídos
                List<Cuidado>? cuidadosNoLeidos = _dbQry.All<Cuidado>(c => !c.Leido);
                return await Task.FromResult(cuidadosNoLeidos);
            }
        }

        public Task MarcarComoLeidoAsync(Cuidado cuidado)
        {
            // Simulación de marcar como leído en la base de datos

            cuidado.Leido = true;
            _dbQry.Update(cuidado);

            return Task.CompletedTask;
        }
    }

}
