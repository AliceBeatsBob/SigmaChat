﻿// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Sigma.Commands;
using Sigma.Interfaces;
using Sigma.Models;
using Sigma.Windows;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;

namespace Sigma.Viewmodels
{
    public class ChatRoomViewModel : BaseViewModel
    {
        public ChatRoom ChatRoom { get; }
        public DelegateCommand AddCommand { get; }
        public DelegateCommand AddUserToGroupchatCommand { get; }
        public DelegateCommand LeavGroupChatCommand { get; }
        public DelegateCommand UploadCommand { get; }
        public DelegateCommand ListMembersCommand { get; }

        public event EventHandler MessageSentBringChatToTopHandler;

        public event EventHandler LeavGroupChatEventHandler;


        string currentMessage;
        public string CurrentMessage
        {
            get => this.currentMessage;
            set
            {
                if (value != this.currentMessage)
                {
                    this.currentMessage = value;
                    this.RaisePropertyChanged();
                    this.AddCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public ChatRoomViewModel(ChatRoom chatRoom)
        {
            this.ChatRoom = chatRoom;
            this.AddCommand = new DelegateCommand(_ => CanAddMessage(), _ => AddMessage());
            this.AddUserToGroupchatCommand = new DelegateCommand(
                _ =>
                {
                    // prüfen ob admin
                    return true;
                },
                _ =>
                {
                    AddUserToGroupchat();
                });
            this.LeavGroupChatCommand = new DelegateCommand(
                _ =>
                {
                    LeaveGroup();
                });
            this.UploadCommand = new DelegateCommand(
                _ =>
                {
                    SendFile();
                });
            this.ListMembersCommand = new DelegateCommand(
                _ =>
                {
                    ListGroupchatParticipants();
                });
        }

        private void ListGroupchatParticipants()
        {
            var userListVm = new GroupMemberListViewModel(((Groupchat)ChatRoom).Participants, ChatRoom.Name);
            GroupMemberList memberList = new() { GroupMemberListViewModel = userListVm };
            memberList.Resources = MainWindow.Instance.Resources;
            memberList.ShowDialog();
        }

        private void AddUserToGroupchat()
        {
            var addUserVm = new AddUserToGroupChatViewModel();
            AddUserToGroupChat addUser = new() { AddUserToGroupChatViewModel = addUserVm };
            addUser.Resources = MainWindow.Instance.Resources;
            var result = addUser.ShowDialog();
            if (result == true)
            {
                AddUserToChatroom(IPAddress.Parse(addUserVm.IpField));
            }
        }

        private void LeaveGroup()
        {
            var result = MessageBox.Show(Application.Current.FindResource("StrLeaveGroupMsg").ToString(), Application.Current.FindResource("StrLeaveGroupMsgTitle").ToString(), MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Cancel);
            if (result == MessageBoxResult.Yes)
                LeavGroupChatEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private async void SendFile()
        {
            string[] validExtensions = new string[] { "jpg", "jpeg", "png", "zip", "rar", "exe", "ඞ", "sus" };
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "image files (*.jpg, *.png)|*.jpg;*.jpeg;*.png|exe (*.exe)|*.exe|amogus (*.ඞ)|*.ඞ;*.sus|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                if (validExtensions.Contains(openFileDialog.FileName.Split('.')[openFileDialog.FileName.Split('.').Length - 1]))
                {
                    if (ChatRoom is PrivateChat)
                        try
                        {
                            await ChatRoom.Sender.SendFilePrivateSteam(((PrivateChat)ChatRoom).OtherUser.Ip, ChatRoom.Me.UserId, filePath);
                        }
                        catch { }
                    else
                    {
                        Groupchat groupchat = (Groupchat)ChatRoom;
                        for (int i = 0; i < groupchat.Participants.Count; i++)
                        {
                            try
                            {
                                await ChatRoom.Sender.SendFileGroupSteam(groupchat.Participants[i].Ip, ((Groupchat)ChatRoom).RoomId, ChatRoom.Me.UserId, filePath);
                            }
                            catch
                            {
                                //MessageBox.Show($"Could not send {((Groupchat)ChatRoom).Name} message to {groupchat.Participants[i].UserName}", "Could not send message");
                            }
                        }
                    }
                    ChatRoom.ChatHistory.Add(new Message(ChatRoom.Me, $"FILE: {filePath}"));
                }
                else
                    MessageBox.Show(Application.Current.FindResource("StrSendFileError").ToString(), Application.Current.FindResource("StrSendFileErrorTitle").ToString(), MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async void AddUserToChatroom(IPAddress addedUserIp)
        {
            try
            {
                await ChatRoom.Sender.AddToGroupchat(addedUserIp, ((Groupchat)ChatRoom).RoomId, ((Groupchat)ChatRoom).Name, ((Groupchat)ChatRoom).Picture, ((Groupchat)ChatRoom).Me.Ip, ((Groupchat)ChatRoom).Me.UserId, ((Groupchat)ChatRoom).Me.UserName, ((Groupchat)ChatRoom).Me.Picture);
            }
            catch { }
        }

        private bool CanAddMessage()
        {
            return !string.IsNullOrEmpty(this.CurrentMessage) && !string.IsNullOrWhiteSpace(this.CurrentMessage);
        }

        /// <summary>
        /// Adds the message the user sent
        /// and sends it to the others
        /// </summary>
        private async void AddMessage()
        {
            if (CurrentMessage.StartsWith('/'))
            {
                ChatCommandParser();
                return;
            }
            this.ChatRoom.ChatHistory.Add(new Message(ChatRoom.Me, currentMessage));
            MessageSentBringChatToTopHandler?.Invoke(this, EventArgs.Empty);
            if (this.ChatRoom is PrivateChat)
            {
                // call async SendPrivate
                try
                {
                    await ChatRoom.Sender.SendPrivateMessage(((PrivateChat)ChatRoom).OtherUser.Ip, ChatRoom.Me.UserId, CurrentMessage);
                }
                catch
                {
                    MessageBox.Show($"Could not send message to {((PrivateChat)ChatRoom).Name}", "Could not send message");
                }
            }
            else
            {
                Groupchat groupchat = (Groupchat)ChatRoom;
                string errors = "";
                for (int i = 0; i < groupchat.Participants.Count; i++)
                {
                    try
                    {
                        await ChatRoom.Sender.SendGroupMessage(groupchat.Participants[i].Ip, ChatRoom.Me.UserId, CurrentMessage, groupchat.RoomId);
                    }
                    catch
                    {
                        errors += "\n" + groupchat.Participants[i].UserName;
                    }
                }
                MessageBox.Show($"Could not send from {((Groupchat)ChatRoom).Name} message to {errors}", "Could not send message");
            }
            this.CurrentMessage = "";
        }

        private void ChatCommandParser()
        {
            string command = CurrentMessage[1..];
            switch (command.ToLower())
            {
                case "leave":
                    LeaveGroup();
                    break;
                case "add":
                    AddUserToGroupchat();
                    break;
                case "participants":
                case "members":
                    ListGroupchatParticipants();
                    break;
                case string c when c.StartsWith("id"):
                    if (c == "id")
                        CurrentMessage = ChatRoom.Me.UserId.ToString();
                    else
                    {
                        string name = c.Split(' ')[1];
                        if (ChatRoom is Groupchat groupchat)
                        {
                            foreach (User user in groupchat.Participants)
                                if (user.UserName == name)
                                {
                                    CurrentMessage = user.UserId.ToString();
                                    break;
                                }
                        }
                    }
                    break;
                case string c when c.StartsWith("ip"):
                    if (c == "ip")
                        CurrentMessage = ChatRoom.Me.Ip.ToString();
                    else
                    {
                        string name = c.Split(' ')[1];
                        if (ChatRoom is Groupchat groupchat)
                        {
                            foreach (User user in groupchat.Participants)
                                if (user.UserName == name)
                                {
                                    CurrentMessage = user.Ip.ToString();
                                    break;
                                }
                        }

                    }
                    break;
                case string c when c.StartsWith("sus"):
                    if (c == "sus")
                        CurrentMessage = "░██████╗██╗░░░██╗░██████╗ \n" +
                                         "██╔════════╝ ██║░░░██║██╔══════╝ \n" +
                                         "░╚█████╗ ██║░░░██║╚█████╗░ \n" +
                                         "░░╚════██╗██║░░░██║░╚═════██╗ \n" +
                                         "██████╔╝╚██████╔╝██████╔╝ \n" +
                                         "╚══════════╝░░╚═════════╝░╚═════════╝░";
                    break;
                case string c when c.StartsWith("amogus"):
                    c = c.Replace(" ", "");
                    if (c == "amogus")
                        CurrentMessage = "ඞ";
                    else if (c.StartsWith("amogusl"))
                        CurrentMessage = 
                            "                                  ⬛⬛⬛⬛⬛⬛ \n" +
                            "                                ⬛🟥🟥🟪🟪🟪🟥🟥⬛ \n" +
                            "                              ⬛🟥🟥🟥🟥🟥🟥🟥🟥⬛ \n" +
                            "                           ⬛🟥🟥🟥🟥⬛⬛⬛⬛⬛⬛ \n" +
                            "                           ⬛🟥🟥🟥⬛🟪🟪🟪🟪🟪🟪🟪🟪⬛ \n" +
                            "                           ⬛🟥🟥⬛🟪🟪🟪🟪🟪🟪🟪🟪🟪⬛ \n" +
                            "                      ⬛⬛⬛🟥🟥⬛🟪🟪🟪🟪🟪🟪🟪🟪🟪⬛ \n" +
                            "                     ⬛🟥🟥⬛🟥🟥⬛🟪🟪🟪🟦🟦🟦🟦🟪⬛ \n" +
                            "                     ⬛🟥🟥⬛🟥🟥🟥⬛🟪🟪🟪🟪🟪🟪⬛ \n" +
                            "                     ⬛🟥🟥⬛🟥🟥🟥🟥⬛⬛⬛⬛⬛ \n" +
                            "                     ⬛🟥🟥⬛🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥⬛ \n" +
                            "                     ⬛🟥🟥⬛🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥⬛ \n"+
                            "                     ⬛🟥🟥⬛🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥⬛ \n"+
                            "                     ⬛🟥🟥⬛🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥⬛ \n"+
                            "                     ⬛🟥🟥⬛🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥⬛ \n"+
                            "                     ⬛🟥🟥⬛🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥⬛ \n" +
                            "                      ⬛⬛⬛🟥🟥🟥🟥🟥🟥🟥🟥🟥🟥⬛ \n" +
                            "                         ⬛🟥🟥🟥⬛⬛⬛⬛🟥🟥🟥⬛ \n" +
                            "                        ⬛🟥🟥🟥⬛        ⬛🟥🟥🟥⬛ \n" +
                            "                        ⬛🟥🟥🟥⬛        ⬛🟥🟥🟥⬛ \n" +
                            "                         ⬛⬛⬛            ⬛⬛⬛";
                            break;
            }
        }
    }
}