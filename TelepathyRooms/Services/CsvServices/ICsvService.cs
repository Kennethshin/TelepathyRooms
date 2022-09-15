using TelepathyRooms.ViewModel;

namespace TelepathyRooms.Services.CsvServices
{
    public interface ICsvService
    {
        CsvResult? ImportCsv();
    }
}