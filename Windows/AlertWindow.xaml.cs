﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Terminal_XP.Classes;

namespace Terminal_XP.Windows
{
    /// <summary>
    /// Логика взаимодействия для AlertWindow.xaml
    /// </summary>
    public partial class AlertWindow : Window
    {
        
        public AlertWindow()
        {
            InitializeComponent();
        }
        
        public AlertWindow(string title, string message, string button, string theme) : this()
        {
            LoadTheme(theme);
            LoadParams(title, message, button);

            KeyDown += (obj, e) =>
            {
                if (e.Key == Key.Enter || e.Key == Key.Escape)
                    Close();
            };
        }

        private void LoadTheme(string theme)
        {
            LblTitle.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Addition.Themes + theme + "/#Fallout Regular");
            LblMessage.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Addition.Themes + theme + "/#Fallout Regular");
            LblButton.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), Addition.Themes + theme + "/#Fallout Regular");
            
            Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(Addition.Themes + theme + "/Alert_background.png", UriKind.Relative)) };
        }

        private void LoadParams(string title, string message, string button)
        {
            LblTitle.Content = title;
            LblMessage.Content = message;
            LblButton.Content = button;
            
            LblTitle.FontSize = ConfigManager.Config.FontSize;
            LblTitle.Foreground = (Brush)new BrushConverter().ConvertFromString(ConfigManager.Config.TerminalColor);
            
            LblMessage.FontSize = ConfigManager.Config.FontSize;
            LblMessage.Foreground = (Brush)new BrushConverter().ConvertFromString(ConfigManager.Config.TerminalColor);
            
            LblButton.FontSize = ConfigManager.Config.FontSize;
            LblButton.Foreground = (Brush)new BrushConverter().ConvertFromString(ConfigManager.Config.TerminalColor);
        }
    }
}
