using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelepathyRooms.ViewModel
{
    public class CsvObjectViewModel : ClassMap<CsvObjectViewModel>
    {
        [Name("id")]
        public Guid Id { get; set; }

        [Name("roomNumber")]
        public string RoomNumber { get; set; }

        [Name("status")]
        public string Status { get; set; }
    }

    public class CsvResult
    {
        public List<HotelRoom> HotelRooms { get; set; }

    }
}
