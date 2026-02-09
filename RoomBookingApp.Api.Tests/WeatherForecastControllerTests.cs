using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using RoomBookingApp.Api.Controllers;
using Shouldly;

namespace RoomBookingApp.Api.Tests
{
    public class WeatherForecastControllerTests
    {
        [Fact]
        public void Shoudl_Return_Forcast_Results()
        {
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMock.Object);
            var result = controller.Get();

            result.Count().ShouldBeGreaterThan(1);
            result.ShouldNotBe(null);
        }
    }
}
