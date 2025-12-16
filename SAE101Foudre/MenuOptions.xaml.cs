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
        public static double VolumeValeur = 20.0;

        
        public MenuOptions()
        {
            InitializeComponent();
            labDifficulteUpdateMoyen.Visibility = Visibility.Hidden;
            labDifficulteUpdateFacile.Visibility = Visibility.Hidden;
            labDifficulteUpdateDifficile.Visibility = Visibility.Hidden;
        }

        //Visibility = Visibility.Hidden; 

        private void butRetourOptions_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new MenuAccueil());
        }

        private void butDifficileOptions_Click(object sender, RoutedEventArgs e)
        {
            frequenceEclair = 30;
            vitesseEclair = 20;
            frequenceBoule = 90;
            vitesseBoule = 18;
            labDifficulteUpdateMoyen.Visibility = Visibility.Hidden;
            labDifficulteUpdateFacile.Visibility = Visibility.Hidden;
            labDifficulteUpdateDifficile.Visibility = Visibility.Visible;
        }

        private void butMoyenOptions_Click(object sender, RoutedEventArgs e)
        {
            frequenceEclair = 65;
            vitesseEclair = 12;
            frequenceBoule = 140;
            vitesseBoule = 15;
            labDifficulteUpdateFacile.Visibility = Visibility.Hidden;
            labDifficulteUpdateDifficile.Visibility = Visibility.Hidden;
            labDifficulteUpdateMoyen.Visibility = Visibility.Visible;
        }

        private void butFacileOptions_Click(object sender, RoutedEventArgs e)
        {
            frequenceEclair = 75;
            vitesseEclair = 9;
            frequenceBoule = 170;
            vitesseBoule = 12;
            labDifficulteUpdateMoyen.Visibility = Visibility.Hidden;
            labDifficulteUpdateDifficile.Visibility = Visibility.Hidden;
            labDifficulteUpdateFacile.Visibility = Visibility.Visible;
        }

        private void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VolumeValeur = Math.Round(sliderVolume.Value, 0);
            labVolumeValue.Content = $"{VolumeValeur}%";
        }
    }
}
