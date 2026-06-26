using BugetPersonal.Data;
using BugetPersonal.Models;
using System.Collections.ObjectModel;

namespace BugetPersonal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VenituriPage : ContentPage
    {
        private readonly VenituriDatabase _database;
        public ObservableCollection<Venit> Venituri { get; set; } = new();
        public Command LoadVenituriCommand { get; }

        public VenituriPage(VenituriDatabase database)
        {
            InitializeComponent();
            _database = database;
            LoadVenituriCommand = new Command(async () => await LoadVenituri());
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadVenituri();
        }

        private async Task LoadVenituri()
        {
            try
            {
                Venituri.Clear();
                var venituriList = await _database.GetVenituriAsync();
                foreach (var venit in venituriList)
                {
                    Venituri.Add(venit);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eroare", $"Nu s-au putut încărca veniturile: {ex.Message}", "OK");
            }
            finally
            {
                venituriRefreshView.IsRefreshing = false;
            }
        }

        private async void OnAddVenituriClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VenitDetailPage(new Venit { Date = DateTime.Today }, _database));
        }

        private async void OnVenituriSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not Venit selectedVenit)
                return;

            await Navigation.PushAsync(new VenitDetailPage(selectedVenit, _database));
            // Deselectează elementul
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
