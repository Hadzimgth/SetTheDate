using FluentValidation;
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
            .Matches(@"^\+?[0-9\s\-]{7,20}$")
            .WithMessage("Phone number format is invalid.");

    }
}
