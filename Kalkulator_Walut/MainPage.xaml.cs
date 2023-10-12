using System.Net;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;

namespace Kalkulator_Walut
{
    class Waluta
    {
        public string kodWaluty { get; private set; }
        public string waluta { get; private set;}
        public string data { get; private set; } = "2023-10-11";
        public double skup { get; private set; }
        public double sprzedaz { get; private set; }
        public Waluta(string code = "eur")
        {
            kodWaluty = code;
            string wynik;
            using (var webClient = new WebClient())
            {
                wynik = webClient.DownloadString("https://api.nbp.pl/api/exchangerates/rates/c/" + kodWaluty + "/" + data + "/?format=json");
                using JsonDocument j1 = JsonDocument.Parse(wynik);
                JsonElement json = j1.RootElement;
                waluta = json.GetProperty("currency").ToString();
                var rates = json.GetProperty("rates");
                var rate = rates[0];
                string ask = rate.GetProperty("ask").ToString();
                ask = ask.Replace('.', ',');
                string bid = rate.GetProperty("bid").ToString();
                bid = bid.Replace(".", ",");
                skup = double.Parse(ask);
                sprzedaz = double.Parse(bid);
            }
        }
    }
    

    public partial class MainPage : ContentPage
    {
        Waluta w;
        public MainPage()
        {
            InitializeComponent();
            w = new Waluta("eur");
        }
        private void OnSprzedajeClicked(object sender, EventArgs e)
        {
            string t = "";
            if (kwotaEnt.Text != null) t = kwotaEnt.Text.ToString();
            if (t.Contains(".")) t = t.Replace(".", ",");
            double kwotaZr = (double.TryParse(t, out double number)) ? kwotaZr = double.Parse(kwotaEnt.Text) : kwotaZr = 0;
            if (kwotaZr < 0) kwotaZr = 0;
            double kwotaWy = Math.Round(kwotaZr * w.skup,2);
            otrzymaszLbl.Text = kwotaWy.ToString();
            OtLbl.Text = "Otrzymasz: ";
            SemanticScreenReader.Announce(OtLbl.Text);
            SemanticScreenReader.Announce(otrzymaszLbl.Text);
        }
        private void OnKupujeClicked(object sender, EventArgs e)
        {
            string t = "";
            if (kwotaEnt.Text != null) t = kwotaEnt.Text.ToString();
            if (t.Contains(".")) t = t.Replace(".", ",");
            double kwotaZr = (double.TryParse(t, out double number)) ? kwotaZr = double.Parse(kwotaEnt.Text) : kwotaZr = 0;
            if (kwotaZr < 0) kwotaZr = 0;
            double kwotaWy = Math.Round(kwotaZr * w.sprzedaz,2);
            otrzymaszLbl.Text = kwotaWy.ToString();
            OtLbl.Text = "Zapłacisz: ";
            SemanticScreenReader.Announce(OtLbl.Text);
            SemanticScreenReader.Announce(otrzymaszLbl.Text);
        }
        private void OnEuroClicked(object sender, EventArgs e)
        {
            euroNaPlnBtn.IsEnabled = false;
            plnNaEuroBtn.IsEnabled = true;
            Ramzamzam.Text = "";
            SemanticScreenReader.Announce(Ramzamzam.Text);
        }

        private void OnPlnClicked(object sender, EventArgs e)
        {
            plnNaEuroBtn.IsEnabled = false;
            euroNaPlnBtn.IsEnabled = true;
            Ramzamzam.Text = w.waluta;
            SemanticScreenReader.Announce(Ramzamzam.Text);
        }

        private void OnPrzeliczClicked(object sender, EventArgs e)
        {
            string t = "";
            if (kwotaEnt.Text != null) t = kwotaEnt.Text.ToString();
            if (t.Contains(".")) t = t.Replace(".", ",");
            double kwotaZr = (double.TryParse(t,out double number)) ? kwotaZr = double.Parse(kwotaEnt.Text) : kwotaZr = 0;
            if (kwotaZr < 0) kwotaZr = 0;
            if (euroNaPlnBtn.IsEnabled==true)
                otrzymaszLbl.Text = ( Math.Round(kwotaZr * w.skup,2)).ToString() + "PLN";
            else
                otrzymaszLbl.Text = (Math.Round(kwotaZr / w.skup,2)).ToString() + "€";
            OtLbl.Text = "Otrzymasz: ";
            SemanticScreenReader.Announce(OtLbl.Text);
            SemanticScreenReader.Announce(otrzymaszLbl.Text);
            //PickerTitle.Text = picker.ind
            SemanticScreenReader.Announce(PickerTitle.Text);
        }
    }
}