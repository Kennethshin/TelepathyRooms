using TelepathyRooms.ViewModel;

namespace TelepathyRooms.Services.BookingServices
{
    public interface IBookingService
    {
        string AssignRoom(List<HotelRoom> hotelRooms);
        bool CheckOutRoom(List<HotelRoom> hotelRooms, string roomNumber);
        bool CleanRoom(List<HotelRoom> hotelRooms, string roomNumber);
        List<HotelRoom> GetAllAvailableRooms(List<HotelRoom> hotelRooms);
        List<HotelRoom>? GetRoomPath(List<HotelRoom> hotelRooms);
        bool OutOfServiceRoom(List<HotelRoom> hotelRooms, string roomNumber);
        bool RepairedRoom(List<HotelRoom> hotelRooms, string roomNumber);
    }
}