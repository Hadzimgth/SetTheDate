using FluentValidation;
using SetTheDate.Models;

public class EventQuestionModelValidator : AbstractValidator<EventQuestionModel>
{
    public EventQuestionModelValidator()
    {
        RuleFor(x => x.Question)
            .NotEmpty().WithMessage("Question is required.")
            .MaximumLength(500).WithMessage("Question cannot exceed 500 characters.");

        //RuleForEach(x => x.EventAnswerModels)
        //    .SetValidator(new EventAnswerModelValidator());
    }
}
