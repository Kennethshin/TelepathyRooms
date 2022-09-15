using TelepathyRooms.ViewModel;
using TelepathyRooms.Helper;
using TelepathyRooms.Services.BookingServices;
using TelepathyRooms.Services.CsvServices;

namespace TelepathyRooms.Services.MenuService
{
    public class MenuService : IMenuService
    {
        private readonly IBookingService _bookingService;
        private readonly ICsvService _csvService;

        public MenuService(IBookingService bookingService, ICsvService csvService)
        {
            _bookingService = bookingService;
            _csvService = csvService;
        }

        public void Run()
        {
            bool IsExit = false;
            string Input;
            var csvResults = _csvService.ImportCsv();

            if (csvResults == null || csvResults.HotelRooms == null)
            {
                Console.Write("There is not hotels in the CSV file");
                return;
            }

            var hotelRooms = _bookingService.GetRoomPath(csvResults.HotelRooms);
            PrintRooms(hotelRooms);

            while (!IsExit)
            {
                Console.WriteLine("==================");
                Console.WriteLine("Hotel Rooms, please state your action. Enter empty key to exit");
                Console.WriteLine("1. Get all available rooms");
                Console.WriteLine("2. Assign the closest room");
                Console.WriteLine("3. Checkout room");
                Console.WriteLine("4. Clean room");
                Console.WriteLine("5. Set Room to Out of Service");
                Console.WriteLine("6. Repair room");

                Input = Console.ReadLine();

                switch (Input)
                {
                    case "1":
                        {
                            DisplayAvailableRooms(hotelRooms);
                            break;
                        }
                    case "2":
                        {
                            AssignHotelRoom(hotelRooms);
                            break;
                        }
                    case "3":
                        {

                            CheckOutRoom(hotelRooms);
                            break;
                        }
                    case "4":
                        {
                            CleanRoom(hotelRooms);
                            break;
                        }
                    case "5":
                        {
                            OutOfServiceRoom(hotelRooms);
                            break;
                        }
                    case "6":
                        {
                            RepairRoom(hotelRooms);
                            break;
                        }

                    default:
                        IsExit = true;
                        break;
                }
            }
        }

        private void PrintRooms(List<HotelRoom> hotelRooms)
        {
            if (hotelRooms == null || hotelRooms.Count == 0) return;

            var groupedByFloor = hotelRooms.GroupBy(x => x.FloorNo).ToList();
            var rooms = new List<HotelRoom>();

            groupedByFloor.Reverse();
            
            foreach(var floorRoom in groupedByFloor)
            {
                rooms = floorRoom.ToList();
                rooms.Reverse();
                foreach (var hotelRoom in rooms)
                {
                    Console.Write($"{hotelRoom.RoomNumber} - [{hotelRoom.Status}] || ");
                }
                Console.WriteLine();
            }
        }

        private void DisplayAvailableRooms(List<HotelRoom> hotelRooms)
        {
            var availableRoom = _bookingService.GetAllAvailableRooms(hotelRooms);
            PrintRooms(availableRoom);
        }

        private void AssignHotelRoom(List<HotelRoom> hotelRooms)
        {
            var assignedRoom = _bookingService.AssignRoom(hotelRooms);
            if (!String.IsNullOrEmpty(assignedRoom)) Console.WriteLine($"Successfully assigned room {assignedRoom}");
            PrintRooms(hotelRooms);
        }

        private void CheckOutRoom(List<HotelRoom> hotelRooms)
        {
            Console.WriteLine("Please enter Room Number to Check Out");
            string roomNumber = Console.ReadLine();
            var isCheckedOut = _bookingService.CheckOutRoom(hotelRooms, roomNumber);
            if (isCheckedOut) Console.WriteLine($"Successfully checked out room {roomNumber}");
            else Console.WriteLine($"Unsuccessfully checked out room {roomNumber}");

            PrintRooms(hotelRooms);
        }

        private void CleanRoom(List<HotelRoom> hotelRooms)
        {
            Console.WriteLine("Please enter Room Number to Clean");
            string roomNumber = Console.ReadLine();
            var isCleaned = _bookingService.CleanRoom(hotelRooms, roomNumber);
            if (isCleaned) Console.WriteLine($"Successfully clean room {roomNumber}");
            else Console.WriteLine($"Unsuccessfully clean room {roomNumber}");

            PrintRooms(hotelRooms);
        }

        private void OutOfServiceRoom(List<HotelRoom> hotelRooms)
        {
            Console.WriteLine("Please enter Room Number to Out of Service");
            string roomNumber = Console.ReadLine();
            var isCleaned = _bookingService.OutOfServiceRoom(hotelRooms, roomNumber);
            if (isCleaned) Console.WriteLine($"Successfully out of service room {roomNumber}");
            else Console.WriteLine($"Unsuccessfully out of service room {roomNumber}");

            PrintRooms(hotelRooms);
        }

        private void RepairRoom(List<HotelRoom> hotelRooms)
        {
            Console.WriteLine("Please enter Room Number to Repair");
            string roomNumber = Console.ReadLine();
            var isCleaned = _bookingService.RepairedRoom(hotelRooms, roomNumber);
            if (isCleaned) Console.WriteLine($"Successfully repaired room {roomNumber}");
            else Console.WriteLine($"Unsuccessfully repaired room {roomNumber}");

            PrintRooms(hotelRooms);
        }

    }
}
