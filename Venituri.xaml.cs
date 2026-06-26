using BugetPersonal.Data;
using BugetPersonal.Models;

namespace BugetPersonal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VenituriView : ContentPage
    {
        private VenituriChartViewModel _viewModel;

        public VenituriView()
        {
            InitializeComponent();
            _viewModel = (VenituriChartViewModel)BindingContext;

            // Înregistr?m pentru evenimentul când pagina devine vizibil?
            this.Appearing += VenituriView_Appearing;
        }

        private void VenituriView_Appearing(object sender, EventArgs e)
        {
            // Reînc?rc?m datele când pagina devine vizibil?
            _viewModel?.LoadData();
        }

        private async void OnVenituriTapped(object sender, EventArgs e)
        {
            var page = new VenituriPage(App.VenituriDatabase);
            // Când ne întoarcem de la pagina de venituri, reînc?rc?m datele
            page.Disappearing += (s, args) => _viewModel?.LoadData();
            await Navigation.PushAsync(page);
        }

        private void OnCarouselViewItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            if (e.CurrentItem is Venit venit)
            {
                // Aici pute?i ad?uga logic? suplimentar? când se schimb? elementul din CarouselView
                // De exemplu, pute?i actualiza alte UI elemente bazate pe venitul selectat
            }
        }
    }
}

