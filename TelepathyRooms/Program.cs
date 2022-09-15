using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TelepathyRooms.Services.CsvServices;
using TelepathyRooms.Services.BookingServices;
using TelepathyRooms.Services.MenuService;

class Program
{
    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder();
        BuildConfig(builder);

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddScoped<ICsvService, CsvService>();
                services.AddScoped<IBookingService, BookingService>();
                services.AddScoped<IMenuService, MenuService>();
            })
            .Build();

        var svc = ActivatorUtilities.CreateInstance<MenuService>(host.Services);
        svc.Run();

        Console.ReadLine();
    }

    static void BuildConfig(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    }
}