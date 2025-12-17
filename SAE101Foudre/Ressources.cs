using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SAE101Foudre
{
    public static class Ressources
    {
        public static BitmapImage PersoDroit = ChargerImage("Images/imgPerso0.png");
        public static BitmapImage PersoGauche = ChargerImage("Images/imgPerso1.png");
        public static BitmapImage Boule = ChargerImage("Images/imgBoule.png");
        public static BitmapImage FondTroll = ChargerImage("Images/imgFondTroll.png");

        public static BitmapImage[] SpritesPerso;
        public static BitmapImage[] SpritesEclair;

        static Ressources()
        {
            SpritesPerso = new BitmapImage[] {
                ChargerImage("SpritesPerso/imgPerso0.1.png"),
                ChargerImage("SpritesPerso/imgPerso0.2.png"),
                ChargerImage("SpritesPerso/imgPerso0.3.png"),
                ChargerImage("SpritesPerso/imgPerso0.4.png"),
                ChargerImage("SpritesPerso/imgPerso0.5.png"),
                ChargerImage("SpritesPerso/imgPerso0.6.png"),
                ChargerImage("SpritesPerso/imgPerso0.7.png"),
                ChargerImage("SpritesPerso/imgPerso0.8.png")
            };

            SpritesEclair = new BitmapImage[] {
                ChargerImage("SpritesEclair/imgEclair0.png"),
                ChargerImage("SpritesEclair/imgEclair1.png"),
                ChargerImage("SpritesEclair/imgEclair2.png"),
                ChargerImage("SpritesEclair/imgEclair3.png")
            };
        }

        private static BitmapImage ChargerImage(string chemin)
        {
            return new BitmapImage(new Uri("pack://application:,,,/" + chemin));
        }
    }
}
