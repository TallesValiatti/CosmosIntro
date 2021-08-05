using CosmosIntro.Interfaces;
using CosmosIntro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosIntro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarCosmosService _carCosmosService;
        public CarController(ICarCosmosService carCosmosService)
        {
            _carCosmosService = carCosmosService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync() 
        {
            return Ok(await _carCosmosService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsync([FromBody] Car item)
        {
            // Verify whether the item is not null
            if (item == null)
                return BadRequest(new { message = "Item may not be null" });

            // Verify whether the item has the properties: Id and Color
            if (string.IsNullOrWhiteSpace(item.Id))
                return BadRequest(new { message = "Id may not be empty or null" });

            if (string.IsNullOrWhiteSpace(item.Color))
                return BadRequest(new { message = "Color may not be empty or null" });  

            // Verify whether Cosmos container already has the item received
            var cars = await _carCosmosService.GetAllAsync();
            if (cars.Any(x => x.Equals(item)))
                return BadRequest(new { message = "Item already saved on Cosmos container" });

            await _carCosmosService.SaveAsync(item);
            return Ok();
        }
    }
}
