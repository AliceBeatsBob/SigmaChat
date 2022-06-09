﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Basics.Interfaces
{
    public interface ISender
    {
        public Task<bool> SendPrivateMessage(IPAddress reciverIp, long senderId, string messsage);
        public Task<bool> SendGroupMessage(IPAddress reciverIp, long senderId, string messsage, long roomId);

        public Task<bool> TransmitChatroomParticipantsToAddedUser(IPAddress reciverIp, long roomId, IPAddress participantIp, long participantId, string participantName, string participantPfp);
        public Task<bool> TellOthersANewUserWasAddedToChatroomAsync(IPAddress reciverIp, long roomId, IPAddress addedUserIp, long addedUserId, string addedUserName, string addedUserPfp);
        public Task<bool> RequestedUserPrivate(IPAddress reciverIp, IPAddress senderIp);
        public Task<bool> SendUser(IPAddress reciverIp, IPAddress myIp, long myId, string myName, string myPfp);
        public Task<bool> OpenPrivateChat(IPAddress reciverIp, IPAddress myIp, long myId, string myName, string myPfp);
        public Task<bool> OpenPrivateChatroom(IPAddress reciverIp, IPAddress myIp, long myId, string myName, string myPfp);
        public Task<bool> AddToGroupchat(IPAddress reciverIp, long roomId, string name, string pfp, IPAddress senderIp, long senderId, string sendername, string senderPfp);
    }
}
