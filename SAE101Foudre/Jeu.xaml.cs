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

        public static BitmapImage imgPerosD = new BitmapImage(new Uri("pack://application:,,,/Images/imgPerso0.png"));
        public static BitmapImage imgPerosD1 = new BitmapImage(new Uri("pack://application:,,,/Images/imgPerso0.1.png"));
        public static BitmapImage imgPerosD2 = new BitmapImage(new Uri("pack://application:,,,/Images/imgPerso0.2.png"));
        public static BitmapImage imgPerosD3 = new BitmapImage(new Uri("pack://application:,,,/Images/imgPerso0.3.png"));
        public static BitmapImage imgPerosD4 = new BitmapImage(new Uri("pack://application:,,,/Images/imgPerso0.4.png"));
        public static BitmapImage imgPersoG = new BitmapImage(new Uri("pack://application:,,,/Images/imgPerso1.png"));

        public static BitmapImage imgEclair0 = new BitmapImage(new Uri("pack://application:,,,/Images/imgEclair0.png"));
        public static BitmapImage imgEclair1 = new BitmapImage(new Uri("pack://application:,,,/Images/imgEclair1.png"));
        public static BitmapImage imgEclair2 = new BitmapImage(new Uri("pack://application:,,,/Images/imgEclair2.png"));
        public static BitmapImage imgEclair3 = new BitmapImage(new Uri("pack://application:,,,/Images/imgEclair3.png"));
        public static BitmapImage imgBoule = new BitmapImage(new Uri("pack://application:,,,/Images/imgBoule.png"));

        public static BitmapImage imgTroll = new BitmapImage(new Uri("pack://application:,,,/Images/imgFondTroll.png"));

        public static List<Image> eclairs = new List<Image>();
        public static List<Image> boules = new List<Image>();

        public static List<Line> pluie = new List<Line>();
        public static int nombreGouttes = 80;
        public static int vitessePluie = 25;

        // -----------------------------------------Variables de deplacement---------------------------------------------

        public bool droite = false;
        public bool gauche = false;
        public int vitessePerso = 10;

        public bool auSol = true;
        public static int niveauSol = 950;
        public int vitesseVerticale = 0; //vitesse actuelle du personnage
        public static int gravite = 1; //vitesse du personnage qui tombe (1 = lente, 5 = rapide)
        public static int hauteurSaut = -23; //hauteur du saut (-10 = tres bas, -50 = tres haut)

        public int frequenceEclair = MenuAccueil.frequenceEclair;
        public int vitesseEclair = MenuAccueil.vitesseEclair;
        public int frequenceBoule = MenuAccueil.frequenceBoule;
        public int vitesseBoule = MenuAccueil.vitesseBoule;

        
        // ---------------------------------------------------------------------------------------------------------

        public static DispatcherTimer gameTimer = new DispatcherTimer();

        public static Random alea = new Random();

        public static MediaPlayer musique = new MediaPlayer();
        public static MediaPlayer musique2 = new MediaPlayer();

        public static int score = 0;
        public int compteurTempsScore = 0;

        public Jeu()
        {
            InitializeComponent();

            eclairs.Clear(); // On vide la liste des éclairs précédents
            boules.Clear();  // On vide la liste des boules précédentes
            pluie.Clear();   // On vide la pluie (sinon ça va ramer de plus en plus)
            score = 0;       // On remet le score à 0

            animationMarche = new BitmapImage[] { imgPerosD1, imgPerosD2, imgPerosD3, imgPerosD4 };
            animationEclair = new BitmapImage[] { imgEclair0, imgEclair1, imgEclair2, imgEclair3 };



            Minuterie();
            Musique(musique, "Sons/tonerre.wav", MenuOptions.VolumeValeur / 100);
            Musique(musique2, "Sons/musique.wav", MenuOptions.VolumeValeur / 100);
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
            if (e.Key == Key.D)
            {
                droite = true;
                gauche = false;
                
            }
            else if (e.Key == Key.Q)
            {
                gauche = true;
                droite = false;
                
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

        // -------------------- Animation du Personnage --------------------
        // Tableau pour stocker les images de la course
        private BitmapImage[] animationMarche;

        // Quel numéro d'image affiche-t-on ? (0, 1, 2 ou 3)
        private int indexImage = 0;

        // Compteur pour ralentir l'animation
        private int tempsAnim = 0;

        // Vitesse de l'animation (plus c'est bas, plus il court vite). 
        // 10 signifie "change d'image toutes les 10 frames du jeu"
        private int vitesseAnim = 8;

        private void AnimerMarche(bool versLaDroite)
        {
            // 1. On incrémente le compteur de temps
            tempsAnim++;

            // 2. Si le temps dépasse la vitesse définie, on change d'image
            if (tempsAnim >= vitesseAnim)
            {
                tempsAnim = 0; // Reset du timer
                indexImage++;  // Image suivante

                // Si on dépasse la dernière image, on revient à la première (0)
                if (indexImage >= animationMarche.Length)
                {
                    indexImage = 0;
                }

                // 3. On applique la nouvelle image au contrôle XAML
                imgPerso.Source = animationMarche[indexImage];
            }

            // 4. Gestion du miroir (Gauche/Droite)
            if (versLaDroite)
            {
                // Pas de transformation (image normale)
                imgPerso.RenderTransform = new ScaleTransform(1, 1);
            }
            else
            {
                // Effet miroir horizontal (flip)
                // L'origine de la transformation doit être au centre de l'image pour qu'il ne se décale pas
                imgPerso.RenderTransformOrigin = new Point(0.5, 0.5);
                imgPerso.RenderTransform = new ScaleTransform(-1, 1);
            }
        }


        private BitmapImage[] animationEclair;
        private int indexEclair = 0;
        private int tempsAnimEclair = 0;
        private int vitesseAnimEclair = 4;


        private void AnimerEclairs()
        {
            // 1. Incrémenter le temps
            tempsAnimEclair++;

            // 2. Si le temps est écoulé, on change de frame
            if (tempsAnimEclair >= vitesseAnimEclair)
            {
                tempsAnimEclair = 0;
                indexEclair++;

                // Boucler l'animation (0 -> 1 -> 2 -> 3 -> 0 ...)
                if (indexEclair >= animationEclair.Length)
                {
                    indexEclair = 0;
                }

                // 3. Mettre à jour TOUS les éclairs existants dans la liste
                foreach (Image eclair in eclairs)
                {
                    eclair.Source = animationEclair[indexEclair];
                }
            }
        }

        // -----------------------------------------Création Obstacles------------------------------------------------------------

        private void CreerEclair()
        {
            // MODIFICATION ICI : On utilise l'image actuelle de l'animation
            Image nouvelEclair = new Image
            {
                Width = 150,
                Height = 300,
                Source = animationEclair[indexEclair]
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
                JouerSon("eclair.wav", MenuOptions.VolumeValeur/100);
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
            int valHit = 40;
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
                Canvas.SetLeft(goutte, Canvas.GetLeft(goutte) - 2);

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

        public void Musique(MediaPlayer mp, string son, double volume)
        {
            mp.Volume = volume;
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

        private void GererScore()
        {
            compteurTempsScore++;
            if (compteurTempsScore >= 120)
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
            musique.Stop();
            musique2.Stop();
            ((MainWindow)Application.Current.MainWindow).OuvrirUC(new GameOverUC());
        }

        private void UCJeu_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown += UCJeu_KeyDown;
            Application.Current.MainWindow.KeyUp += UCJeu_KeyUp;
            CreerPluie();
        }
    }
}
