using FluentValidation;
using SetTheDate.Models;

public class EventSurveySetupValidator : AbstractValidator<EventSurveySetup>
{
    public EventSurveySetupValidator()
    {
        RuleFor(x => x.EventQuestionListModel)
            .NotEmpty().WithMessage("At least one question is required.");

        RuleForEach(x => x.EventQuestionListModel)
            .Must(question =>
            {
                // Initialize if null (shouldn't happen after controller filtering, but be safe)
                if (question.EventAnswerListModel == null)
                {
                    question.EventAnswerListModel = new List<EventAnswerModel>();
                }

                // Count only non-empty answers
                int nonEmptyAnswerCount = question.EventAnswerListModel
                    .Count(answer => answer != null && !string.IsNullOrWhiteSpace(answer.Answer));

                // If question has no text, it shouldn't have answers
                if (string.IsNullOrWhiteSpace(question.Question))
                {
                    // Empty question with no answers is OK (might be filtered out)
                    // But empty question with answers is invalid
                    return nonEmptyAnswerCount == 0;
                }

                // Question has text - that's valid (answers are validated in next rule)
                return true;
            })
            .WithMessage("A question with answers must have question text.");

        RuleForEach(x => x.EventQuestionListModel)
            .Must(question =>
            {
                // Only validate questions that have text (empty questions are handled above)
                if (string.IsNullOrWhiteSpace(question.Question))
                {
                    return true;
                }

                // Initialize if null
                if (question.EventAnswerListModel == null)
                {
                    return false;
                }

                // Count only non-empty answers
                int nonEmptyAnswerCount = question.EventAnswerListModel
                    .Count(answer => answer != null && !string.IsNullOrWhiteSpace(answer.Answer));

                // Question with text must have at least 2 non-empty answers
                return nonEmptyAnswerCount >= 2;
            })
            .WithMessage("Each question must have at least 2 answers with text.");
    }
}