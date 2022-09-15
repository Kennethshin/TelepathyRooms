using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelepathyRooms.ViewModel
{
    public class CsvObjectMapper : ClassMap<CsvObjectViewModel>
    {
        public CsvObjectMapper()
        {
            Map(m => m.RoomNumber).Name("roomNumber");
            Map(m => m.Status).Name("status");
        }
    }
}
