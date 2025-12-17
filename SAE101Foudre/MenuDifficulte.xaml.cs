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
    /// Logique d'interaction pour MenuDifficulte.xaml
    /// </summary>
    public partial class MenuDifficulte : UserControl
    {
        public static int frequenceEclair;
        public static int vitesseEclair;
        public static int frequenceBoule;
        public static int vitesseBoule;

        public MenuDifficulte()
        {
            InitializeComponent();
        }

        private void butFacile_Click(object sender, RoutedEventArgs e)
        {
            frequenceEclair = 75;
            vitesseEclair = 9;
            frequenceBoule = 170;
            vitesseBoule = 12;
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new Jeu());
        }

        private void butMoyen_Click(object sender, RoutedEventArgs e)
        {
            frequenceEclair = 55;
            vitesseEclair = 12;
            frequenceBoule = 120;
            vitesseBoule = 15;
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new Jeu());
        }

        private void butDifficile_Click(object sender, RoutedEventArgs e)
        {
            frequenceEclair = 25;
            vitesseEclair = 20;
            frequenceBoule = 60;
            vitesseBoule = 18;
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new Jeu());
        }

        private void butRetourDiff_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new MenuAccueil());
        }
    }
}
