using FluentValidation;
using SetTheDate.Models;

public class EventAnswerModelValidator : AbstractValidator<EventAnswerModel>
{
    public EventAnswerModelValidator()
    {
        RuleFor(x => x.Answer)
            .NotEmpty().WithMessage("Answer cannot be empty.")
            .MaximumLength(500).WithMessage("Answer cannot exceed 500 characters.");

    }
}
