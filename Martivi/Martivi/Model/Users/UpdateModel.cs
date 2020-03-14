using Martivi.Models.Transaction;
using System.Collections.Generic;

namespace MartiviSharedLib.Models.Users
{
  public class UpdateModel
    {
        public string ProfileImageUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public virtual ICollection<UserAddress> UserAddresses { get; set; }
    }
}