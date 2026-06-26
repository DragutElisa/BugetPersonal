namespace BugetPersonal
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }
        private async void OnCheltuieliClicked(object sender, EventArgs e)
        {
            // Navigare către pagina de cheltuieli
            await Navigation.PushAsync(new AboutPage());
        }

        private async void OnVenituriClicked(object sender, EventArgs e)
        {
            // Navigare către pagina de venituri
            await Navigation.PushAsync(new VenituriView());


        }

    }
}