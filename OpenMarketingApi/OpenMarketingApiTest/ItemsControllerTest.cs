namespace OpenMarketingApiTest
{
    public class ItemsControllerTest
    {
        private readonly ItemsController _controller;
        private readonly Mock<ILogger<ItemsController>> _mockLogger;
        private readonly Mock<IItemsService> _services;

        public ItemsControllerTest()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            _services = new Mock<IItemsService>();
            _mockLogger = new Mock<ILogger<ItemsController>>();

            _controller = new ItemsController(_services.Object, _mockLogger.Object)
            {
                ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        RequestServices = serviceProvider.Object,
                    }
                }
            };
        }

        [Fact]
        public async Task GetPicture_ReturnsOkResult_WhenPictureExists()
        {
            // Arrange
            var itemId = "yourItemId";
            _services.Setup(service => service.GetPictureAsync(itemId)).ReturnsAsync(new byte[] { 1, 2, 3 });

            // Act
            var result = await _controller.GetPictureAsync(itemId);

            // Assert
            result.Should().NotBeNull();
            _services.Verify(service => service.GetPictureAsync(It.IsAny<string>()), Times.Once);

            var okResult = result.Result as ObjectResult;
            okResult?.Value.Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
        }

        [Fact]
        public async Task GetPicture_ReturnsNotFoundResult_WhenPictureDoesNotExist()
        {
            // Arrange
            var itemId = "yourItemId";
            _services.Setup(service => service.GetPictureAsync(itemId)).ReturnsAsync((byte[])null);

            // Act
            var result = await _controller.GetPictureAsync(itemId);

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result as ObjectResult;
            okResult?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetPicture_ServiceException_ReturnsErrorResult()
        {
            // Arrange
            var itemId = "yourItemId"; // Provide a valid item ID
            var exceptionMessage = "Service exception message";

            _services.Setup(s => s.GetPictureAsync(itemId)).ThrowsAsync(new ServiceException(System.Net.HttpStatusCode.InternalServerError, exceptionMessage));

            // Act
            var result = await _controller.GetPictureAsync(itemId);

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Result as ObjectResult;
            okResult?.StatusCode.Should().Be(500);
        }
    }
}
