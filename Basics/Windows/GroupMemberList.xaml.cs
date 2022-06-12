// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Sigma.Viewmodels;
using System.Windows;

namespace Sigma.Windows
{
    /// <summary>
    /// Interaction logic for GroupMemberList.xaml
    /// </summary>
    public partial class GroupMemberList : Window
    {
        public GroupMemberListViewModel GroupMemberListViewModel
        {
            get { return (GroupMemberListViewModel)GetValue(GroupMemberListViewModelProperty); }
            set { SetValue(GroupMemberListViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupMemberListViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupMemberListViewModelProperty =
            DependencyProperty.Register("GroupMemberListViewModel", typeof(GroupMemberListViewModel), typeof(GroupMemberList), new PropertyMetadata(null));

        public GroupMemberList()
        {
            InitializeComponent();
        }
    }
}
