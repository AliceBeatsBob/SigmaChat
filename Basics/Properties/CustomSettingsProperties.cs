// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Sigma.Viewmodels;
using System;
using System.Collections.ObjectModel;

namespace Sigma.Properties
{
    [Serializable]
    class CustomSettingsProperties
    {
        public ObservableCollection<ChatRoomViewModel> ChatRoomViewModels { get; set; }

        public CustomSettingsProperties()
        {
            ChatRoomViewModels = new ObservableCollection<ChatRoomViewModel>();
        }
    }
}
