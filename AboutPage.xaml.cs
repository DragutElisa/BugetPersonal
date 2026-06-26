using BugetPersonal.Data;
using BugetPersonal.Models;

namespace BugetPersonal;

public partial class AboutPage : ContentPage
{
    private ChartViewModel _viewModel;

    public AboutPage()
    {
        InitializeComponent();
        _viewModel = (ChartViewModel)BindingContext;

        // Înregistrăm pentru evenimentul când pagina devine vizibilă
        this.Appearing += AboutPage_Appearing;
    }

    private void AboutPage_Appearing(object sender, EventArgs e)
    {
        // Reîncărcăm datele când pagina devine vizibilă
        _viewModel?.LoadData();
    }

    private async void OnCheltuieliTapped(object sender, EventArgs e)
    {
        var page = new CheltuieliPage(App.CheltuieliDatabase);
        // Când ne întoarcem de la pagina de cheltuieli, reîncărcăm datele
        page.Disappearing += (s, args) => _viewModel?.LoadData();
        await Navigation.PushAsync(page);
    }
}