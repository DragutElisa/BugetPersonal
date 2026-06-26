using BugetPersonal.Data;
using BugetPersonal.Models;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace BugetPersonal
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder

                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "cheltuieli.db3");
            builder.Services.AddSingleton<CheltuieliDatabase>(s => new CheltuieliDatabase(dbPath));
            builder.Services.AddTransient<Cheltuieli>();
            builder.Services.AddTransient<Cheltuieli>();

            // Adaugă serviciul de bază de date
            string dbPath1 = Path.Combine(FileSystem.AppDataDirectory, "venituri.db3");
            builder.Services.AddSingleton<VenituriDatabase>(s => new VenituriDatabase(dbPath));

            // Înregistrează paginile
            builder.Services.AddTransient<Venit>();
            builder.Services.AddTransient<Venit>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();


        }
    }
}