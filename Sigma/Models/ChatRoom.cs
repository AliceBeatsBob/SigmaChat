// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Newtonsoft.Json.Linq;
using Sigma.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Sigma.Models
{
    public class ChatRoom : BaseNotifyPropertyChanged
    {
        public ChatRoom()
        {
            
        }
        private string name;
        private string picture;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != Name)
                {
                    name = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Picture
        {
            get { return picture; }
            set
            {
                if (value != picture)
                {
                    picture = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<Message> ChatHistory { get; set; }

        public User Me { get; init; }

        public GrpcSender Sender { get; set; }

        public ChatRoom(string name, string picture, User me, ISender sender)
        {
            Me = me;
            Name = name;
            Picture = picture;
            ChatHistory = new();
            Sender = (GrpcSender)sender;
        }
    }
}