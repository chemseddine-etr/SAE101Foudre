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
        public static double volumeAudio = MenuOptions.VolumeValeur;

        public MenuAccueil()
        {
            InitializeComponent();
        }

        private void butJouerAcceuil_Click(object sender, RoutedEventArgs e)
        {
            MettreAJourVolume();
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new MenuDifficulte());
        }

        private void butOptionsAcceuil_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new MenuOptions());
        }

        private void butQuitterAcceuil_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MettreAJourVolume()
        {
            volumeAudio = MenuOptions.VolumeValeur;
        }

    }
}
