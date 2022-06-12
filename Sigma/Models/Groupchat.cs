﻿// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Sigma.Interfaces;
using System.Collections.Generic;

namespace Sigma.Models
{
    public class Groupchat : ChatRoom
    {
        public List<User> Participants { get; set; }
        public long RoomId { get; }

        public Groupchat(long roomId, string name, string picture, User me, ISender sender) : base(name, picture, me, sender)
        {
            RoomId = roomId;
            Participants = new();
        }
    }
}
