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
        public MenuAccueil()
        {
            InitializeComponent();
        }

        private void butQuitterAcceuil_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void butOptionsAcceuil_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void butJouerAcceuil_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
