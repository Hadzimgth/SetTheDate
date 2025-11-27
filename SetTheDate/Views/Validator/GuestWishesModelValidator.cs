using FluentValidation;
using SetTheDate.Models;

public class GuestWishesModelValidator : AbstractValidator<GuestWishesModel>
{
    public GuestWishesModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Wish)
            .NotEmpty().WithMessage("Wish message is required.")
            .MaximumLength(1000).WithMessage("Wish cannot exceed 1000 characters.");
    }
}
