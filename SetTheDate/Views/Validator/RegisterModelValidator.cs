using FluentValidation;
using PhoneNumbers;
using SetTheDate.Libraries.Services;
using SetTheDate.Models;

public class RegisterModelValidator : AbstractValidator<RegisterModel>
{
    private readonly UserService _userService;

    public RegisterModelValidator(UserService userService)
    {
        _userService = userService;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address format.")
            .MustAsync(async (email, cancellation) =>
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                var existingUser = (await _userService.GetAllUser())
                    .FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                return existingUser == null;
            }).WithMessage("Email has already been registered.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        RuleFor(x => x.Password)
            .Must(password => !string.IsNullOrWhiteSpace(password) && password.Any(char.IsUpper))
            .WithMessage("Password must contain at least one capital letter.")
            .When(x => !string.IsNullOrWhiteSpace(x.Password) && x.Password.Length >= 6);

        RuleFor(x => x.Password)
            .Must(password => !string.IsNullOrWhiteSpace(password) && password.Any(char.IsDigit))
            .WithMessage("Password must contain at least one number.")
            .When(x => !string.IsNullOrWhiteSpace(x.Password) && x.Password.Length >= 6);

        RuleFor(x => x.Password)
            .Must(password => !string.IsNullOrWhiteSpace(password) && password.Any(ch => !char.IsLetterOrDigit(ch)))
            .WithMessage("Password must contain at least one special character.")
            .When(x => !string.IsNullOrWhiteSpace(x.Password) && x.Password.Length >= 6);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm Password is required.")
            .Equal(x => x.Password).WithMessage("Passwords do not match.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Must(phoneNumber => ValidatePhoneNumber(phoneNumber))
            .WithMessage("Invalid phone number format. Please enter a valid mobile number.");
    }

    private bool ValidatePhoneNumber(string mobile)
    {
        if (string.IsNullOrWhiteSpace(mobile))
            return false;

        try
        {
            var phoneUtil = PhoneNumberUtil.GetInstance();
            var number = phoneUtil.Parse(mobile, "MY");

            return phoneUtil.IsValidNumber(number) &&
                   phoneUtil.GetNumberType(number) == PhoneNumberType.MOBILE;
        }
        catch (NumberParseException)
        {
            return false;
        }
    }
}
