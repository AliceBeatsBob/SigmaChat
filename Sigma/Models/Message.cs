﻿// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using System;
using System.Text.Json.Serialization;

namespace Sigma.Models
{
    public class Message
    {
        public string Content { get; set; }
        public User Sender { get; set; }

        public Message(User sender, string content)
        {
            Content = content;
            Sender = sender;
        }
    }
}