using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Domain.BaseModels;

namespace RoomBookingApp.Core.Processors
{
    public class RoomBookingRequestProcessor : IRoomBookingRequestProcessor
    {
        private readonly IRoomBookingService _roomBookingService;

        public RoomBookingRequestProcessor(IRoomBookingService roomBookingService)
        {
            this._roomBookingService = roomBookingService;
        }

        public RoomBookingResult BookRoom(RoomBookingRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var availableRooms = _roomBookingService.GetAvailableRooms(request.Date);
            var result = CreateRoomBookingObject<RoomBookingResult>(request);

            if (availableRooms.Any())
            {
                var room = availableRooms.First();
                var roomBooking = CreateRoomBookingObject<Domain.RoomBooking>(request);
                roomBooking.RoomId = room.Id;
                _roomBookingService.Save(roomBooking);

                result.Flag = BookingResultFlag.Success;
                result.RoomBookingId = roomBooking.Id;
            }
            else
            {
                result.Flag = BookingResultFlag.Failure;
            }

            return result;
        }

        private static TRoomBooking CreateRoomBookingObject<TRoomBooking>(RoomBookingRequest bookingRequest) where TRoomBooking
            : RoomBookingBase, new()
        {
            return new TRoomBooking
            {
                FullName = bookingRequest.FullName,
                Email = bookingRequest.Email,
                Date = bookingRequest.Date
            };
        }
    }
}