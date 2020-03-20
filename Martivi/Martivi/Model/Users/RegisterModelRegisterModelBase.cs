
namespace MartiviSharedLib.Models.Users
{
    public class RegisterModelBase
    {
        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Username { get; set; }

        public virtual string Password { get; set; }

        public string Phone { get; set; }
    }
}