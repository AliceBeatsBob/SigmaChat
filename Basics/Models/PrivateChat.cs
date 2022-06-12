// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Sigma.Interfaces;

namespace Sigma.Models
{
    public class PrivateChat : ChatRoom
    {
        public User OtherUser { get; set; }

        public PrivateChat(User otherUser, string picture, User me, ISender sender) : base(otherUser.UserName, picture, me, sender)
        {
            OtherUser = otherUser;
        }
    }
}
