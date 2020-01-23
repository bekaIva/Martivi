namespace MartiviSharedLib.Models.Users
{
  public class UserModelBase
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string UserAddress { get; set; }
    }
}