using BugetPersonal.Data;
using BugetPersonal.Models;
using System.Collections.ObjectModel;

namespace BugetPersonal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheltuieliDetailPage : ContentPage
    {

        private readonly CheltuieliDatabase _database;
        public Cheltuieli Cheltuieli { get; set; }
        public ObservableCollection<CheltuieliCategory> Categories { get; set; }
        public CheltuieliCategory SelectedCategory { get; set; }
        public bool IsExistingCheltuieli => Cheltuieli?.Id != 0;
        public string PageTitle => IsExistingCheltuieli ? "Editare cheltuială" : "Adăugare cheltuială";

        public CheltuieliDetailPage(Cheltuieli cheltuieli, CheltuieliDatabase database)
        {
            InitializeComponent();
            _database = database;
            Cheltuieli = cheltuieli;

            // Populare categorii
            Categories = new ObservableCollection<CheltuieliCategory>(CheltuieliCategory.GetCategories());

            // Selectarea categoriei existente dacă este o cheltuială existentă
            if (!string.IsNullOrEmpty(Cheltuieli.Category))
            {
                SelectedCategory = Categories.FirstOrDefault(c => c.Name == Cheltuieli.Category);
            }

            BindingContext = this;
        }

        private void OnCategorySelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is CheltuieliCategory category)
            {
                Cheltuieli.Category = category.Name;
                Cheltuieli.CategoryIcon = category.IconSource;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Cheltuieli.Description))
            {
                await DisplayAlert("Eroare", "Descrierea nu poate fi goală", "OK");
                return;
            }
            if (Cheltuieli.Amount <= 0)
            {
                await DisplayAlert("Eroare", "Suma trebuie să fie pozitivă", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(Cheltuieli.Category))
            {
                await DisplayAlert("Eroare", "Te rugăm să selectezi o categorie", "OK");
                return;
            }

            int result = await _database.SaveCheltuialaAsync(Cheltuieli);
            // Verifică rezultatul
            if (result > 0)
            {
                await DisplayAlert("Succes", "Cheltuiala a fost salvată cu succes!", "OK");
            }
            else
            {
                await DisplayAlert("Eroare", "Nu s-a putut salva cheltuiala", "OK");
            }
            await Navigation.PopAsync();
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (!IsExistingCheltuieli)
                return;

            bool confirm = await DisplayAlert("Confirmare", "Ești sigur că vrei să ștergi această cheltuială?", "Da", "Nu");
            if (confirm)
            {
                int result = await _database.DeleteCheltuieliByIdAsync(Cheltuieli.Id);
                if (result > 0)
                {
                    await DisplayAlert("Succes", "Cheltuiala a fost ștearsă!", "OK");
                    // Restul codului tău
                }
                else
                {
                    await DisplayAlert("Eroare", "Nu s-a putut șterge cheltuiala.", "OK");
                }
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}