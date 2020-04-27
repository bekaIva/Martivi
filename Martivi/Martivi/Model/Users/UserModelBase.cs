namespace MartiviSharedLib.Models.Users
{
    public class PasswordChangeRequestModel
    {
        public int Code { get; set; }
        public string Username { get; set; }
        public string NewPassword { get; set; }
    }
    public class RequestPasswordRecoveryCodeModel
    {
        public string Username { get; set; }
    }

    public enum Result
    {
        PasswordChanged = 0,
        CodeSent = 1,
        InvalidCode = 2,
        CodeOutOfDated = 3,
        UserNotFound = 4,
        UnknownError = 5
    }
    public class PasswordChangeResult
    {
        public Result Error { get; set; }
        public string Message { get; set; }
    }
    public class UserModelBase
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string UserAddress { get; set; }
    }
}