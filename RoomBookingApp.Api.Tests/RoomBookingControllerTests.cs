using Microsoft.AspNetCore.Mvc;
using Moq;
using RoomBookingApp.Api.Controllers;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Api.Tests
{
    public class RoomBookingControllerTests
    {
        private readonly Mock<IRoomBookingRequestProcessor> _roomBookingRequestProcessor;
        private readonly RoomBookingController _controller;
        private readonly RoomBookingRequest _request;
        private readonly RoomBookingResult _result;

        public RoomBookingControllerTests()
        {
            _roomBookingRequestProcessor = new Mock<IRoomBookingRequestProcessor>();
            _controller = new RoomBookingController(_roomBookingRequestProcessor.Object);
            _request = new RoomBookingRequest();
            _result = new RoomBookingResult();

            _roomBookingRequestProcessor.Setup(x => x.BookRoom(_request)).Returns(_result);
        }

        [Theory]
        [InlineData(1, true, typeof(OkObjectResult), BookingResultFlag.Success)]
        [InlineData(0, false, typeof(BadRequestObjectResult), BookingResultFlag.Failure)]
        public async Task Should_Call_Booking_Method_When_Valid(int expectedMethodCalls, bool isModelValid,
            Type expectedActionResultType, BookingResultFlag bookingResultFlag)
        {
            //Arrange
            if (!isModelValid)
            {
                _controller.ModelState.AddModelError("key", "error message");
            }

            _result.Flag = bookingResultFlag;

            //Act
            var result = await _controller.BookRoom(_request);

            //Assert
            result.ShouldBeOfType(expectedActionResultType);
            _roomBookingRequestProcessor.Verify(x => x.BookRoom(_request), Times.Exactly(expectedMethodCalls));
        }

    }
}
