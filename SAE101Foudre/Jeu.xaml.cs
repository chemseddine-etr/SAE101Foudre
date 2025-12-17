using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
using static System.Formats.Asn1.AsnWriter;

namespace SAE101Foudre
{
    /// <summary>
    /// Logique d'interaction pour Jeu.xaml
    /// </summary>
    public partial class Jeu : UserControl
    {
        // -----------------------------------------Images----------------------------------------------------

        public static List<Image> eclairs = new List<Image>();
        public static List<Image> boules = new List<Image>();
        public static List<Line> pluie = new List<Line>();

        // -----------------------------------------Variables de deplacement---------------------------------------------

        public bool droite = false;
        public bool gauche = false;
        public int vitessePerso = 10;

        public bool auSol = true;
        public static int niveauSol;
        public int vitesseVerticale = 0; //vitesse actuelle du personnage
        public static int gravite = 1; //vitesse du personnage qui tombe (1 = lente, 5 = rapide)
        public static int hauteurSaut = -23; //hauteur du saut (-10 = tres bas, -50 = tres haut)

        public int frequenceEclair = MenuDifficulte.frequenceEclair;
        public int vitesseEclair = MenuDifficulte.vitesseEclair;
        public int frequenceBoule = MenuDifficulte.frequenceBoule;
        public int vitesseBoule = MenuDifficulte.vitesseBoule;

        public static int nombreGouttes = 80;
        public static int vitessePluie = 25;

        // ---------------------------------------------------------------------------------------------------------

        DispatcherTimer gameTimer = new DispatcherTimer();

        public static Random alea = new Random();

        public static int score = 0;
        public int compteurTempsScore = 0;

        private int indexImage = 0;
        private int tempsAnim = 0;
        private int vitesseAnim = 6;

        private int indexEclair = 0;
        private int tempsAnimEclair = 0;
        private int vitesseAnimEclair = 2;

        private bool estEnPause = false;

        public Jeu()
        {
            InitializeComponent();

            eclairs.Clear();
            boules.Clear();
            pluie.Clear();
            score = 0;

            sliderPauseVolume.Value = MenuOptions.VolumeValeur;

            Minuterie();
            Audio.LancerMusiqueDeFond();
        }

        // -------------------------------------------Boucle de jeu----------------------------------------------------

        private void GameLoop(object? sender, EventArgs e)
        {
            DeplacerJoueur();
            AppliquerGravite();
            DeplacerObstacle();
            VerifierCollisions();
            AnimerEclairs();
            AnimerPluie();
            GererScore();
        }

        // -----------------------------------------Deplacement du personnage---------------------------------------------

        private void DeplacerJoueur()
        {
            if (gauche && Canvas.GetLeft(imgPerso) > 0)
            {
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) - vitessePerso);
                AnimerMarche(false);
            }


