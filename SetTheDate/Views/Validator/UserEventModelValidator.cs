using FluentValidation;
using SetTheDate.Models;

public class UserEventModelValidator : AbstractValidator<UserEventModel>
{
    public UserEventModelValidator()
    {
        RuleFor(x => x.EventName)
            .NotEmpty().WithMessage("Event name is required.")
            .MaximumLength(200).WithMessage("Event name is too long.");

        RuleFor(x => x.EventDescription)
            .MaximumLength(500).WithMessage("Event description is too long.");

        RuleFor(x => x.EventDate)
            .NotEmpty().WithMessage("Event date is required.")
            .GreaterThan(DateTime.MinValue).WithMessage("Invalid event date.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(DateTime.MinValue).WithMessage("Invalid end date.");

        RuleFor(x => x.GroomName)
            .NotEmpty().WithMessage("Groom name is required.");

        RuleFor(x => x.BrideName)
            .NotEmpty().WithMessage("Bride name is required.");

        RuleFor(x => x.GroomFatherName)
            .NotEmpty().WithMessage("Groom father name is required.");

        RuleFor(x => x.GroomMotherName)
            .NotEmpty().WithMessage("Groom mother name is required.");

        RuleFor(x => x.BrideFatherName)
            .NotEmpty().WithMessage("Bride father name is required.");

        RuleFor(x => x.BrideMotherName)
            .NotEmpty().WithMessage("Bride mother name is required.");

        RuleFor(x => x.LocationName)
            .NotEmpty().WithMessage("Venue name is required.");

        RuleFor(x => x.Address1)
            .NotEmpty().WithMessage("Address line 1 is required.");

        RuleFor(x => x.Address2)
            .NotEmpty().WithMessage("Address line 2 is required.");

        RuleFor(x => x.Address3)
            .NotEmpty().WithMessage("Address line 3 is required.");

        RuleFor(x => x.Postcode)
            .NotEmpty().WithMessage("Postcode is required.");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required.");

        //RuleFor(x => x.WeddingCardType)
        //    .GreaterThan(0).WithMessage("Wedding card type is required.");

        //RuleFor(x => x.EventImageAttachmentId)
        //    .GreaterThan(0).WithMessage("Event image is required.");

        // Contact information list validation - at least one contact person is required
        //RuleFor(x => x.ContactInformations)
        //    .NotEmpty().WithMessage("At least one contact person is required.");

        RuleForEach(x => x.ContactInformations)
            .SetValidator(new ContactInformationModelValidator());

        //// Event images list validation
        //RuleForEach(x => x.EventImages)
        //    .SetValidator(new EventImageAttachmentModelValidator());
    }
}
