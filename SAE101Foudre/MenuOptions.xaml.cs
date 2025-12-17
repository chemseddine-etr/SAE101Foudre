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
        public static Key toucheSauter = Key.Space;
        public static double VolumeValeur = 20.0;

        private Button boutonActif = null;

        public MenuOptions()
        {
            InitializeComponent();
            MettreAJourTextes();
        }        

        private void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VolumeValeur = Math.Round(sliderVolume.Value, 0);
            labVolumeValue.Content = $"{VolumeValeur}%";
        }

        private void MettreAJourTextes()
        {
            btnGauche.Content = "GAUCHE : " + toucheGauche.ToString();
            btnDroit.Content = "DROITE : " + toucheDroit.ToString();
            btnSaut.Content = "SAUT : " + toucheSauter.ToString();
        }

        private void BtnConfig_Click(object sender, RoutedEventArgs e)
        {
            boutonActif = (Button)sender;
            boutonActif.Content = "Appuyez sur une touche...";
            boutonActif.Focus();
        }

        private void BtnConfig_KeyDown(object sender, KeyEventArgs e)
        {

            e.Handled = true;
            Key nouvelleTouche = e.Key;

            if (boutonActif == btnGauche)
            {
                toucheGauche = nouvelleTouche;
            }
            else if (boutonActif == btnDroit)
            {
                toucheDroit = nouvelleTouche;
            }
            else if (boutonActif == btnSaut)
            {
                toucheSauter = nouvelleTouche;
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
