using CsvHelper;
using TelepathyRooms.ViewModel;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using TelepathyRooms.Services.BookingServices;
using TelepathyRooms.Helper;

namespace TelepathyRooms.Services.CsvServices
{
    public class CsvService : ICsvService
    {
        private readonly IConfiguration _configuration;
        private readonly IBookingService _bookingService;
        private readonly FloorNumberHelper _floorNumberHelper = new FloorNumberHelper();

        public CsvService(IConfiguration configuration, IBookingService bookingService)
        {
            _configuration = configuration;
            _bookingService = bookingService;
        }

        public CsvResult? ImportCsv()
        {
            List<CsvObjectViewModel> csvObjects = new List<CsvObjectViewModel>();
            List<HotelRoom> hotelRooms = new List<HotelRoom>();

            var csvPath = _configuration["csvPath"];
            if (csvPath == null)
            {
                Console.WriteLine($"{csvPath} is a invalid path");
                return null;
            }


            using (var reader = new StreamReader($"{csvPath}"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<CsvObjectMapper>();
                csvObjects = csv.GetRecords<CsvObjectViewModel>().ToList();
            }

            if (csvObjects != null && csvObjects.Count > 0)
            {
                hotelRooms = csvObjects.Select(x => new HotelRoom
                {
                    RoomNumber = x.RoomNumber,
                    Status = x.Status,
                    FloorNo = _floorNumberHelper.GetFloorNumber(x.RoomNumber)
                }).ToList();
            }

            hotelRooms = hotelRooms.OrderBy(x => x.RoomNumber).ToList();

            CsvResult csvResults = new CsvResult() { HotelRooms = hotelRooms };

            return csvResults;
        }
    }
}
