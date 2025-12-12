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
        public MenuOptions()
        {
            InitializeComponent();
        }

        private void butRetourOptions_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).NaviguerVers(new MenuAccueil());
        }

        private void butDifficileOptions_Click(object sender, RoutedEventArgs e)
        {

        }

        private void butMoyenOptions_Click(object sender, RoutedEventArgs e)
        {

        }

        private void butFacileOptions_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
