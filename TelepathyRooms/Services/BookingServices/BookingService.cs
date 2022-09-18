using TelepathyRooms.ViewModel;
using TelepathyRooms.Helper;

namespace TelepathyRooms.Services.BookingServices
{
    public class BookingService : IBookingService
    {
        private RoomStatusHelper _roomStatusHelper = new RoomStatusHelper();
        private FloorNumberHelper _floorNumberHelper = new FloorNumberHelper();

        public BookingService()
        {
        }

        public List<HotelRoom>? GetRoomPath(List<HotelRoom> hotelRooms)
        {
            if (hotelRooms == null || hotelRooms.Count == 0) return null;

            var groupedByFloor = hotelRooms.GroupBy(x => x.FloorNo).ToList();
            int roomLength = 0;
            int totalRooms = 0;
            int count = 0;

            foreach (var hotelFloor in groupedByFloor)
            {
                roomLength = hotelFloor.ToList().Count();
                totalRooms += roomLength;

                if (hotelFloor.Key % 2 == 0)
                {
                    count = 0;
                    foreach(var room in hotelFloor.ToList())
                    {
                        room.OrderNumber = totalRooms - count;
                        count++;
                    }
                }
                else
                {
                    count = roomLength;
                    foreach (var room in hotelFloor.ToList())
                    {
                        count--;
                        room.OrderNumber = totalRooms - count;
                    }
                }
            }

            return hotelRooms;
        }

        public List<HotelRoom> GetAllAvailableRooms(List<HotelRoom> hotelRooms)
        {
            if (hotelRooms == null || hotelRooms.Count == 0) return null;

            List<HotelRoom> availableRooms = new List<HotelRoom>();

            availableRooms = hotelRooms.Where(hotels => hotels.Status == RoomStatus.Available).ToList();

            return availableRooms;
        }

        public string AssignRoom(List<HotelRoom> hotelRooms)
        {
            if (hotelRooms == null || hotelRooms.Count == 0) return String.Empty;

            HotelRoom? assignRoom = new HotelRoom();

            assignRoom = hotelRooms.OrderBy(x => x.OrderNumber).FirstOrDefault(x => x.Status == RoomStatus.Available);

            if (assignRoom != null)
            {
                var status = _roomStatusHelper.RoomStatusHandler(assignRoom.Status, RoomActions.CheckIn);

                if(status == RoomStatus.None) return String.Empty;

                assignRoom.Status = status;

                return assignRoom.RoomNumber;
            }
            else
                return String.Empty;
        }

        public bool CheckOutRoom(List<HotelRoom> hotelRooms, string roomNumber)
        {
            if (hotelRooms == null || hotelRooms.Count == 0 || string.IsNullOrEmpty(roomNumber)) return false;

            int floorNumber = _floorNumberHelper.GetFloorNumber(roomNumber);
            var hotelRoom = hotelRooms.Where(x => x.FloorNo == floorNumber && x.RoomNumber == roomNumber).FirstOrDefault();

            if (hotelRoom != null)
            {
                if (hotelRoom.Status == RoomStatus.Occupied)
                {
                    var status = _roomStatusHelper.RoomStatusHandler(hotelRoom.Status, RoomActions.CheckOut);

                    if (status == RoomStatus.None) return false;

                    hotelRoom.Status = status;
                    return true;
                }
                else return false;
            }

            return false;
        }

        public bool CleanRoom(List<HotelRoom> hotelRooms, string roomNumber)
        {
            if (hotelRooms == null || hotelRooms.Count == 0 || string.IsNullOrEmpty(roomNumber)) return false;

            int floorNumber = _floorNumberHelper.GetFloorNumber(roomNumber);
            var hotelRoom = hotelRooms.Where(x => x.FloorNo == floorNumber && x.RoomNumber == roomNumber).FirstOrDefault();

            if (hotelRoom != null)
            {
                if (hotelRoom.Status == RoomStatus.Vacant)
                {
                    var status = _roomStatusHelper.RoomStatusHandler(hotelRoom.Status, RoomActions.Clean);

                    if (status == RoomStatus.None) return false;

                    hotelRoom.Status = status;
                    return true;
                }
                else return false;
            }

            return false;
        }

        public bool OutOfServiceRoom(List<HotelRoom> hotelRooms, string roomNumber)
        {
            if (hotelRooms == null || hotelRooms.Count == 0 || string.IsNullOrEmpty(roomNumber)) return false;

            int floorNumber = _floorNumberHelper.GetFloorNumber(roomNumber);
            var hotelRoom = hotelRooms.Where(x => x.FloorNo == floorNumber && x.RoomNumber == roomNumber).FirstOrDefault();

            if (hotelRoom != null)
            {
                if (hotelRoom.Status == RoomStatus.Vacant)
                {
                    var status = _roomStatusHelper.RoomStatusHandler(hotelRoom.Status, RoomActions.OutOfService);

                    if (status == RoomStatus.None) return false;

                    hotelRoom.Status = status;
                    return true;
                }
                else return false;
            }

            return false;
        }

        public bool RepairedRoom(List<HotelRoom> hotelRooms, string roomNumber)
        {
            if (hotelRooms == null || hotelRooms.Count == 0 || string.IsNullOrEmpty(roomNumber)) return false;

            int floorNumber = _floorNumberHelper.GetFloorNumber(roomNumber);
            var hotelRoom = hotelRooms.Where(x => x.FloorNo == floorNumber && x.RoomNumber == roomNumber).FirstOrDefault();

            if (hotelRoom != null)
            {
                if (hotelRoom.Status == RoomStatus.Repair)
                {
                    var status = _roomStatusHelper.RoomStatusHandler(hotelRoom.Status, RoomActions.Repaired);

                    if (status == RoomStatus.None) return false;

                    hotelRoom.Status = status;
                    return true;
                }
                else return false;
            }

            return false;
        }
    }
}
