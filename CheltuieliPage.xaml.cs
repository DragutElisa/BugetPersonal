using BugetPersonal.Data;
using BugetPersonal.Models;
using System.Collections.ObjectModel;

namespace BugetPersonal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheltuieliPage : ContentPage
    {


        private readonly CheltuieliDatabase _database;
        public ObservableCollection<Cheltuieli> Cheltuieli { get; set; } = new();
        public Command LoadCheltuielisCommand { get; }

        public CheltuieliPage(CheltuieliDatabase database)
        {
            InitializeComponent();
            _database = database;

            LoadCheltuielisCommand = new Command(async () => await LoadCheltuielis());

            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadCheltuielis();
        }

        public async Task LoadCheltuielis()
        {
            {
                try
                {
                    Cheltuieli.Clear(); // sau Cheltuielis.Clear() în funcție de numele ales
                    var cheltuielis = await _database.GetCheltuieliAsync();

                    // Adaugă mesaj de debug
                    await DisplayAlert("Debug", $"Încărcate {cheltuielis.Count} cheltuieli din baza de date", "OK");

                    foreach (var cheltuieli in cheltuielis)
                    {
                        Cheltuieli.Add(cheltuieli); // sau Cheltuielis.Add() în funcție de numele ales
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Eroare", $"Nu s-au putut încărca cheltuielile: {ex.Message}", "OK");
                }
                finally
                {
                    RefreshView.IsRefreshing = false;
                }
            }
        }

        private async void OnAddCheltuieliClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CheltuieliDetailPage(new Cheltuieli { Date = DateTime.Today }, _database));
        }

        private async void OnCheltuieliSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not Cheltuieli selectedCheltuieli)
                return;

            await Navigation.PushAsync(new CheltuieliDetailPage(selectedCheltuieli, _database));

            // Deselectează elementul
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}