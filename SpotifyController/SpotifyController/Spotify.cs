using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpotifyAPI.Local;

namespace SpotifyController
{
    public class Spotify
    {
        private SpotifyLocalAPI _spotify;

        public Spotify()
        {
            Connect();
        }

        public void Connect()
        {
            _spotify = new SpotifyLocalAPI();
            CheckSpotify();
            if (!Connected())
            {
                MessageBox.Show(@"Error connecting to Spotify!

Application will now close.", @"Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        public bool Connected()
        {
            return _spotify.Connect();
        }

        public void CheckSpotify()
        {
            if (!SpotifyLocalAPI.IsSpotifyRunning())
            {
                MessageBox.Show(@"Spotify is not running!

Application will now close.", @"Not Running", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
            {
                MessageBox.Show(@"Spotify is not running!

Application will now close.", @"Not Running", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        public async void Play()
        {
            await _spotify.Play();
        }

        public async void Pause()
        {
            await _spotify.Pause();
        }

        public void Next()
        {
            _spotify.Skip();
        }

        public void Previous()
        {
            _spotify.Previous();
        }

        public float GetVolume()
        {
            return _spotify.GetSpotifyVolume();
        }

        public void SetVolume(float value)
        {
            _spotify.SetSpotifyVolume(GetVolume() + value);
        }

        public string GetArtist()
        {
            return _spotify.GetStatus().Track.ArtistResource.Name;
        }

        public string GetSong()
        {
            return _spotify.GetStatus().Track.TrackResource.Name;
        }

        public string GetPlaying()
        {
            return $"{GetSong()} - {GetArtist()}";
        }
    }
}
