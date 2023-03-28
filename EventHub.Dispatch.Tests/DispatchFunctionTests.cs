using EventHub.Common.Entities;
using EventHub.Common.ServiceBus;
using EventHub.Common.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace EventHub.Dispatch.Tests
{
    [TestClass]
    public class DispatchFunctionTests
    {
        private Mock<IEventValidator> _mockEventValidator;
        private Mock<IQueueMessageHandler> _mockQueueMessageHandler;
        private Mock<ILogger> _mockLogger;
        private DispatchFunction function;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockLogger = new Mock<ILogger>();
            _mockEventValidator = new Mock<IEventValidator>();
            _mockQueueMessageHandler = new Mock<IQueueMessageHandler>();
            function = new DispatchFunction(_mockEventValidator.Object, _mockQueueMessageHandler.Object);
        }

        [TestMethod]
        public async Task Run_EventFailsValidation_ReturnsBadRequest()
        {
            _mockEventValidator.Setup(x => x.IsValid(It.IsAny<Event>())).Returns(false);

            var eventData = new Event();
            var body = JsonConvert.SerializeObject(eventData);
            var httpRequest = HttpRequestSetup(body);

            var result = await function.Run(httpRequest, _mockLogger.Object) as BadRequestObjectResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Run_PassesValidationAndCallsMessageHandler_ReturnsOkResult()
        {
            _mockEventValidator.Setup(x => x.IsValid(It.IsAny<Event>())).Returns(true);

            var eventData = new Event();
            var body = JsonConvert.SerializeObject(eventData);
            var httpRequest = HttpRequestSetup(body);

            var result = await function.Run(httpRequest, _mockLogger.Object) as OkResult;

            Assert.IsNotNull(result);
            _mockQueueMessageHandler.Verify(x => x.HandleMessageAsync(It.IsAny<Event>()), Times.Once);
        }
        
        [TestMethod]
        public async Task Run_MessageHandlerThrowsException_Returns500Response()
        {
            _mockEventValidator.Setup(x => x.IsValid(It.IsAny<Event>())).Returns(true);

            _mockQueueMessageHandler.Setup(x => x.HandleMessageAsync(It.IsAny<Event>())).ThrowsAsync(new Exception());

            var eventData = new Event();
            var body = JsonConvert.SerializeObject(eventData);
            var httpRequest = HttpRequestSetup(body);

            var result = await function.Run(httpRequest, _mockLogger.Object) as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(500, result.StatusCode);
        }

        private HttpRequest HttpRequestSetup(string body)
        {
            var mockHttpRequest = new Mock<HttpRequest>();

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(body);
            writer.Flush();
            stream.Position = 0;
            mockHttpRequest.Setup(req => req.Body).Returns(stream);
            return mockHttpRequest.Object;
        }
    }
}