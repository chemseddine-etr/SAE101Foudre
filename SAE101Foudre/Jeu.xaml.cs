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
        public static BitmapImage imgBoule = new BitmapImage(new Uri("pack://application:,,,/Images/imgBoule.png"));

        public static BitmapImage imgTroll = new BitmapImage(new Uri("pack://application:,,,/Images/imgFondTroll.png"));

        List<Image> eclairs = new List<Image>();
        List<Image> boules = new List<Image>();

        List<Line> pluie = new List<Line>();
        int nombreGouttes = 80;
        int vitessePluie = 25;

        // -----------------------------------------Variables de deplacement---------------------------------------------

        public static bool droite = false;
        public static bool gauche = false;
        public static int vitessePerso = 10;

        public static bool auSol = true;
        public static int niveauSol = 810;
        public static int vitesseVerticale = 0; //vitesse actuelle du personnage
        public static int gravite = 1; //vitesse du personnage qui tombe (1 = lente, 5 = rapide)
        public static int hauteurSaut = -23; //hauteur du saut (-10 = tres bas, -50 = tres haut)

        public static int frequenceEclair = 50; //fréquence d'apparition des éclairs (5 = beaucoup)
        public static int vitesseEclair = 15;
        public static int frequenceBoule = 165; //fréquence d'apparition des boules (5 = beaucoup)
        public static int vitesseBoule = 15;

        // mode difficile : 
        // mode hardocre : 60 15 165 15

        // ---------------------------------------------------------------------------------------------------------

        DispatcherTimer gameTimer = new DispatcherTimer();
        Random alea = new Random();

        MediaPlayer musique = new MediaPlayer();
        MediaPlayer musique2 = new MediaPlayer();

        public Jeu()
        {
            InitializeComponent();
            Minuterie();
            Musique(musique, "Sons/tonerre.wav");
            Musique(musique2, "Sons/musique.wav");
        }

        // -------------------------------------------Boucle de jeu----------------------------------------------------

        private void GameLoop(object? sender, EventArgs e)
        {
            DeplacerJoueur();
            AppliquerGravite();
            DeplacerObstacle();
            VerifierCollisions();
            AnimerPluie();
        }

        // -----------------------------------------Deplacement du personnage---------------------------------------------

        private void DeplacerJoueur()
        {
            if (gauche && Canvas.GetLeft(imgPerso) > 0)
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) - vitessePerso);
            if (droite && Canvas.GetLeft(imgPerso) + imgPerso.ActualWidth < canvasJeu.ActualWidth)
                Canvas.SetLeft(imgPerso, Canvas.GetLeft(imgPerso) + vitessePerso);
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
                droite = true;
                gauche = false;
                imgPerso.Source = imgPerosD;
            }
            else if (e.Key == Key.Q)
            {
                gauche = true;
                droite = false;
                imgPerso.Source = imgPersoG;
            }
            if (e.Key == Key.Space && auSol == true)
            {
                vitesseVerticale = hauteurSaut;
                auSol = false;
            }

            if (e.Key == Key.T)
            {
                var uriSource = new Uri("pack://application:,,,/Images/imgFondTroll.png");
                imgFond.ImageSource = new BitmapImage(uriSource);
            }

        }

        private void UCJeu_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D)
            {
                droite = false;
            }
            else if (e.Key == Key.Q)
            {
                gauche = false;
            }
        }

        // -----------------------------------------Création Obstacles------------------------------------------------------------

        private void CreerEclair()
        {
            Image nouvelEclair = new Image { Width = 150, Height = 300, Source = imgEclair };

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
                Source = imgBoule
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
                JouerSon("eclair.wav", 0.2);
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
            int valHit = 25;
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

        private void FinDuJeu()
        {
            gameTimer.Stop();
            musique.Stop();
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new GameOverUC());
        }


        // ---------------------------------------------------------------------------------------------------------

        private void CreerPluie()
        {
            for (int i = 0; i < nombreGouttes; i++)
            {
                Line goutte = new Line();

                // --- STYLE DE LA GOUTTE ---
                goutte.Stroke = Brushes.LightBlue; // Couleur
                goutte.StrokeThickness = 2;        // Epaisseur
                goutte.X1 = 0;
                goutte.Y1 = 0;
                goutte.X2 = 0;
                goutte.Y2 = 15; // La goutte fait 15 pixels de long
                goutte.Opacity = 0.6; // Un peu transparente

                // --- POSITION DE DEPART ---
                // On les place aléatoirement partout sur l'écran (X et Y)
                // Comme ça, dès le début du jeu, il pleut déjà partout
                double posX = alea.Next(0, (int)SystemParameters.PrimaryScreenWidth); // Largeur max de l'écran
                double posY = alea.Next(0, (int)SystemParameters.PrimaryScreenHeight);

                Canvas.SetLeft(goutte, posX);
                Canvas.SetTop(goutte, posY);

                // Ajout au jeu et à la liste
                canvasJeu.Children.Add(goutte);
                pluie.Add(goutte);
            }
        }

        private void AnimerPluie()
        {
            foreach (Line goutte in pluie)
            {
                // 1. On fait descendre la goutte
                double topActuel = Canvas.GetTop(goutte);
                Canvas.SetTop(goutte, topActuel + vitessePluie);

                // 2. Si elle dépasse le bas de l'écran (niveauSol ou un peu plus bas)
                if (topActuel > niveauSol + 50)
                {
                    // LE RECYCLAGE : On la renvoie tout en haut
                    Canvas.SetTop(goutte, -20); // Juste au dessus de l'écran

                    // Et à une nouvelle position X aléatoire
                    double nouveauX = alea.Next(0, (int)canvasJeu.ActualWidth);
                    Canvas.SetLeft(goutte, nouveauX);
                }
            }
        }

        private void Minuterie()
        {
            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(8.33);
            gameTimer.Start();
        }

        public void Musique(MediaPlayer mp, string son)
        {
            mp.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + son));
            mp.MediaEnded += RelancerMusique;
            mp.Play();
        }

        private void RelancerMusique(object sender, EventArgs e)
        {
            musique.Position = TimeSpan.Zero;
            musique.Play();
        }

        private void JouerSon(string nomFichier, double volume)
        {
            MediaPlayer son = new MediaPlayer();
            son.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Sons/" + nomFichier));
            son.Volume = volume;
            son.Play();
        }

        private void UCJeu_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown += UCJeu_KeyDown;
            Application.Current.MainWindow.KeyUp += UCJeu_KeyUp;
            CreerPluie();
        }
    }
}
