// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Collaborators:
//  ඞ Hackl Tobias
//  ඞ Ratzenböck Peter

using System;
using System.Net;
using System.Xml.Serialization;

namespace Sigma.Models
{
    public class User : BaseNotifyPropertyChanged
    {
        public User()
        {
            
        }
        private string userName;

        private string picture;

        private long userId;

        private IPAddress ip;

        [XmlElement("SerializableIp")]
        public string SerializableIp
        {
            get { return ip.ToString(); }
            set { ip = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value); }
        }

        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                if (value != userName)
                {
                    userName = value;
                    RaisePropertyChanged();
                }
            }
        }

        [XmlIgnore]
        public IPAddress Ip
        {
            get
            {
                return ip;
            }
            set
            {
                if (value != ip)
                {
                    ip = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Picture
        {
            get
            {
                return picture;
            }
            set
            {
                if (value != picture)
                {
                    picture = value;
                    RaisePropertyChanged();
                }
            }
        }

        [XmlIgnore]
        public long UserId { get => userId; }
        [XmlElement("SerializableId")]
        public string SerializableId
        {
            get { return UserId.ToString(); }
            set { userId = long.Parse(value); }
        }

        public User(IPAddress ip, string userName, string picture, long id)
        {
            Ip = ip;
            UserName = userName;
            Picture = picture;
            userId = id;
        }
    }
}