﻿using System;
using System.Linq;
using System.Windows;
using Terminal_XP.Frames;
using Terminal_XP.Classes;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Path = System.IO.Path;

namespace Terminal_XP
{
    public partial class MainWindow : Window
    {
        private string _theme;
        private bool _stop = true;

        public MainWindow()
        {
            InitializeComponent();

            ConfigManager.Load();

            _theme = ConfigManager.Config.Theme;

            LoadTheme(_theme);
            LoadParams();

            ExecuteFile(Addition.Local + "/Test.flac");
        }

        private void LoadTheme(string name)
        {
            Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(Addition.Themes + name + "/Background.png", UriKind.Relative)) };
        }

        private void LoadParams()
        {
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            ResizeMode = ResizeMode.NoResize;
            AllowsTransparency = true;

            DevicesManager.AddDisk += name => Logger.Debug($"Add disk: {name}");
            DevicesManager.RemoveDisk += name => Logger.Debug($"Remove disk: {name}");

            KeyDown += (obj, e) =>
            {
                switch (e.Key)
                {
                    case Key.Escape:
                        Close();
                        break;
                    // TODO: Delete in future. In this moment using for test
                    case Key.R:
                        Frame.NavigationService.Content.GetType().GetMethod("Reload")?.Invoke(Frame.NavigationService.Content, default);
                        break;
                    // End TODO
                    case Key.Space:
                        Frame.NavigationService.Content.GetType().GetMethod(_stop ? "Pause" : "Play")?.Invoke(Frame.NavigationService.Content, default);

                        _stop = !_stop;
                        break;
                    case Key.Up:
                    case Key.VolumeUp:
                        Frame.NavigationService.Content.GetType().GetMethod("VolumePlus")?.Invoke(Frame.NavigationService.Content, default);
                        break;
                    case Key.Down:
                    case Key.VolumeDown:
                        Frame.NavigationService.Content.GetType().GetMethod("VolumeMinus")?.Invoke(Frame.NavigationService.Content, default);
                        break;
                }
            };

            Closing += (obj, e) =>
            {
                Frame.NavigationService.Content.GetType().GetMethod("Closing")?.Invoke(Frame.NavigationService.Content, default);
            };
        }

        private void ExecuteFile(string filename)
        {
            var exct = Path.GetExtension(filename).Remove(0, 1);

            var picture = new[] { "jpeg", "jpg", "tiff", "bmp" };
            var video = new[] { "mp4", "gif", "wmv", "avi" };
            var text = new[] { "txt" };
            var audio = new[] { "wav", "m4a", "mp3", "flac" };

            if (picture.Contains(exct))
                Frame.NavigationService.Navigate(new PictureViewPage(filename, _theme));

            if (text.Contains(exct))
                Frame.NavigationService.Navigate(new TextViewPage(filename, _theme));

            if (video.Contains(exct))
                Frame.NavigationService.Navigate(new VideoViewPage(filename, _theme));

            if (audio.Contains(exct))
                Frame.NavigationService.Navigate(new AudioViewPage(filename, _theme));
        }
    }
}
