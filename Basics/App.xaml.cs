// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Sigma.Interfaces;
using System.ComponentModel;
using System.Windows;

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
    }
}