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
        public static int frequenceEclair = 65; //fréquence d'apparition des éclairs (5 = beaucoup)
        public static int vitesseEclair = 12;
        public static int frequenceBoule = 140; //fréquence d'apparition des boules (5 = beaucoup)
        public static int vitesseBoule = 15;

        // mode facile : 75 9 170 12
        // mode moyen : 65 12 140 15
        // mode difficile : 50 15 115 15
        public MenuOptions()
        {
            InitializeComponent();

        }

        private void butRetourOptions_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new MenuAccueil());
        }

        private void butDifficileOptions_Click(object sender, RoutedEventArgs e)
        {
            frequenceEclair = 50;
            vitesseEclair = 9;
            frequenceBoule = 115;
            vitesseBoule = 15;
        }

        private void butMoyenOptions_Click(object sender, RoutedEventArgs e)
        {
            frequenceEclair = 65;
            vitesseEclair = 12;
            frequenceBoule = 140;
            vitesseBoule = 15;
        }

        private void butFacileOptions_Click(object sender, RoutedEventArgs e)
        {
            frequenceEclair = 50;
            vitesseEclair = 9;
            frequenceBoule = 115;
            vitesseBoule = 15;
        }
    }
}
