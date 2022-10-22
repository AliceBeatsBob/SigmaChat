// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using Sigma.Commands;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Sigma.Viewmodels
{
    public class AddUserToGroupChatViewModel : BaseViewModel
    {
        #region Fields
        private string ip = SetIpField();
        private bool addButtonEnabled;
        #endregion

        #region DelegateCommands
        public DelegateCommand AddUserCommand { get; set; }
        #endregion

        #region Properties
        public string IpField
        {
            get { return ip; }
            set
            {
                ip = value;
                RaisePropertyChanged();
                AddButtonEnabled = !String.IsNullOrEmpty(IpField) && IpField.Split('.')[3] != "0" && IpField.Split('.')[3] != "255";
            }
        }

        public bool AddButtonEnabled
        {
            get { return addButtonEnabled; }
            set
            {
                addButtonEnabled = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public AddUserToGroupChatViewModel()
        {
            AddUserCommand = new DelegateCommand(
                _ =>
                {
                    return IpField.Split('.')[3] != "0" && IpField.Split('.')[3] != "255";
                },
                _ =>
                {

                });
        }

        /// <summary>
        /// Returns the part of the ip which is constant because of the subnetmask
        /// </summary>
        /// <returns>A string of the constant part of an ip</returns>
        static string SetIpField()
        {
            string ip = App.GetAllLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Wireless80211).FirstOrDefault() ??
                        App.GetAllLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Ethernet).FirstOrDefault();
            if (ip != null)
            {
                string mask = GetSubnetMask(ip);
                if (mask != null)
                {
                    string setIp = "";
                    string[] ipParts = ip.Split('.');
                    string[] maskParts = mask.Split('.');
                    for (int i = 0; i < maskParts.Length; i++)
                    {
                        if (maskParts[i] == "255")
                        {
                            setIp += ipParts[i] + ".";
                        }
                        else
                        {
                            for (int c = i; c < maskParts.Length; c++)
                                setIp += "0.";
                            return setIp.Substring(0, setIp.Length - 1);
                        }
                    }
                }
            }
            return "0.0.0.0";
        }

        /// <summary>
        /// Gets the subnet mask from the host computer and returns it
        /// </summary>
        /// <param name="ip">The ip address so the method knows which network adapter to get the subnet mask from</param>
        /// <returns>The subnet mask as a string</returns>
        static string GetSubnetMask(string ip)
        {
            NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface Interface in Interfaces)
            {
                if (Interface.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                Console.WriteLine(Interface.Description);
                UnicastIPAddressInformationCollection UnicastIPInfoCol = Interface.GetIPProperties().UnicastAddresses;
                foreach (UnicastIPAddressInformation UnicatIPInfo in UnicastIPInfoCol)
                {
                    if (UnicatIPInfo.Address.ToString() == ip)
                    {
                        return UnicatIPInfo.IPv4Mask.ToString();
                    }
                }
            }
            return null;
        }
    }
}