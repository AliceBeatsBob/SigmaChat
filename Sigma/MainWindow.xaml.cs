﻿// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Microsoft.Toolkit.Uwp.Notifications;
using Sigma.Viewmodels;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shell;

namespace Sigma
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public static EventHandler OnClose;
        public MainWindow()
        {
            // gets the windows language if language is not in settings (first time starting or something went wrong)
            if (string.IsNullOrEmpty(Properties.Settings.Default.Language))
            {
                // gets the first part of the selected language (en for en-GB)
                string language = CultureInfo.CurrentUICulture.ToString().Split('-')[0];
                // saves the language for the next start
                Properties.Settings.Default.Language = language;
                Properties.Settings.Default.Save();
            }
            if (string.IsNullOrEmpty(Properties.Settings.Default.Name))
            {
                Windows.Welcome welcome = new();
                welcome.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                welcome.ShowInTaskbar = false;
                welcome.ShowDialog();
            }
            if (string.IsNullOrEmpty(Properties.Settings.Default.Name))
            {
                MessageBox.Show("Error, no Name was given.", "Alert", MessageBoxButton.OK);
                this.Close();
            }
            if (Properties.Settings.Default.UserId == -1)
            {
                Properties.Settings.Default.UserId = DateTime.Now.Ticks;
                Properties.Settings.Default.Save();
            }
            Instance = this;
            InitializeComponent();
            //this.DataContext = new MainWindowViewModel();
            ((MainWindowViewModel)DataContext).SwaptoSetting += (sender, _) => SwaptoSetting();
            ((MainWindowViewModel)DataContext).ThemeChanged += (sender, _) => Changetheme();
            ((MainWindowViewModel)DataContext).LeftGroup += (sender, e) => SelectedChanged(sender, null);

            Chat.Visibility = Visibility.Collapsed;
            Set.Visibility = Visibility.Collapsed;

            this.Resources["Backgroundcolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Properties.Settings.Default.BackgroundColor));
            this.Resources["Bordercolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Properties.Settings.Default.BorderColor));
            this.Resources["Menucolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Properties.Settings.Default.MenuColor));
            this.Resources["Textcolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Properties.Settings.Default.TextColor));
            this.Resources["Userlistboxcolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Properties.Settings.Default.UserListBoxColor));
            this.Resources["Textfieldcolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Properties.Settings.Default.TextfielColor));
            this.Resources["Buttoncolor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Properties.Settings.Default.ButtonColor));

            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                MainWindow.Instance.Dispatcher.Invoke(delegate ()
                {
                    try
                    {
                        if (!MainWindow.Instance.IsVisible)
                        {
                            MainWindow.Instance.Show();
                        }
                        if (MainWindow.Instance.WindowState == WindowState.Minimized)
                        {
                            MainWindow.Instance.WindowState = WindowState.Normal;
                        }
                        MainWindow.Instance.Activate();
                        MainWindow.Instance.Topmost = true;
                        MainWindow.Instance.Topmost = false;
                        MainWindow.Instance.Focus();
                    }
                    catch { }
                });
            };
        }

        private void SwaptoSetting()
        {
            Chatselection.SelectedIndex = -1;
            Chat.Visibility = Visibility.Collapsed;
            Set.Visibility = Visibility.Visible;
        }

        private void Changetheme()
        {
            Color background, border, menu, text, listbox, textfield, buttoncolor;
            switch (Properties.Settings.Default.Theme)
            {
                case "Blue":
                    background = Color.FromRgb(240, 248, 255);
                    border = Color.FromRgb(0, 0, 0);
                    menu = Color.FromRgb(245, 248, 255);
                    text = Color.FromRgb(0, 0, 0);
                    listbox = Color.FromRgb(163, 215, 245);
                    textfield = Color.FromRgb(255, 255, 255);
                    buttoncolor = Color.FromArgb(50, 163, 215, 245);

                    Colorsetter(background, border, menu, text, listbox, textfield, buttoncolor);
                    break;

                case "Black":
                    background = Color.FromRgb(50, 50, 50);
                    border = Color.FromRgb(255, 255, 255);
                    menu = Color.FromRgb(100, 100, 100);
                    text = Color.FromRgb(255, 255, 255);
                    listbox = Color.FromRgb(50, 50, 50);
                    textfield = Color.FromRgb(100, 100, 100);
                    buttoncolor = menu;

                    Colorsetter(background, border, menu, text, listbox, textfield, buttoncolor);
                    break;

                case "Yellow":
                    background = (Color)ColorConverter.ConvertFromString("Beige"); //245,245,220
                    border = Color.FromRgb(0, 0, 0);
                    menu = Color.FromRgb(250, 250, 230);
                    text = Color.FromRgb(0, 0, 0);
                    listbox = Color.FromRgb(245, 200, 160);
                    textfield = Color.FromRgb(255, 255, 235);
                    buttoncolor = menu;

                    Colorsetter(background, border, menu, text, listbox, textfield, buttoncolor);
                    break;

                case "Green":
                    background = Color.FromRgb(27, 188, 156);
                    border = Color.FromRgb(0, 0, 0);
                    menu = Color.FromRgb(32, 178, 170);
                    text = Color.FromRgb(0, 0, 0);
                    listbox = Color.FromRgb(27, 188, 156);
                    textfield = menu;
                    buttoncolor = menu;

                    Colorsetter(background, border, menu, text, listbox, textfield, buttoncolor);
                    break;
            }
        }

        private void Colorsetter(Color background, Color border, Color menu, Color text, Color listbox, Color textfield, Color buttoncolor)
        {
            this.Resources["Backgroundcolor"] = new SolidColorBrush(background);
            this.Resources["Bordercolor"] = new SolidColorBrush(border);
            this.Resources["Menucolor"] = new SolidColorBrush(menu);
            this.Resources["Textcolor"] = new SolidColorBrush(text);
            this.Resources["Userlistboxcolor"] = new SolidColorBrush(listbox);
            this.Resources["Textfieldcolor"] = new SolidColorBrush(textfield);
            this.Resources["Buttoncolor"] = new SolidColorBrush(buttoncolor);
            Properties.Settings.Default.BackgroundColor = background.ToString();
            Properties.Settings.Default.BorderColor = border.ToString();
            Properties.Settings.Default.MenuColor = menu.ToString();
            Properties.Settings.Default.TextColor = text.ToString();
            Properties.Settings.Default.UserListBoxColor = listbox.ToString();
            Properties.Settings.Default.TextfielColor = textfield.ToString();
            Properties.Settings.Default.ButtonColor = buttoncolor.ToString();
            Properties.Settings.Default.Save();
        }

        private void SelectedChanged(object sender, RoutedEventArgs e)
        {
            if (Chatselection.SelectedIndex >= 0)
            {
                Chat.Visibility = Visibility.Visible;
                Set.Visibility = Visibility.Collapsed;
            }
            else
                Chat.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //tbl_ip.IsEnabled = false;
            string localIP = App.GetAllLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Wireless80211).FirstOrDefault() ??
                             App.GetAllLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Ethernet).FirstOrDefault();

            Clipboard.SetText(localIP);

            Task.Run(() => changeText());

            void changeText()
            {
                this.Dispatcher.Invoke(delegate ()
                {
                    tbl_ip.SetResourceReference(Button.ContentProperty, "StrCopyIpButtonClicked");
                });
                Thread.Sleep(1000);
                this.Dispatcher.Invoke(delegate ()
                {
                    tbl_ip.SetResourceReference(Button.ContentProperty, "StrCopyIpButton");
                    //tbl_ip.IsEnabled = true;
                });
            }
        }

        private void WindowClosing(object? sender, CancelEventArgs e)
        {
            OnClose.Invoke(sender, e);
        }
    }
}