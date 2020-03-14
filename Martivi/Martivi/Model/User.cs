using Martivi.Models.Transaction;
using MartiviSharedLib.Models.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Martivi.Model
{
    public enum UserType
    {
        Admin,
        Client
    }
    public class User:PropertyChangedBase
    {
        public User()
        {
            UserAddresses = new ObservableCollection<UserAddress>();
        }
       

        private string _ProfileImageUrl { get; set; }
        public string ProfileImageUrl { get { return _ProfileImageUrl; } set { _ProfileImageUrl = value; OnPropertyChanged(); } }
        
        private int _UserId { get; set; }
        public int UserId { get { return _UserId; } set { _UserId = value; OnPropertyChanged(); } }

        private UserType _Type { get { return _Type; } set { _Type = value; OnPropertyChanged(); } }
        public UserType Type { get; set; }

        private string _FirstName { get { return _FirstName; } set { _FirstName = value; OnPropertyChanged(); } }
        public string FirstName { get; set; }

        private string _LastName { get; set; }
        public string LastName { get { return _LastName; } set { _LastName = value; OnPropertyChanged(); } }

        private string _Username { get; set; }
        public string Username { get { return _Username; } set { _Username = value; OnPropertyChanged(); } }

        public string Password { get; set; }

        private string _Phone { get; set; }
        public string Phone { get { return _Phone; } set { _Phone = value; OnPropertyChanged(); } }

        

        public string Token { get; set; }

        public virtual ICollection<ChatMessage> Messages { get; set; }



        private ObservableCollection<UserAddress> _UserAddresses;
        public ObservableCollection<UserAddress> UserAddresses
        {
            get { return _UserAddresses; }
            set { _UserAddresses = value; OnPropertyChanged(); }
        }




    }
}
