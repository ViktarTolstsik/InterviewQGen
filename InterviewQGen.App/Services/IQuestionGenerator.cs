namespace InterviewQGen.App.Services;

public interface IQuestionGenerator
{
    Task<IReadOnlyList<string>> GenerateQuestionsAsync(string vacancyDescription);
    Task<IReadOnlyList<(string Question, string ExpectedAnswer)>> GenerateQuestionsWithAnswersAsync(string vacancyDescription);
}
