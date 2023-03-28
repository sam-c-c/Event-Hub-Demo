using EventHub.Common.Entities;
using EventHub.Common.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHub.Common.Tests.Validators
{
    [TestClass]
    public class EventValidatorTests
    {
        [TestMethod]
        public void IsValid_PassInNullEvent_ReturnsFalse()
        {
            var result = new EventValidator().IsValid(null);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValid_PassInNullApplicationId_ReturnsFalse()
        {
            var result = new EventValidator().IsValid(new Event() { ApplicationName = "AppName", EventId = 123, EventName = "EventName" });
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValid_PassInNullApplicationName_ReturnsFalse()
        {
            var result = new EventValidator().IsValid(new Event() { ApplicationId = Guid.NewGuid().ToString(), EventId = 123, EventName = "EventName" });
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValid_PassInvalidEventId_ReturnsFalse()
        {
            var result = new EventValidator().IsValid(new Event() { ApplicationId = Guid.NewGuid().ToString(), ApplicationName = "AppName", EventName = "EventName" });
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValid_PassInNullEventName_ReturnsFalse()
        {
            var result = new EventValidator().IsValid(new Event() { ApplicationId = Guid.NewGuid().ToString(), ApplicationName = "AppName", EventId = 123 });
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValid_PassValidEvent_ReturnsTrue()
        {
            var result = new EventValidator().IsValid(new Event() { ApplicationId = Guid.NewGuid().ToString(), ApplicationName = "AppName", EventId = 123, EventName = "EventName" });
            Assert.IsTrue(result);
        }
    }
}
