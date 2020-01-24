﻿using MartiviSharedLib.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martivi.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string UserAddress { get; set; }
        public string Token { get; set; }
        public virtual ICollection<ChatMessage> Messages { get; set; }
    }
}