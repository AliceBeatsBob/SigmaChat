﻿// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Sigma.Viewmodels;
using System.Windows;

namespace Sigma.Windows
{
    /// <summary>
    /// Interaction logic for AddContacts.xaml
    /// </summary>
    public partial class AddContacts : Window
    {
        public AddContacsViewModel AddContacsViewModel
        {
            get { return (AddContacsViewModel)GetValue(AddContacsViewModelProperty); }
            set { SetValue(AddContacsViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddContacsViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddContacsViewModelProperty =
            DependencyProperty.Register("AddContacsViewModel", typeof(AddContacsViewModel), typeof(AddContacts), new PropertyMetadata(null));

        public AddContacts()
        {
            InitializeComponent();
        }

        private void btn_createGroup_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}