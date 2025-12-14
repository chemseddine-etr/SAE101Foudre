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
    /// Logique d'interaction pour GameOverUC.xaml
    /// </summary>
    public partial class GameOverUC : UserControl
    {
        public GameOverUC()
        {
            InitializeComponent();
        }

        private void butQuitterGameOver_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void butMenuGameOver_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new MenuAccueil());
        }
    }
}
