// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Microsoft.Toolkit.Uwp.Notifications;
using Sigma.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows;
//using Windows.ApplicationModel.Activation;
//using Windows.Foundation.Collections;

namespace Sigma
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServer Server { get; private set; }

        public static App Instance => ((App)App.Current);

        protected override void OnStartup(StartupEventArgs e)
        {
            //TODO: initialize Server application property
            base.OnStartup(e);
            ChangeLanguage(Sigma.Properties.Settings.Default.Language);
            Sigma.Properties.Settings.Default.PropertyChanged += SettingChanged;
        }

        private void SettingChanged(object? sendeer, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Sigma.Properties.Settings.Default.Language))
                ChangeLanguage(Sigma.Properties.Settings.Default.Language);
        }

        private static void ChangeLanguage(string languageCode)
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            switch (languageCode)
            {
                case "de":
                    dictionary.Source = new System.Uri("..\\Properties\\languages\\lang_de.xaml", System.UriKind.Relative);
                    break;
                case "en":
                    dictionary.Source = new System.Uri("..\\Properties\\languages\\lang_en.xaml", System.UriKind.Relative);
                    break;
                default:
                    dictionary.Source = new System.Uri("..\\Properties\\languages\\lang_en.xaml", System.UriKind.Relative);
                    break;
            }
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }
        public static string[] GetAllLocalIPv4(NetworkInterfaceType _type)
        {
            List<string> ipAddrList = new List<string>();
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddrList.Add(ip.Address.ToString());
                        }
                    }
                }
            }
            return ipAddrList.ToArray();
        }
    }
}