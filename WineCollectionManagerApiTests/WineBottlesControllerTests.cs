using Microsoft.AspNetCore.Mvc;
using Moq;
using WineCollectionManagerApi.Controllers;
using WineCollectionManagerApi.Models;
using WineCollectionManagerApi.Services;
using AutoMapper;

namespace WineCollectionManagerApiTests
{
    public class WineBottlesControllerTests
    {
        private readonly WineBottlesController _controller;
        private readonly Mock<IWineBottleService> _mockWineBottleService;
        private readonly Mock<IMapper> _mockMapper;

        public WineBottlesControllerTests()
        {
            _mockWineBottleService = new Mock<IWineBottleService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new WineBottlesController(_mockWineBottleService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllWineBottles_ReturnsOkResult_WithListOfWineBottles()
        {
            // Given
            var wineBottles = new List<WineBottleModel>
            {
                new() { Id = 1, Name = "Wine A" },
                new() { Id = 2, Name = "Wine B" }
            };

            _mockWineBottleService.Setup(service => service.GetAll()).ReturnsAsync(wineBottles);

            // When
            var result = await _controller.GetAllWineBottles();

            // Then
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.IsType<OkObjectResult>(okResult);

            var returnedWineBottles = okResult.Value as IEnumerable<WineBottleModel>;
            Assert.NotNull(returnedWineBottles);
            Assert.Equal(returnedWineBottles, wineBottles);
        }

        [Fact]
        public async Task GetWineBottlesByWinemakerId_ValidId_ReturnsOkResult_WithListOfWineBottles()
        {
            // Given
            var wineBottles = new List<WineBottleModel>
            {
                new() { Id = 1, Name = "Wine A", WinemakerId = 1},
                new() { Id = 2, Name = "Wine B", WinemakerId = 2},
                new() { Id = 3, Name = "Wine C", WinemakerId = 1}
            };

            _mockWineBottleService.Setup(service => service.GetByWinemakerId(1))
                .ReturnsAsync(wineBottles.Where(w => w.WinemakerId == 1));

            // When
            var result = await _controller.GetWineBottlesByWinemakerId(1);

            // Then
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.IsType<OkObjectResult>(okResult);

            var model = okResult.Value as IEnumerable<WineBottleModel>;
            Assert.NotNull(model);
            Assert.Equal(2, model.Count());

            Assert.Contains(model, b => b.Name == "Wine A" && b.Id == 1);
            Assert.Contains(model, b => b.Name == "Wine C" && b.Id == 3);
        }


        [Fact]
        public async Task GetWineBottlesByWinemakerId_InvalidId_ReturnsBadRequest()
        {
            // When
            var result = await _controller.GetWineBottlesByWinemakerId(-1);

            // Then
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.IsType<BadRequestObjectResult>(badRequestResult);

            var expectedErrorMessage = "Winemaker ID must be a non-negative integer.";
            Assert.Equal(expectedErrorMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task GetWineBottleById_ValidId_ReturnsOkResult_WithWineBottle()
        {
            // Given
            var wineBottle = new WineBottleModel { Id = 1, Name = "Wine A" };
            var wineBottle2 = new WineBottleModel { Id = 2, Name = "Wine B" };

            _mockWineBottleService.Setup(service => service.GetById(1)).ReturnsAsync(wineBottle);

            // When
            var result = await _controller.GetWineBottleById(1);

            // Then
            var okResult = result.Result as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(wineBottle, okResult.Value);
        }

        [Fact]
        public async Task GetWineBottleById_InvalidId_ReturnsBadRequest()
        {
            // When
            var result = await _controller.GetWineBottleById(-1);

            // Then
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.IsType<BadRequestObjectResult>(badRequestResult);

            var expectedErrorMessage = "Wine bottle ID must be a non-negative integer.";
            Assert.Equal(expectedErrorMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task AddWineBottle_ValidData_ReturnsCreatedAtAction_And_IncreasesItemCount()
        {
            // Given
            var initialWineBottles = new List<WineBottleModel>
            {
                new() { Id = 2, Name = "Wine B" }
            };

            var wineBottleCreate = new WineBottleCreateModel { Name = "Wine A" };
            var newWineBottle = new WineBottleModel { Id = 1, Name = "Wine A" };

            _mockMapper.Setup(m => m.Map<WineBottleModel>(wineBottleCreate)).Returns(newWineBottle);
            _mockWineBottleService.Setup(service => service.Add(newWineBottle)).Returns(Task.CompletedTask);

            _mockWineBottleService.Setup(service => service.GetAll()).ReturnsAsync(initialWineBottles);

            // When
            var addResult = await _controller.AddWineBottle(wineBottleCreate);

            // Then
            var createdAtActionResult = addResult as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            Assert.IsType<CreatedAtActionResult>(createdAtActionResult);
            Assert.Equal(newWineBottle, createdAtActionResult.Value);

            // Setup
            _mockWineBottleService.Setup(service => service.GetAll())
                .ReturnsAsync(initialWineBottles.Concat(new[] { newWineBottle }));

            var getAllResult = await _controller.GetAllWineBottles();

            // Then
            var okResult = getAllResult.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.IsType<OkObjectResult>(okResult);

            var model = okResult.Value as IEnumerable<WineBottleModel>;
            Assert.NotNull(model);
            Assert.Equal(2, model.Count());
            Assert.Contains(model, b => b.Id == 1 && b.Name == "Wine A");
        }

        [Fact]
        public async Task AddWineBottle_InvalidData_ReturnsBadRequest()
        {
            // Given
            _controller.ModelState.AddModelError("Name", "Name is required");

            // When
            var wineBottleCreate = new WineBottleCreateModel { Name = "" };
            var result = await _controller.AddWineBottle(wineBottleCreate);

            // Then
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
        }

        [Fact]
        public async Task UpdateWineBottle_ValidData_ReturnsNoContent_And_ItemIsUpdated()
        {
            // Given
            var initialWineBottle = new WineBottleModel { Id = 1, Name = "Wine A", SizeInMilliLiter = 500 };
            var updatedWineBottle = new WineBottleModel { Id = 1, Name = "Wine A", SizeInMilliLiter = 750 };

            _mockWineBottleService.Setup(service => service.GetById(1))
                .ReturnsAsync(initialWineBottle);

            _mockWineBottleService
                .Setup(service => service.Update(It.Is<WineBottleModel>(wb => wb.Id == 1 && wb.SizeInMilliLiter == 750)))
                .Callback<WineBottleModel>(wb => {
                    initialWineBottle.SizeInMilliLiter = wb.SizeInMilliLiter;
                });

            // When
            var updateResult = await _controller.UpdateWineBottle(1, updatedWineBottle);

            // Then
            var noContentResult = updateResult as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.IsType<NoContentResult>(noContentResult);

            var getResult = await _controller.GetWineBottleById(1);

            var okObjectResult = getResult.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.IsType<OkObjectResult>(okObjectResult);

            var returnedWineBottle = okObjectResult.Value as WineBottleModel;
            Assert.NotNull(returnedWineBottle);
            Assert.Equal(750, returnedWineBottle.SizeInMilliLiter);
        }

        [Fact]
        public async Task DeleteWineBottleById_ValidId_ReturnsNoContent_And_DeletesBottle()
        {
            // Given
            var wineBottle1 = new WineBottleModel { Id = 1, Name = "Wine A", SizeInMilliLiter = 500 };
            var wineBottle2 = new WineBottleModel { Id = 2, Name = "Wine B", SizeInMilliLiter = 750 };

            var initialWineBottles = new List<WineBottleModel>
            {
                wineBottle1, wineBottle2
            };

            _mockWineBottleService.Setup(service => service.GetAll())
                .ReturnsAsync(initialWineBottles);

            _mockWineBottleService.Setup(service => service.Delete(1))
                .Callback<int>(id =>
                {
                    var list = initialWineBottles;
                    var itemToRemove = list.FirstOrDefault(b => b.Id == id);
                    if (itemToRemove != null)
                    {
                        list.Remove(itemToRemove);
                    }
                })
                .Returns(Task.CompletedTask);

            // When
            var deleteResult = await _controller.DeleteWineBottleById(1);

            // Then
            Assert.IsType<NoContentResult>(deleteResult);

            var getAllResult = await _controller.GetAllWineBottles();
            var actionResult = Assert.IsType<ActionResult<IEnumerable<WineBottleModel>>>(getAllResult);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            var remainingBottles = Assert.IsType<List<WineBottleModel>>(okResult.Value);
            Assert.Single(remainingBottles);
            Assert.Contains(wineBottle2, remainingBottles);
        }

        [Fact]
        public async Task FilterWineBottles_ValidFilter_ReturnsFilteredWineBottles()
        {
            // Given
            var wineBottles = new List<WineBottleModel>
            {
                new() { Id = 1, Name = "Wine A", SizeInMilliLiter = 500, Year = 2020 },
                new() { Id = 2, Name = "Wine B", SizeInMilliLiter = 750, Year = 2019 },
                new() { Id = 3, Name = "Wine C", SizeInMilliLiter = 500, Year = 2024 }
            };

            _mockWineBottleService.Setup(service => service.Filter(null, 500, null, null, null, null))
                .ReturnsAsync(wineBottles.Where(w => w.SizeInMilliLiter == 500));

            // When
            var result = await _controller.FilterWineBottles(null, 500, null, null, null, null);

            // Then
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.IsType<OkObjectResult>(okResult);

            var model = okResult.Value as IEnumerable<WineBottleModel>;
            Assert.NotNull(model);
            Assert.Equal(2, model.Count());
            Assert.Contains(model, b => b.Name == "Wine A");
            Assert.Contains(model, b => b.Name == "Wine C");
        }
    }
}
