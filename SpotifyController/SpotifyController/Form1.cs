using System;
using System.Windows.Forms;
using System.Speech.Recognition;
using SpotifyAPI.Local;

namespace SpotifyController
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine sre = new SpeechRecognitionEngine();
        private bool enable;
        private SpotifyLocalAPI spotify;
        private bool connected;

        public Form1()
        {
            InitializeComponent();
            Connect();
            EnableVoice();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Connect()
        {
            spotify = new SpotifyLocalAPI();
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
            connected = spotify.Connect();
            if (connected)
            {
                label1.Text = "Connected to Spotify!";
            }
            else
            {
                label1.Text = "Could not connect to Spotify. Retrying..";
                if (MessageBox.Show("Could not connect. Want to retry?", "Connection Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    Connect();

            }
        }

        private void EnableVoice()
        {
            try
            {
                sre.SetInputToDefaultAudioDevice();
                sre.LoadGrammar(new DictationGrammar());
                sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Listener);
                sre.RecognizeAsync(RecognizeMode.Multiple);
                enable = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Listener(object o, SpeechRecognizedEventArgs e)
        {
            
            foreach (var rec in e.Result.Words)
            {
                if (enable)
                {
                    label1.Text = rec.Text;
                    if (rec.Text == "play"
                        || rec.Text == "way"
                        || rec.Text == "late"
                        || rec.Text == "date"
                        || rec.Text == "eight"
                        || rec.Text == "lay") //Different words, since my english is not the best, and it didn't detect very well.
                    {
                        spotify.Play();
                    }
                    if (rec.Text == "pause"
                        || rec.Text == "pulse"
                        || rec.Text == "balls"
                        || rec.Text == "boasts")
                    {
                        spotify.Pause();
                    }
                    if (rec.Text == "next")
                    {
                        spotify.Skip();
                    }
                    if (rec.Text == "previous"
                        || rec.Text == "previews"
                        || rec.Text == "preview")
                    {
                        spotify.Previous();
                    }
                }
                if (rec.Text == "enable"
                    || rec.Text == "naval"
                    || rec.Text == "able")
                {
                    enable = !enable;
                    label1.Text = "Enable: " + enable;
                }
            }
        }
    }
}
