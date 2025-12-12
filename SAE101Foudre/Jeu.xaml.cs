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
using System.Windows.Threading;

namespace SAE101Foudre
{
    /// <summary>
    /// Logique d'interaction pour Jeu.xaml
    /// </summary>
    public partial class Jeu : UserControl
    {
        // -----------------------------------------Images----------------------------------------------------

        public static BitmapImage imgPerosD = new BitmapImage(new Uri("pack://application:,,,/Images/imgPerso0.png"));
        public static BitmapImage imgPersoG = new BitmapImage(new Uri("pack://application:,,,/Images/imgPerso1.png"));

        public static BitmapImage imgEclair = new BitmapImage(new Uri("pack://application:,,,/Images/imgEclair0.png"));

        // -----------------------------------------Variables de deplacement---------------------------------------------

        public static bool DROITE = false;
        public static bool GAUCHE = false;
        public static int VITESSE_PERSO = 10;

        public static bool auSol = true;
        public static int vitesseVerticale = 0; //vitesse actuelle du personnage
        public static int gravite = 1; //vitesse du personnage qui tombe (1 = lente, 5 = rapide)
        public static int hauteurSaut = -23; //(-10 = tres bas, -50 = tres haut)
        public static int niveauSol = 810;

        // ---------------------------------------------------------------------------------------------------------

        public Jeu()
        {
            InitializeComponent();
            Minuterie();
        }

        // -------------------------------------------Boucle de jeu----------------------------------------------------

        private void GameLoop(object? sender, EventArgs e)
        {
            DeplacerJoueur();
            AppliquerGravite();
            CreerEclair();
            //DeplacerEclair();
        }

        // -----------------------------------------Deplacement du personnage---------------------------------------------

        private void DeplacerJoueur()
        {
            if (GAUCHE && Canvas.GetLeft(imgPerso) > 0)
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) - VITESSE_PERSO);
            if (DROITE && Canvas.GetLeft(imgPerso) + imgPerso.ActualWidth < canvasJeu.ActualWidth)
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) + VITESSE_PERSO);
        }

        private void AppliquerGravite()
        {
            double topPerso = Canvas.GetTop(imgPerso);
            Canvas.SetTop(imgPerso, topPerso + vitesseVerticale);

            if (auSol == false)
            {
                vitesseVerticale = vitesseVerticale + gravite;
            }

            if (Canvas.GetTop(imgPerso) + imgPerso.ActualHeight >= niveauSol)
            {
                Canvas.SetTop(imgPerso, niveauSol - imgPerso.ActualHeight);
                vitesseVerticale = 0;
                auSol = true;
            }
            else
            {
                auSol = false;
            }
        }

        private void UCJeu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D)
            {
                DROITE = true;
                GAUCHE = false;
                imgPerso.Source = imgPerosD;
            }
            else if (e.Key == Key.Q)
            {
                GAUCHE = true;
                DROITE = false;
                imgPerso.Source = imgPersoG;
            }
            if (e.Key == Key.Space && auSol == true)
            {
                vitesseVerticale = hauteurSaut;
                auSol = false;
            }
        }

        private void UCJeu_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D)
            {
                DROITE = false;
            }
            else if (e.Key == Key.Q)
            {
                GAUCHE = false;
            }
        }

        // -----------------------------------------Création Obstacles------------------------------------------------------------

        private void CreerEclair()
        {
            Random rand = new Random();
            Image nouvelEclair = new Image
            {
                Width = 100,
                Height = 200,
                Source = imgEclair
            };
            canvasJeu.Children.Add(nouvelEclair);
            Canvas.SetLeft(nouvelEclair, rand.Next(0,(int)canvasJeu.ActualWidth) - imgEclair.Width);
            Canvas.SetTop(nouvelEclair, 0 - nouvelEclair.Height);
        }

        private void DeplacerEclair(Image eclair)
        {
            double topEclair = Canvas.GetTop(eclair);
            Canvas.SetTop(eclair, topEclair + 5);
        }

        // ---------------------------------------------------------------------------------------------------------

        private void Minuterie()
        {
            DispatcherTimer gameTimer = new DispatcherTimer();
            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(8.33);
            gameTimer.Start();
        }

        private void UCJeu_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown += UCJeu_KeyDown;
            Application.Current.MainWindow.KeyUp += UCJeu_KeyUp;
        }
    }
}
