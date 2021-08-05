using CosmosIntro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosIntro.Interfaces
{
    public interface ICarCosmosService
    {
        Task<IEnumerable<Car>> GetAllAsync();
        Task SaveAsync(Car item);
    }
}
