using BugetPersonal.Data;
using BugetPersonal.Models;

namespace BugetPersonal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VenitDetailPage : ContentPage
    {
        private readonly VenituriDatabase _database;
        public Venit Venit { get; set; }
        public bool IsExistingVenit => Venit?.Id != 0;
        public string PageTitle => IsExistingVenit ? "Editare venit" : "Adăugare venit";

        public VenitDetailPage(Venit venit, VenituriDatabase database)
        {
            InitializeComponent();
            _database = database;
            Venit = venit;
            BindingContext = this;
        }

        async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                await _database.SaveVenitAsync(Venit);
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eroare", $"Nu s-a putut salva venitul: {ex.Message}", "OK");
            }
        }

        async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (!IsExistingVenit)
                return;

            bool confirm = await DisplayAlert("Confirmare", "Sigur doriți să ștergeți acest venit?", "Da", "Nu");
            if (confirm)
            {
                await _database.DeleteVenitAsync(Venit);
                await Navigation.PopAsync();
            }
        }

        async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
