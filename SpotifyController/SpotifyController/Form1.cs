using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Speech.Recognition;
using SpotifyAPI.Local;
using System.IO;
using System.Linq;

namespace SpotifyController
{
    public partial class FormMain : Form
    {
        private readonly SpeechRecognitionEngine _sre = new SpeechRecognitionEngine();
        private bool _enable;
        private string _currentSong = "";
        private readonly string _dir = Directory.GetCurrentDirectory() + "\\Song.txt";
        
        //Different words, since my english is not the best, and it didn't detect very well.
        private readonly string[] _play = { "play", "way", "late", "date", "eight", "lay" };
        private readonly string[] _pause = { "pause", "pulse", "balls", "boasts" };
        private readonly string[] _next = { "next" };
        private readonly string[] _previous = { "previous", "previews", "preview" };
        private readonly string[] _up = { "up", "out" };
        private readonly string[] _down = { "down", "non" };
        private readonly string[] _enableRecognition = {"enable", "naval", "able"};

        private readonly Spotify _spotify;


        public FormMain()
        {
            InitializeComponent();
            _spotify = new Spotify();
            EnableVoice();
            CreateDirectory();
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

        private void Listener(object o, SpeechRecognizedEventArgs e)
        {
            _spotify.CheckSpotify();
            SaveSong();
            foreach (var rec in e.Result.Words)
            {
                if (_enable)
                {
                    labelDetected.Text = rec.Text;
                    if (_play.Contains(rec.Text))
                        _spotify.Play();

                    if (_pause.Contains(rec.Text))
                        _spotify.Pause();

                    if (_next.Contains(rec.Text))
                        _spotify.Next();

                    if (_previous.Contains(rec.Text))
                        _spotify.Previous();

                    if (_up.Contains(rec.Text))
                    {
                        if (_spotify.GetVolume() + 5 <= 100)
                            _spotify.SetVolume(5);

                        else if (_spotify.GetVolume() + 1 <= 100)
                            _spotify.SetVolume(1);
                    }
                    if (_down.Contains(rec.Text))
                    {
                        if (_spotify.GetVolume() - 5 >= 0)
                            _spotify.SetVolume(-5);

                        else if(_spotify.GetVolume() - 1 >= 0)
                            _spotify.SetVolume(-1);
                    }
                }
                if (_enableRecognition.Contains(rec.Text))
                {
                    _enable = !_enable;
                    labelDetected.Text = $@"Enable: {_enable}";
                }
            }
        }

        private void CreateDirectory()
        {
            if (!File.Exists(_dir))
                File.Create(_dir);
        }

        private void SaveSong()
        {
            if (_currentSong == _spotify.GetPlaying() || !_spotify.Connected()) return;
                
            _currentSong = _spotify.GetPlaying();

            File.WriteAllText(_dir, _currentSong);
            labelSong.Text = _currentSong;
        }
    }
}