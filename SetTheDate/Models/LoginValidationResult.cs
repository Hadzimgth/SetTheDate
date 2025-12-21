namespace SetTheDate.Models
{
    public class LoginValidationResult
    {
        public UserModel? User { get; set; }
        public LoginErrorType ErrorType { get; set; }
        public bool IsSuccess => User != null && ErrorType == LoginErrorType.None;

        public static LoginValidationResult Success(UserModel user)
        {
            return new LoginValidationResult { User = user, ErrorType = LoginErrorType.None };
        }

        public static LoginValidationResult AccountNotFound()
        {
            return new LoginValidationResult { User = null, ErrorType = LoginErrorType.AccountNotFound };
        }

        public static LoginValidationResult WrongPassword()
        {
            return new LoginValidationResult { User = null, ErrorType = LoginErrorType.WrongPassword };
        }
    }

    public enum LoginErrorType
    {
        None,
        AccountNotFound,
        WrongPassword
    }
}

