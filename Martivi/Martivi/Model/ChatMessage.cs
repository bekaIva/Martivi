using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martivi.Model
{
    public class RetryPolicy : IRetryPolicy
    {
        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            return TimeSpan.FromSeconds(3);
        }
    }
    public enum MessageSide
    {
        Support,
        Client
    }
    public enum TemplateType
    {
        IncomingText,
        OutGoingText,
    }
    public enum MessageTarget
    {
        TargetUser,
        Admin,
        Global
    }
    public class ChatMessage:PropertyChangedBase
    {
        public int ChatMessageId { get; set; }
        public int UserId { get; set; }
        public int TargetUser { get; set; }
        public MessageTarget Target { get; set; }



        private string text;
        private string profileImage;
        private string dateTime;
        private string username;
     
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        public string ProfileImage
        {
            get { return profileImage; }
            set
            {
                profileImage = value;
                OnPropertyChanged("ProfileImage");
            }
        }

        public string DateTime
        {
            get { return dateTime; }
            set
            {
                dateTime = value;
                OnPropertyChanged("DateTime");
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        public TemplateType TemplateType
        {
            get;
            set;
        }

        private string _OwnerProfileImage;

        public string OwnerProfileImage
        {
            get { return _OwnerProfileImage; }
            set { _OwnerProfileImage = value; OnPropertyChanged(); }
        }


    }
}
