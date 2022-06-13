// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Sigma.Viewmodels;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Sigma.UserControls
{
    /// <summary>
    /// Interaction logic for Chatroom.xaml
    /// </summary>
    public partial class Chatroom : UserControl
    {
        public ChatRoomViewModel ChatRoomViewModel
        {
            get { return (ChatRoomViewModel)GetValue(ChatRoomViewModelProperty); }
            set { SetValue(ChatRoomViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChatRoomViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChatRoomViewModelProperty =
            DependencyProperty.Register("ChatRoomViewModel", typeof(ChatRoomViewModel), typeof(Chatroom), new PropertyMetadata(null));

        public Chatroom()
        {
            InitializeComponent();
            //ChatRoomViewModel = new ChatRoomViewModel();
            //ChatRoomViewModel.ScrollOnSendHandler += (_, _) => ScrollDown();
        }

        private void ScrollDown()
        {
            if(lstChat.Items.Count != 0)
                lstChat.ScrollIntoView(lstChat.Items[lstChat.Items.Count - 1]);
        }

        private void Button_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ScrollDown();
        }

        private void TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return || e.Key == System.Windows.Input.Key.Enter)
                ScrollDown();
        }

        private void lstChat_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(lstChat.SelectedValue != null)
            {
                Models.Message message = (Models.Message)lstChat.SelectedItem;
                if (message.Content.StartsWith("FILE: "))
                {
                    int start = message.Content.IndexOf(' ');
                    string path = message.Content.Substring(start + 1);
                    string args = string.Format($"/e, /select, \"{path}\"");

                    ProcessStartInfo info = new ProcessStartInfo();
                    info.FileName = "explorer";
                    info.Arguments = args;
                    Process.Start(info);
                }
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}