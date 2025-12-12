using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OuvrirJeu(new MenuAccueil());
        }

        public void OuvrirJeu(UserControl jeu)
        {
            this.Content = jeu;
        }

        public void NaviguerVers(UserControl fenetre)
        {
            this.Content = fenetre;
        }
    }
}