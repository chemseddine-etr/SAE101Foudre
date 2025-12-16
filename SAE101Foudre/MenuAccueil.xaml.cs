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
    /// Logique d'interaction pour MenuAccueil.xaml
    /// </summary>
    public partial class MenuAccueil : UserControl
    {
        // mode facile : 75 9 170 12
        // mode moyen : 65 12 140 15
        // mode difficile : 50 15 115 15

        public static int frequenceEclair = MenuOptions.frequenceEclair; //fréquence d'apparition des éclairs (5 = beaucoup)
        public static int vitesseEclair = MenuOptions.vitesseEclair;
        public static int frequenceBoule = MenuOptions.frequenceBoule; //fréquence d'apparition des boules (5 = beaucoup)
        public static int vitesseBoule = MenuOptions.vitesseBoule;
        public static double volumeAudio = MenuOptions.VolumeValeur;

        public MenuAccueil()
        {
            InitializeComponent();
        }

        private void butJouerAcceuil_Click(object sender, RoutedEventArgs e)
        {
            MettreAJourDifficulte();
            MettreAJourVolume();
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new Jeu());
        }

        private void butOptionsAcceuil_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new MenuOptions());
        }

        private void butQuitterAcceuil_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MettreAJourDifficulte()
        {
            frequenceEclair = MenuOptions.frequenceEclair; //fréquence d'apparition des éclairs (5 = beaucoup)
            vitesseEclair = MenuOptions.vitesseEclair;
            frequenceBoule = MenuOptions.frequenceBoule; //fréquence d'apparition des boules (5 = beaucoup)
            vitesseBoule = MenuOptions.vitesseBoule;
        }

        private void MettreAJourVolume()
        {
            volumeAudio = MenuOptions.VolumeValeur;
        }

    }
}
