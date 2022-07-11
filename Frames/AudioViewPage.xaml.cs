using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Terminal_XP.Classes;

namespace Terminal_XP.Frames
{
    public partial class AudioViewPage : Page
    {
        public const int CntSymbol = 64;
        private string _filename;
        private string _theme;
        private bool _stop;
        private MediaPlayer _player = new MediaPlayer();
        private DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Input);
        private bool _loaded;

        public double Volume
        {
            get => _player.Volume;
            set
            {
                _player.Volume = Math.Max(Math.Min(value, 1.0), 0.0);
            }
        }

        public AudioViewPage(string filename, string theme)
        {
            InitializeComponent();

            _filename = filename;
            _theme = theme;

            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Tick += UpdateProgressBar;
            _timer.Start();

            _player.MediaEnded += (obj, e) =>
            {
                _player.Position = new TimeSpan(0, 0, 0);
                _player.Play();
            };

            ProgressBar.Text = $"[{new string('-', (int)CntSymbol)}]";

            Focusable = true;
            Focus();

            KeyDown += AdditionalKeys;

            LoadTheme(theme);
            LoadAudio();
        }

        private void UpdateProgressBar(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            var nowTime = _player.Position.TotalSeconds;
            nowTime = nowTime > _player.NaturalDuration.TimeSpan.TotalSeconds ? _player.NaturalDuration.TimeSpan.TotalSeconds : nowTime;

            var ind = (int)Math.Ceiling(nowTime / _player.NaturalDuration.TimeSpan.TotalSeconds * CntSymbol);
            ProgressBar.Text = $"[{new string('>', ind)}{new string('=', CntSymbol - ind)}]";
        }


        public void Closing()
        {
            _loaded = false;

            _timer.Stop();
            Stop();
            _player.Close();
        }

        public void Reload()
        {
            LoadTheme(_theme);
            LoadAudio();
        }

        private void LoadTheme(string name)
        {

        }

        public void Play() => _player.Play();

        public void Stop() => _player.Stop();

        public void Pause() => _player.Pause();

        public void VolumePlus() => Volume += 0.01d;

        public void VolumeMinus() => Volume -= 0.01d;

        private void LoadAudio()
        {
            if (!File.Exists(_filename))
                return;

            Stop();
            _player.Close();

            _player.Open(new Uri(_filename, UriKind.Relative));
            _player.Play();

            _player.MediaOpened += (obj, e) =>
            {
                ProgressBar.Text = $"[{new string('=', CntSymbol)}]";
                _loaded = true;
            };
        }
        private void AdditionalKeys(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Closing();
                    GC.Collect();
                    NavigationService.Navigate(new LoadingPage(Path.GetDirectoryName(_filename), _theme));
                    break;
                case Key.Space:
                    if (_stop)
                        Pause();
                    else
                        Play();
                    _stop = !_stop;
                    break;
                case Key.Up:
                case Key.VolumeUp:
                    VolumePlus();
                    break;
                case Key.Down:
                case Key.VolumeDown:
                    VolumeMinus();
                    break;
            }

        }
    }
}