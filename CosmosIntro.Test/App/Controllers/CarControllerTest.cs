using CosmosIntro.Controllers;
using CosmosIntro.Interfaces;
using CosmosIntro.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CosmosIntro.Test.App.Controllers
{
    #region Data

    public class CarControllerTestFailData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] 
            { 
                new Car { Id = null, Color = "Some color" }, 
                "Id may not be empty or null"
            },
            new object[] 
            {
                new Car { Id = string.Empty, Color = "Some color" },
                "Id may not be empty or null"
            },
            new object[] 
            {
                new Car { Id = "Some Id", Color = null },
                "Color may not be empty or null"
            }, 
            new object[] 
            {
                new Car { Id = "Some Id", Color = string.Empty },
                "Color may not be empty or null"
            }, 
            new object[] 
            {
                new Car { Id = "Car1", Color = "Color1" },
                "Item already saved on Cosmos container"
            },
        };

        public IEnumerator<object[]> GetEnumerator()
        { return _data.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }

    public class CarControllerTestOkData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {   
            new object[]
            {
                new Car { Id = "Car2", Color = "Color2" }
            }
        };

        public IEnumerator<object[]> GetEnumerator()
        { return _data.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }

    #endregion

    #region Tests
    public class CarControllerTest
    {
        #region Fail

        [Theory]
        [ClassData(typeof(CarControllerTestFailData))]
        public async Task Execute_Validation_Fail(Car item, string message)
        {
            // Prepare
            Moq.Mock<ICarCosmosService> mockCarCosmosService = new Moq.Mock<ICarCosmosService>();
            mockCarCosmosService.Setup(x => x.SaveAsync(It.IsAny<Car>()))
                                .Returns(Task.CompletedTask);

            IEnumerable<Car> listCars = new List<Car>()
            {
                new Car { Id = "Car1", Color = "Color1" }
            };

            mockCarCosmosService.Setup(x => x.GetAllAsync())
                                .Returns(Task.FromResult(listCars));
            // Act
            var controller = new CarController(mockCarCosmosService.Object);
            var result = await controller.SaveAsync(item);

            // Assert
            Assert.True(result is BadRequestObjectResult);

            var messageSerialized = JsonConvert.SerializeObject(((BadRequestObjectResult)result).Value);
            var parameterMessageSerialized = JsonConvert.SerializeObject(new { message = message });

            Assert.True(messageSerialized.Equals(parameterMessageSerialized));
        }

        #endregion

        #region Ok

        [Theory]
        [ClassData(typeof(CarControllerTestOkData))]
        public async Task Execute_Validation_OK(Car item)
        {
            // Prepare
            Moq.Mock<ICarCosmosService> mockCarCosmosService = new Moq.Mock<ICarCosmosService>();
            mockCarCosmosService.Setup(x => x.SaveAsync(It.IsAny<Car>()))
                                .Returns(Task.CompletedTask);

            IEnumerable<Car> listCars = new List<Car>()
            {
                new Car { Id = "Car1", Color = "Color1" }
            };

            mockCarCosmosService.Setup(x => x.GetAllAsync())
                                .Returns(Task.FromResult(listCars));
            // Act
            var controller = new CarController(mockCarCosmosService.Object);
            var result = await controller.SaveAsync(item);

            // Assert
            Assert.True(result is OkResult);
        }

        #endregion
    }
    #endregion
}