            if (droite && Canvas.GetLeft(imgPerso) + imgPerso.ActualWidth < canvasJeu.ActualWidth)
            {
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) + vitessePerso);
                AnimerMarche(true);
            }
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
            if (e.Key == Key.E)
            {
                BasculerPause();
            }

            if (estEnPause) return;

            if (e.Key == MenuOptions.toucheDroit)
            {
                droite = true;
                gauche = false;
            }
            else if (e.Key == MenuOptions.toucheGauche)
            {
                gauche = true;
                droite = false;
            }
            if (e.Key == MenuOptions.toucheSauter && auSol == true)
            {
                vitesseVerticale = hauteurSaut;
                auSol = false;
            }

            if (e.Key == Key.T)
            {
                imgFond.ImageSource = Ressources.FondTroll;
            }

        }

        private void UCJeu_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == MenuOptions.toucheDroit)
            {
                droite = false;
                imgPerso.Source = Ressources.PersoDroit;
            }
            else if (e.Key == MenuOptions.toucheGauche)
            {
                gauche = false;
                imgPerso.Source = Ressources.PersoDroit;
            }
        }

        // -----------------------------------------Gestion Pause---------------------------------------------------------

        private void BasculerPause()
        {
            if (estEnPause)
            {
                estEnPause = false;
                gridPause.Visibility = Visibility.Collapsed;
                gameTimer.Start();
            }
            else
            {
                estEnPause = true;
                gridPause.Visibility = Visibility.Visible;
                gameTimer.Stop();

                droite = false;
                gauche = false;
            }
        }

        private void ButReprendre_Click(object sender, RoutedEventArgs e)
        {
            BasculerPause();
        }

        private void ButQuitter_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Stop();
            Audio.ArreterMusique();
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new MenuAccueil());
        }

        private void SliderPauseVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (labPauseVolumeValue != null)
            {
                MenuOptions.VolumeValeur = Math.Round(sliderPauseVolume.Value, 0);
                labPauseVolumeValue.Content = $"{MenuOptions.VolumeValeur}%";

                Audio.ChangerVolume(MenuOptions.VolumeValeur);
            }
        }

        //---------------------------------------------Animations sprites---------------------------------------------------------

        private void AnimerMarche(bool versLaDroite)
        {
            tempsAnim++;
            if (tempsAnim >= vitesseAnim)
            {
                tempsAnim = 0;
                indexImage++;  
                if (indexImage >= Ressources.SpritesPerso.Length)
                {
                    indexImage = 0;
                }

                imgPerso.Source = Ressources.SpritesPerso[indexImage];
            }

            if (versLaDroite)
            {
                imgPerso.RenderTransform = new ScaleTransform(1, 1);
            }
            else
            {
                imgPerso.RenderTransformOrigin = new Point(0.5, 0.5);
                imgPerso.RenderTransform = new ScaleTransform(-1, 1);
            }
        }

        private void AnimerEclairs()
        {
            tempsAnimEclair++;
            if (tempsAnimEclair >= vitesseAnimEclair)
            {
                tempsAnimEclair = 0;
                indexEclair++;
                if (indexEclair >= Ressources.SpritesEclair.Length)
                {
                    indexEclair = 0;
                }
                foreach (Image eclair in eclairs)
                {
                    eclair.Source = Ressources.SpritesEclair[indexEclair];
                }
            }
        }

        // -----------------------------------------Création Obstacles------------------------------------------------------------

        private void CreerEclair()
        {
            Image nouvelEclair = new Image
            {
                Width = 150,
                Height = 300,
                Source = Ressources.SpritesEclair[indexEclair]
            };

            double positionX = alea.Next(0, (int)canvasJeu.ActualWidth - (int)nouvelEclair.Width);
            Canvas.SetLeft(nouvelEclair, positionX);
            Canvas.SetTop(nouvelEclair, 0 - nouvelEclair.Height);
            canvasJeu.Children.Add(nouvelEclair);
            eclairs.Add(nouvelEclair);
        }

        private void CreerBoule()
        {
            Image nouvelleBoule = new Image
            {
                Width = 100,
                Height = 200,
                Source = Ressources.Boule
            };

            Canvas.SetLeft(nouvelleBoule, 0 - nouvelleBoule.Width);
            Canvas.SetTop(nouvelleBoule, Canvas.GetTop(imgPerso) - 30);

            canvasJeu.Children.Add(nouvelleBoule);

            boules.Add(nouvelleBoule);
        }

        private void DeplacerObstacle()
        {
            if (alea.Next(0, frequenceEclair) == 0)
            {
                CreerEclair();
                Audio.JouerSon("eclair.wav", MenuOptions.VolumeValeur/250);
            }
            if (alea.Next(0, frequenceBoule) == 0)
            {
                CreerBoule();
            }

            for (int i = eclairs.Count - 1; i >= 0; i--)
            {
                Image eclairActuel = eclairs[i];

                double topActuel = Canvas.GetTop(eclairActuel);
                Canvas.SetTop(eclairActuel, topActuel + vitesseEclair);

                if (topActuel > canvasJeu.ActualHeight)
                {
                    canvasJeu.Children.Remove(eclairActuel);
                    eclairs.RemoveAt(i);
                }
            }

            for (int i = boules.Count - 1; i >= 0; i--)
            {
                Image bouleActuel = boules[i];

                double leftActuel = Canvas.GetLeft(bouleActuel);
                Canvas.SetLeft(bouleActuel, leftActuel + vitesseBoule);

                if (leftActuel > canvasJeu.ActualWidth)
                {
                    canvasJeu.Children.Remove(bouleActuel);
                    boules.RemoveAt(i);
                }
            }
        }
        private void VerifierCollisions()
        {
            int valHit = 50;
            Rect boxJoueur = new Rect(Canvas.GetLeft(imgPerso) + valHit, Canvas.GetTop(imgPerso) + valHit, imgPerso.ActualWidth - valHit, imgPerso.ActualHeight - valHit);
            foreach (Image eclair in eclairs)
            {
                Rect boxEclair = new Rect(Canvas.GetLeft(eclair) + valHit, Canvas.GetTop(eclair) + valHit, eclair.Width - valHit, eclair.Height - valHit);
                if (boxJoueur.IntersectsWith(boxEclair))
                {
                    FinDuJeu();
                    return;
                }
            }
            foreach (Image boule in boules)
            {
                Rect boxBoule = new Rect(Canvas.GetLeft(boule) + valHit, Canvas.GetTop(boule) + valHit, boule.Width - valHit, boule.Height - valHit);
                if (boxJoueur.IntersectsWith(boxBoule))
                {
                    FinDuJeu();
                    return;
                }
            }
        }

        // ---------------------------------------------------Autres Méthodes---------------------------------------------

        private void CreerPluie()
        {
            for (int i = 0; i < nombreGouttes; i++)
            {
                Line goutte = new Line();

                goutte.Stroke = Brushes.LightBlue;
                goutte.StrokeThickness = 2;
                goutte.X1 = 0;
                goutte.Y1 = 0;
                goutte.X2 = 0;
                goutte.Y2 = 15;
                goutte.Opacity = 0.6;

                double posX = alea.Next(0, (int)SystemParameters.PrimaryScreenWidth);
                double posY = alea.Next(0, (int)SystemParameters.PrimaryScreenHeight);

                Canvas.SetLeft(goutte, posX);
                Canvas.SetTop(goutte, posY);

                canvasJeu.Children.Add(goutte);
                pluie.Add(goutte);
            }
        }

        private void AnimerPluie()
        {
            foreach (Line goutte in pluie)
            {
                double topActuel = Canvas.GetTop(goutte);
                Canvas.SetTop(goutte, topActuel + vitessePluie);
                Canvas.SetLeft(goutte, Canvas.GetLeft(goutte) - 2);

                if (topActuel > niveauSol + goutte.ActualHeight)
                {
                    Canvas.SetTop(goutte, 0 - goutte.ActualHeight);

                    double nouveauX = alea.Next(0, (int)canvasJeu.ActualWidth);
                    Canvas.SetLeft(goutte, nouveauX);
                }
            }
        }

        private void Minuterie()
        {
            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(16);
            gameTimer.Start();
        }

        private void GererScore()
        {
            compteurTempsScore++;
            if (compteurTempsScore >= 60)
            {
                score = score + 10;
                txtScore.Text = "Score : " + score;
                compteurTempsScore = 0;
            }
        }

        private void FinDuJeu()
        {
            gameTimer.Stop();
            gameTimer.Tick -= GameLoop;
            Audio.ArreterMusique();
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new GameOverUC());
        }

        private void UCJeu_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown += UCJeu_KeyDown;
            Application.Current.MainWindow.KeyUp += UCJeu_KeyUp;

            CreerPluie();

            niveauSol = (int)(canvasJeu.ActualHeight * 0.90);
            Canvas.SetTop(imgPerso, niveauSol - imgPerso.Height);
            Canvas.SetLeft(imgPerso, canvasJeu.ActualWidth * 0.50);
        }

        private void UCJeu_Unloaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown -= UCJeu_KeyDown;
            Application.Current.MainWindow.KeyUp -= UCJeu_KeyUp;
        }
    }
}
