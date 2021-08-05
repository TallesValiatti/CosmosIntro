using CosmosIntro.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosIntro.Interfaces
{
    public interface ICarCosmosService
    {
        Task<IEnumerable<Car>> GetAllAsync();
        
        Task SaveAsync(Car item);
    }
}
