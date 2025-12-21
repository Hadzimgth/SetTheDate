using FluentValidation;
using PhoneNumbers;
using SetTheDate.Models;

public class ContactInformationModelValidator : AbstractValidator<ContactInformationModel>
{
    public ContactInformationModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.FamilyRole)
            .NotEmpty().WithMessage("Family role is required.")
            .MaximumLength(50).WithMessage("Family role cannot exceed 50 characters.");

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
