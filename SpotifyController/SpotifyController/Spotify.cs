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
        private string _currentPlaying = "";
        private readonly string _dir = Directory.GetCurrentDirectory() + "\\Song.txt";

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
            _spotify.ListenForEvents = true;
        }

        public void UpdateTrack()
        {
            _spotify.OnTrackChange += _spotify_OnTrackChange;
        }

        private void _spotify_OnTrackChange(object sender, TrackChangeEventArgs e)
        {
            _currentPlaying = e.NewTrack.TrackResource.Name + " - " + e.NewTrack.ArtistResource.Name;
            SaveSong();
        }

        public bool Connected()
        {
            try
            {
                return _spotify.Connect();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + @"

Please restart the application.", @"Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
                return false;
            }
        }

        public void CheckSpotify()
        {
            if (!SpotifyLocalAPI.IsSpotifyRunning())
            {
                MessageBox.Show(@"Spotify is not running!

Application will now close.", @"Not Running", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            if (SpotifyLocalAPI.IsSpotifyWebHelperRunning()) return;
            MessageBox.Show(@"Spotify is not running!

Application will now close.", @"Not Running", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(0);
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
            return string.IsNullOrEmpty(_spotify.GetStatus().Track.ArtistResource.Name) ? "" : _spotify.GetStatus().Track.ArtistResource.Name;
        }

        public string GetSong()
        {
            return string.IsNullOrEmpty(_spotify.GetStatus().Track.TrackResource.Name) ? "" : _spotify.GetStatus().Track.TrackResource.Name;
        }

        public string GetPlaying()
        {
            return !string.IsNullOrEmpty(_currentPlaying) ? _currentPlaying : $@"{GetSong()} - {GetArtist()}";
        }

        private void SaveSong()
        {
            if (!Connected()) return;

            File.WriteAllText(_dir, GetPlaying());
        }
    }
}
