using EventHub.Common.Entities;
using EventHub.Common.Validators;
using EventHub.Listener.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace EventHub.Listener.Tests
{
    [TestClass]
    public class ListenerFunctionTests
    {
        private Mock<IEventValidator> _mockEventValidator;
        private Mock<ILogger> _mockLogger;
        private ListenerFunction function;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockLogger = new Mock<ILogger>();
            _mockEventValidator = new Mock<IEventValidator>();
            function = new ListenerFunction(_mockEventValidator.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(UnableToProcessEventException))]
        public void Run_EventIsInvalid_ThrowsException()
        {
            _mockEventValidator.Setup(x => x.IsValid(It.IsAny<Event>())).Returns(false);

            var eventData = new Event();
            var body = JsonConvert.SerializeObject(eventData);

            function.Run(body, _mockLogger.Object);
        }

        [TestMethod]
        public void Run_EventIsValid_ProcessesSuccessfully()
        {
            _mockEventValidator.Setup(x => x.IsValid(It.IsAny<Event>())).Returns(true);

            var eventData = new Event();
            var body = JsonConvert.SerializeObject(eventData);

            function.Run(body, _mockLogger.Object);

            Assert.AreEqual(_mockLogger.Invocations.Count, 2);
        }
    }
}