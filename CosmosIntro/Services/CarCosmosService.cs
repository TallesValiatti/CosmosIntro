using CosmosIntro.Interfaces;
using CosmosIntro.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosIntro.Services
{
    public class CarCosmosService : ICarCosmosService
    {   
        private const string _connectionString = "<AccountEndpoint>";
        private const string _dataBaseId = "cosmos-intro-db";
        private const string _containerId = "cars";
        private readonly Container _container;

        public CarCosmosService()
        {
            var client = new CosmosClientBuilder(_connectionString).Build();
            this._container = client.GetContainer(_dataBaseId, _containerId);
        }

        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            var querDefinition = "SELECT * FROM c";
           
            List<Car> results = new List<Car>();
           
            var query = this._container.GetItemQueryIterator<Car>(new QueryDefinition(querDefinition));
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task SaveAsync(Car item)
        {
            await this._container.CreateItemAsync<Car>(item, new PartitionKey(item.Color));
        }
    }
}
