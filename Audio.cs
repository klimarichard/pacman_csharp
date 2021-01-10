using System;
using System.Threading;

using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace pacman {
    /// <summary>
    /// The <c>Audio</c> class takes care of all music and sounds
    /// being played in the game.
    /// </summary>
    public class Audio {
        private Song currentlyPlaying;
        private bool playingMenu;
        private bool loop;
        private bool playing;

        /// <summary>
        /// Constructor of the <c>Audio</c> class. It sets the main volume
        /// and initializes class variables.
        /// </summary>
        public Audio()
        {
            MediaPlayer.Volume = 0.5f;
            playingMenu = false;
            loop = false;
            playing = false;
        }

        /// <summary>
        /// Plays given non-repeating song.
        /// </summary>
        /// <param name="song">the song to be played</param>
        public void playSong(Song song)
        {
            Thread t = new Thread(() => playSong(song, false));
            t.Start();
        }

        /// <summary>
        /// Plays given song and sets repeatability.
        /// </summary>
        /// <param name="song">the song to be played</param>
        /// <param name="loop">repeatability of the song</param>
        public void playSong(Song song, bool loop)
        {
            if (playing && (currentlyPlaying != song) && (currentlyPlaying != Assets.menu))
            {
                fadeOut();
            }

            this.loop = loop;
            currentlyPlaying = song;
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.IsRepeating = loop;
            MediaPlayer.Play(song);
            if (song == Assets.menuLoop)
            {
                playingMenu = true;
            }
            else
            {
                playingMenu = false;
            }
            playing = true;
        }

        /// <summary>
        /// Plays given sound effect.
        /// </summary>
        /// <param name="soundEffect">the sound effect to be played</param>
        public void playOneShot(SoundEffect soundEffect)
        {
            soundEffect.Play(0.8f, 0.0f, 0.0f);
        }

        private void fadeOut()
        {
            float currentVolume = MediaPlayer.Volume;

            while (currentVolume > 0.0f)
            {
                currentVolume -= 0.0001f;
                MediaPlayer.Volume = Math.Max(0, currentVolume);
            }

            MediaPlayer.Stop();
            playing = false;
        }

        #region Getters & Setters
        /// <summary>
        /// Gets current playing status.
        /// </summary>
        /// <returns><c>true</c>, if music is playing, <c>false</c> otherwise</returns>
        public bool isPlaying()
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                playing = true;
            }
            else
            {
                playing = false;
            }

            return playing;
        }

        /// <summary>
        /// Gets current menu playing status.
        /// </summary>
        /// <returns><c>true</c>, if menu music is playing, <c>false</c> otherwise</returns>
        public bool isPlayingMenu()
        {
            if (isPlaying())
            {
                return playingMenu;
            }

            return false;
        }

        #endregion
    }
}
