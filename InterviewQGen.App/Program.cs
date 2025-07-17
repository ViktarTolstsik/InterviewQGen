using Microsoft.Extensions.Configuration;
using InterviewQGen.App.Services;

// Load configuration from environment variables
var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

try
{
    // Prompt for job description
    Console.Write("Enter a job description or URL: ");
    var jobDescription = Console.ReadLine()?.Trim();

    if (string.IsNullOrEmpty(jobDescription))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: Job description cannot be empty.");
        Console.ResetColor();
        return;
    }

    // Create question generator
    var questionGenerator = new LocalAIQuestionGenerator("");

    // Generate questions
    Console.WriteLine("\nGenerating interview questions...\n");
    var questions = await questionGenerator.GenerateQuestionsAsync(jobDescription);

    Console.WriteLine("Interview Questions:");
    foreach (var question in questions)
    {
        Console.WriteLine($"• {question}");
    }

    // Ask for example answers
    Console.Write("\nDo you want example answers? (y/n): ");
    var wantAnswers = Console.ReadLine()?.Trim().ToLower();

    if (wantAnswers == "y" || wantAnswers == "yes")
    {
        Console.WriteLine("\nGenerating questions with example answers...\n");
        var questionsWithAnswers = await questionGenerator.GenerateQuestionsWithAnswersAsync(jobDescription);

        Console.WriteLine("Questions with Example Answers:");
        foreach (var (question, answer) in questionsWithAnswers)
        {
            Console.WriteLine($"• Question: {question}");
            Console.WriteLine($"  Answer: {answer}\n");
        }
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Error: {ex.Message}");
    Console.ResetColor();
}
