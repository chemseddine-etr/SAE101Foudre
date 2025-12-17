using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAE101Foudre
{
    /// <summary>
    /// Logique d'interaction pour MenuOptions.xaml
    /// </summary>
    public partial class MenuOptions : UserControl
    {
        public static Key toucheDroit = Key.D;
        public static Key toucheGauche = Key.Q;
        public static Key toucheSaut = Key.Space;
        public static double VolumeValeur = 20.0;

        private Button boutonActif = null;

        public MenuOptions()
        {
            InitializeComponent();
            MettreAJourTextes();
        }

        private void sliderVolume_ValueChanged(object sender, RoutedEventArgs e)
        {
            VolumeValeur = Math.Round(sliderVolume.Value, 0);
            labVolumeValue.Content = $"{VolumeValeur}%";
        }

        private void MettreAJourTextes()
        {
            butGauche.Content = "GAUCHE : " + toucheGauche.ToString();
            butDroit.Content = "DROITE : " + toucheDroit.ToString();
            butSaut.Content = "SAUT : " + toucheSaut.ToString();
        }

        private void ButConfig_Click(object sender, RoutedEventArgs e)
        {
            boutonActif = (Button)sender;
            boutonActif.Content = "Appuyez sur une touche...";
            boutonActif.Focus();
        }

        private void ButConfig_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            Key nouvelleTouche = e.Key;

            if (boutonActif == butGauche)
            {
                toucheGauche = nouvelleTouche;
            }
            else if (boutonActif == butDroit)
            {
                toucheDroit = nouvelleTouche;
            }
            else if (boutonActif == butSaut)
            {
                toucheSaut = nouvelleTouche;
            }

            boutonActif = null;
            MettreAJourTextes();
        }

        private void butRetourOptions_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new MenuAccueil());
        }
    }
}
