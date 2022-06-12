// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Sigma.Models;
using System.Collections.Generic;
using System.Windows;

namespace Sigma.Viewmodels
{
    public class GroupMemberListViewModel : BaseViewModel
    {
        public List<User> Participants { get; }
        public string ChatroomName { get; }

        public GroupMemberListViewModel(List<User> patricipants, string chatroomName)
        {
            Participants = patricipants;
            ChatroomName = chatroomName + " " + Application.Current.FindResource("GroupMemberListPartTitle");
        }
    }
}
