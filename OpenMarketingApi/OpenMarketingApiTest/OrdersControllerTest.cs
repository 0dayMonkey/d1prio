namespace OpenMarketingApiTest
{
    public class OrdersControllerTest
    {
        private readonly OrdersController _controller;
        private readonly Mock<ILogger<OrdersController>> _mockLogger;
        private readonly Mock<IOrdersServices> _services;

        public OrdersControllerTest()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            _services = new Mock<IOrdersServices>();
            _mockLogger = new Mock<ILogger<OrdersController>>();

            _controller = new OrdersController(_services.Object, _mockLogger.Object)
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
        public async Task CreateOrderAsync_ShouldReturnOK_WhenModelStateIsValid()
        {
            // Arrange
            var orderCommand = new OrderCommand() {

                PlayerId ="01",
                StoreId = "Casino01",
                OrderItems = new List<OrderItemCommand>
                {
                    new OrderItemCommand(){ItemId = "01", Quantity = 1},
                }
            };

            _services.Setup(service => service.CreateOrderAsync(It.IsAny<OrderCommand>()))
                            .ReturnsAsync(ExpecetedOrder());

            // Act
            var result = await _controller.CreateOrderAsync(orderCommand);

            // Assert
            result.Should().NotBeNull();

            _services.Verify(service => service.CreateOrderAsync(It.IsAny<OrderCommand>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("key", "error message");

            // Act
            var result = await _controller.CreateOrderAsync(new OrderCommand());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetOrdersAsync_ValidSearchModel_ReturnsOkResult()
        {
            // Arrange
            var searchModel = "{\"First\":0,\"Rows\":8,\"Sorts\":[{\"Active\":\"CreationTimestamp\",\"Direction\":\"asc\"},{\"Active\":\"Status\",\"Direction\":\"asc\"}],\"Filters\":[{\"Field\":\"ItemId\",\"Details\":[{\"Value\":\"13, 18\",\"MatchMode\":\"in\",\"Operator\":\"and\"}]},{\"Field\":\"Status\",\"Details\":[{\"Value\":\"approved\",\"MatchMode\":\"equal\",\"Operator\":\"and\"}]},{\"Field\":\"PlayerId\",\"Details\":[{\"Value\":\"2065\",\"MatchMode\":\"equal\",\"Operator\":\"and\"}]},{\"Field\":\"StoreId\",\"Details\":[{\"Value\":\"1\",\"MatchMode\":\"equal\",\"Operator\":\"and\"}]},{\"Field\":\"OrderId\",\"Details\":[{\"Value\":\"171\",\"MatchMode\":\"equal\",\"Operator\":\"and\"}]},{\"Field\":\"CreationTimestamp\",\"Details\":[{\"Value\":\"2023-08-01T17:19:56.170\",\"MatchMode\":\"dateAfter\",\"Operator\":\"and\"},{\"Value\":\"2023-09-20T09:01:55Z\",\"MatchMode\":\"dateBefore\",\"Operator\":\"and\"}]}]}";
            var expected = SearchResultOrder();
            _services.Setup(s => s.GetOrdersAsync(searchModel)).ReturnsAsync(expected);

            // Act
            var result = await _controller.GetOrdersAsync(searchModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as SearchResult<Order>;
            response.Should().NotBeNull();
            response?.Items.Should().BeEquivalentTo(expected.Items);
        }

        [Fact]
        public async Task GetOrdersAsync_ServiceException_ReturnsErrorResult()
        {
            // Arrange
            var searchModel = "{}";

            _services.Setup(s => s.GetOrdersAsync(searchModel)).ThrowsAsync(new ServiceException(HttpStatusCode.InternalServerError, "Error message"));

            // Act
            var result = await _controller.GetOrdersAsync(searchModel);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        private Order ExpecetedOrder()
        {
            return new Order
            {
                Total = 1,
                PlayerId = "01",
                Id = "001",
                Items = new List<OrderItem> { },
                CreationTimestamp = DateTime.UtcNow,
                Status = "201"
            };
        }

        private SearchResult<Order> SearchResultOrder()
        {
            return new SearchResult<Order>()
            {
                Items = new List<Order> { ExpecetedOrder() },
                First = 0,
                Rows = 10,
                TotalItems = 5
            };
        }
    }
}