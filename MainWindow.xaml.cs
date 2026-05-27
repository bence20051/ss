using InformatikaiEszkozok_LIB;
using InformatikaiEszkozok_LIB.MODEL;
using System.Windows;
using System.Windows.Controls;

namespace InformatikaiEszkozok_WPF
{
    /// <summary>
    /// A termékek karbantartására szolgáló ablak kódja (code-behind).
    /// 
    /// A felhasználói interakciókat (kattintás, kiválasztás) itt kezeljük.
    /// Az adatkezelést a Worker osztály végzi az API-n keresztül.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Az aktuálisan betöltött termékek lokális másolata.
        // Ezt mutatjuk a ListBoxban, és innen módosítjuk.
        private List<Termek> termekek = new List<Termek>();

        public MainWindow()
        {
            InitializeComponent();
        }

        // =====================================================
        // ABLAK BETÖLTŐDIK: TERMÉKEK LEKÉRÉSE
        // =====================================================
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await TermekekFrissites();
        }

        /// <summary>
        /// Lekéri a termékeket az API-tól és kiteszi a ListBox-ba.
        /// Több helyről hívjuk, ezért külön metódus.
        /// </summary>
        private async Task TermekekFrissites()
        {
            termekek = await Worker.Termekek();

            lbTermekek.ItemsSource = termekek;
            lbTermekek.DisplayMemberPath = "Nev";
        }

        // =====================================================
        // LISTÁBÓL KIVÁLASZTÁS -> ADATOK A MEZŐKBE
        // =====================================================
        private void lbTermekek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbTermekek.SelectedItem is not Termek t)
            {
                return;
            }

            txtNev.Text         = t.Nev;
            txtLeiras.Text      = t.Leiras;
            txtEgysegar.Text    = t.Egysegar.ToString();
            txtKeszlet.Text     = t.Keszlet.ToString();
            txtElerheto.Text    = t.Elerheto.ToString();
            txtKepFajlnev.Text  = t.KepFajlnev;
        }

        // =====================================================
        // MÓDOSÍTÁS GOMB
        // =====================================================
        private async void btnModositas_Click(object sender, RoutedEventArgs e)
        {
            if (lbTermekek.SelectedItem is not Termek termek)
            {
                MessageBox.Show("Nincs kiválasztott termék!");
                return;
            }

            if (!MezokErvenyesek(out int egysegar, out int keszlet, out bool elerheto))
            {
                return;
            }

            termek.Nev        = txtNev.Text;
            termek.Leiras     = txtLeiras.Text;
            termek.Egysegar   = egysegar;
            termek.Keszlet    = keszlet;
            termek.Elerheto   = elerheto;
            termek.KepFajlnev = txtKepFajlnev.Text;

            bool siker = await Worker.TermekModositas(termek);

            MessageBox.Show(siker
                ? "A módosítás sikeres."
                : "A módosítás nem sikerült.");

            if (siker)
            {
                lbTermekek.Items.Refresh();
                MezoTorles();
            }
        }

        // =====================================================
        // TÖRLÉS GOMB
        // =====================================================
        private async void btnTorles_Click(object sender, RoutedEventArgs e)
        {
            if (lbTermekek.SelectedItem is not Termek termek)
            {
                MessageBox.Show("Nincs kiválasztott termék!");
                return;
            }

            var valasz = MessageBox.Show(
                $"Biztosan törli ezt a terméket: {termek.Nev}?",
                "Megerősítés",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (valasz != MessageBoxResult.Yes)
            {
                return;
            }

            bool siker = await Worker.TermekTorles(termek.Id);

            if (siker)
            {
                termekek.Remove(termek);
                lbTermekek.Items.Refresh();
                MezoTorles();
                MessageBox.Show("A törlés sikeres.");
            }
            else
            {
                MessageBox.Show("A törlés nem sikerült. " +
                    "Lehet, hogy van hozzá tartozó rendelés.");
            }
        }

        // =====================================================
        // ÚJ FELVITEL GOMB
        // =====================================================
        private async void btnUj_Click(object sender, RoutedEventArgs e)
        {
            if (!MezokErvenyesek(out int egysegar, out int keszlet, out bool elerheto))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNev.Text))
            {
                MessageBox.Show("Adjon meg termék nevet!");
                return;
            }

            var ujTermek = new Termek
            {
                Nev        = txtNev.Text,
                Leiras     = txtLeiras.Text,
                Egysegar   = egysegar,
                Keszlet    = keszlet,
                Elerheto   = elerheto,
                KepFajlnev = txtKepFajlnev.Text
            };

            bool siker = await Worker.TermekFelvitel(ujTermek);

            MessageBox.Show(siker
                ? "Az új termék mentése sikeres."
                : "Az új termék mentése nem sikerült.");

            if (siker)
            {
                await TermekekFrissites();
                MezoTorles();
            }
        }

        // =====================================================
        // SEGÉDMETÓDUSOK
        // =====================================================

        /// <summary>
        /// Ellenőrzi és kiparsolja az egységár, készlet és elérhető mezőket.
        /// Ha hibás, üzenetet ír és false-t ad vissza.
        /// </summary>
        private bool MezokErvenyesek(out int egysegar, out int keszlet, out bool elerheto)
        {
            egysegar = 0;
            keszlet  = 0;
            elerheto = false;

            if (!int.TryParse(txtEgysegar.Text, out egysegar))
            {
                MessageBox.Show("Az egységár csak egész szám lehet!");
                return false;
            }
            if (!int.TryParse(txtKeszlet.Text, out keszlet))
            {
                MessageBox.Show("A készlet csak egész szám lehet!");
                return false;
            }
            if (!bool.TryParse(txtElerheto.Text, out elerheto))
            {
                MessageBox.Show("Az elérhető mező csak 'true' vagy 'false' lehet!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Üríti a beviteli mezőket.
        /// </summary>
        private void MezoTorles()
        {
            txtNev.Clear();
            txtLeiras.Clear();
            txtEgysegar.Clear();
            txtKeszlet.Clear();
            txtElerheto.Clear();
            txtKepFajlnev.Clear();
        }

        // =====================================================
        // KILÉPÉS GOMB
        // =====================================================
        private void btnKilepes_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
