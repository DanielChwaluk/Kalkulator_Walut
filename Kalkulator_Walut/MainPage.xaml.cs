namespace Kalkulator_Walut
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void OnEuroClicked(object sender, EventArgs e)
        {
            euroNaPlnBtn.IsEnabled = false;
            plnNaEuroBtn.IsEnabled = true;
        }

        private void OnPlnClicked(object sender, EventArgs e)
        {
            plnNaEuroBtn.IsEnabled = false;
            euroNaPlnBtn.IsEnabled = true;
        }

        private void OnPrzeliczClicked(object sender, EventArgs e)
        {
            if (euroNaPlnBtn.IsEnabled==true)
                otrzymaszLbl.Text = ( Math.Round(float.Parse(kwotaEnt.Text) * float.Parse(kursLbl.Text),2)).ToString() + "PLN";
            else
                otrzymaszLbl.Text = (Math.Round(float.Parse(kwotaEnt.Text) / float.Parse(kursLbl.Text),2)).ToString() + "€";
            SemanticScreenReader.Announce(otrzymaszLbl.Text);
            lblJSON.Text = "";
        }
    }
}