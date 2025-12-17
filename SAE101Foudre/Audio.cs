using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SAE101Foudre
{
    public static class Audio
    {
        private static MediaPlayer musiqueFond = new MediaPlayer();
        private static MediaPlayer musiqueEffet = new MediaPlayer();

        public static void LancerMusiqueDeFond()
        {
            musiqueFond.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Sons/musique.wav"));
            musiqueFond.Volume = MenuOptions.VolumeValeur / 100.0;
            musiqueFond.MediaEnded += (s, e) => { musiqueFond.Position = TimeSpan.Zero; musiqueFond.Play(); };
            musiqueFond.Play();

            musiqueEffet.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Sons/tonerre.wav"));
            musiqueEffet.Volume = MenuOptions.VolumeValeur / 100.0;
            musiqueEffet.MediaEnded += (s, e) => { musiqueEffet.Position = TimeSpan.Zero; musiqueEffet.Play(); };
            musiqueEffet.Play();
        }

        public static void JouerSon(string fichier, double volume)
        {
            MediaPlayer son = new MediaPlayer();
            son.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Sons/" + fichier));
            son.Volume = volume;
            son.Play();
        }

        public static void ArreterMusique()
        {
            musiqueFond.Stop();
            musiqueEffet.Stop();
        }

        public static void ChangerVolume(double volumeSur100)
        {
            double volumeReel = volumeSur100 / 100.0;

            if (musiqueFond != null)
                musiqueFond.Volume = volumeReel;

            if (musiqueEffet != null)
                musiqueEffet.Volume = volumeReel;
        }
    }
}
