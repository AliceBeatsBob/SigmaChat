// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Sigma.Viewmodels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Sigma.Windows
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : Window
    {
        public Welcome()
        {
            InitializeComponent();
            WelcomeViewModel vm = new WelcomeViewModel();
            DataContext = vm;
            vm.CloseAction = new Action(Close);
        }
        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}