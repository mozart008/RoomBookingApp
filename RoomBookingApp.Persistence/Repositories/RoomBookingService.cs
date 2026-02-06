using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Domain;

namespace RoomBookingApp.Persistence.Repositories
{
    public class RoomBookingService : IRoomBookingService
    {
        IEnumerable<Room> IRoomBookingService.GetAvailableRooms(DateTime date)
        {
            throw new NotImplementedException();
        }

        void IRoomBookingService.Save(RoomBooking roomBooking)
        {
            throw new NotImplementedException();
        }
    }
}
