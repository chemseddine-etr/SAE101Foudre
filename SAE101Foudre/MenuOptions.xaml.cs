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
        public static double VolumeValeur = 20.0;

        
        public MenuOptions()
        {
            InitializeComponent();
            labDifficulteUpdateMoyen.Visibility = Visibility.Hidden;
            labDifficulteUpdateFacile.Visibility = Visibility.Hidden;
            labDifficulteUpdateDifficile.Visibility = Visibility.Hidden;
        }


        private void butRetourOptions_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new MenuAccueil());
        }

        private void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VolumeValeur = Math.Round(sliderVolume.Value, 0);
            labVolumeValue.Content = $"{VolumeValeur}%";
        }
    }
}
