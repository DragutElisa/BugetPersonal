using BugetPersonal.Data;
using System.IO;
namespace BugetPersonal
{
    public partial class App : Application
    {
        // Inițializează bazele de date din constructor, nu la prima accesare
        public static CheltuieliDatabase CheltuieliDatabase { get; private set; }
        public static VenituriDatabase VenituriDatabase { get; private set; }

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzgyNzg4NkAzMjM5MmUzMDJlMzAzYjMyMzkzYkZKUzh5aFFyMXlWbDVxeVhXV2dtSUNqKzVuelVXaHFhbjVwNmJWSFdQNlU9");

            // Creează bazele de date imediat la pornirea aplicației
            string cheltuieliDbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CheltuieliList.db3");
            CheltuieliDatabase = new CheltuieliDatabase(cheltuieliDbPath);

            string venituriDbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VenitList.db3");
            VenituriDatabase = new VenituriDatabase(venituriDbPath);

            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}