using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Speech.Recognition;
using SpotifyAPI.Local;
using System.IO;

namespace SpotifyController
{
    public partial class FrmMain : Form
    {
        private readonly SpeechRecognitionEngine _sre = new SpeechRecognitionEngine();
        private bool _enable;
        private SpotifyLocalAPI _spotify;
        private bool _connected;
        private string _currentSong = "";
        private readonly string _dir = Directory.GetCurrentDirectory() + "\\Song.txt";
        
        //Different words, since my english is not the best, and it didn't detect very well.
        private readonly List<string> _play = new List<string> { "play", "way", "late", "date", "eight", "lay" };
        private readonly List<string> _pause = new List<string> { "pause", "pulse", "balls", "boasts" };
        private readonly List<string> _next = new List<string> { "next" };
        private readonly List<string> _previous = new List<string> { "previous", "previews", "preview" };
        private readonly List<string> _up = new List<string> { "up", "out" };
        private readonly List<string> _down = new List<string> { "down", "non" };
        private readonly List<string> _enableRecognition = new List<string> {"enable", "naval", "able"};

        public FrmMain()
        {
            InitializeComponent();
            Connect();
            EnableVoice();
            CreateDirectory();
        }

        private void Connect()
        {
            _spotify = new SpotifyLocalAPI();
            CheckSpotify();
            _connected = _spotify.Connect();
            if (_connected)
            {
                lblDetected.Text = "Connected to Spotify!";
            }
            else
            {
                lblDetected.Text = "Could not connect to Spotify. Retrying..";
                if (MessageBox.Show("Could not connect. Want to retry?", "Connection Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    Connect();

            }
        }

        private void EnableVoice()
        {
            try
            {
                _sre.SetInputToDefaultAudioDevice();
                _sre.LoadGrammar(new DictationGrammar());
                _sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Listener);
                _sre.RecognizeAsync(RecognizeMode.Multiple);
                _enable = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckSpotify()
        {
            if (!SpotifyLocalAPI.IsSpotifyRunning())
            {
                MessageBox.Show("Spotify is not running!\n\nApplication will now close.", "Not Running", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
            {
                MessageBox.Show("Spotify Wel Helper is not running!\n\nApplication will now close.", "Not Running", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private async void Listener(object o, SpeechRecognizedEventArgs e)
        {
            CheckSpotify();
            foreach (var rec in e.Result.Words)
            {
                if (_enable)
                {
                    lblDetected.Text = rec.Text;
                    if (_play.Contains(rec.Text)) 
                    {
                        await _spotify.Play();
                    }
                    if (_pause.Contains(rec.Text))
                    {
                        await _spotify.Pause();
                    }
                    if (_next.Contains(rec.Text))
                    {
                        _spotify.Skip();
                    }
                    if (_previous.Contains(rec.Text))
                    {
                        _spotify.Previous();
                    }
                    if (_up.Contains(rec.Text))
                    {
                        if (_spotify.GetSpotifyVolume() + 5 <= 100)
                        {
                            _spotify.SetSpotifyVolume(_spotify.GetSpotifyVolume() + 5);
                        }
                        else if (_spotify.GetSpotifyVolume() + 1 <= 100)
                        {
                            _spotify.SetSpotifyVolume(_spotify.GetSpotifyVolume() + 1);
                        }
                    }
                    if (_down.Contains(rec.Text))
                    {
                        if (_spotify.GetSpotifyVolume() - 5 >= 0)
                        {
                            _spotify.SetSpotifyVolume(_spotify.GetSpotifyVolume() - 5);
                        }
                        else if(_spotify.GetSpotifyVolume() - 1 >= 0)
                        {
                            _spotify.SetSpotifyVolume(_spotify.GetSpotifyVolume() - 1);
                        }
                    }
                }
                if (_enableRecognition.Contains(rec.Text))
                {
                    _enable = !_enable;
                    lblDetected.Text = $@"Enable: {_enable}";
                }
            }
            SaveSong();
        }

        private void CreateDirectory()
        {
            if (!File.Exists(_dir))
                File.Create(_dir);
        }

        private void SaveSong()
        {
            if (_currentSong == _spotify.GetStatus().Track.ArtistResource.Name + " - " +
                _spotify.GetStatus().Track.TrackResource.Name) return;
                
            _currentSong = _spotify.GetStatus().Track.ArtistResource.Name + " - " +
                            _spotify.GetStatus().Track.TrackResource.Name;

            File.WriteAllText(_dir, _currentSong);
            lblSong.Text = _currentSong;
        }
    }
}