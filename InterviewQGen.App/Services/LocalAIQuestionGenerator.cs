using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;

namespace InterviewQGen.App.Services
{
    public class LocalAIQuestionGenerator : IQuestionGenerator
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string OllamaApiUrl = "http://localhost:11434/api/generate";
        private const string ModelName = "gemma3:1b";

        public LocalAIQuestionGenerator(string _)
        {
            // No API key needed for Ollama local
        }

        public async Task<IReadOnlyList<string>> GenerateQuestionsAsync(string vacancyDescription)
        {
            var prompt = "You are an HR assistant. Given the following job description, generate 7–10 meaningful interview questions. " +
                         "Return ONLY the questions, each on a new line, and prefix each question with 'Q: '. Do not include any explanations, introductions, or extra text."
                         + $"\nJob description: {vacancyDescription}";
            var request = new
            {
                model = ModelName,
                prompt = prompt,
                stream = false
            };
            var response = await _httpClient.PostAsJsonAsync(OllamaApiUrl, request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var content = doc.RootElement.GetProperty("response").GetString();
            var lines = content.Split('\n').Select(l => l.Trim()).Where(l => l.StartsWith("Q: ")).Select(l => l.Substring(3).Trim()).ToList();
            return lines;
        }

        public async Task<IReadOnlyList<(string Question, string ExpectedAnswer)>> GenerateQuestionsWithAnswersAsync(string vacancyDescription)
        {
            var prompt = $"You are an HR expert. Given the following job description, generate 5 interview questions with a short example answer for each.\nJob description: {vacancyDescription}";
            var payload = new
            {
                model = ModelName,
                prompt = prompt,
                stream = false
            };
            var response = await _httpClient.PostAsJsonAsync(OllamaApiUrl, payload);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var content = doc.RootElement.GetProperty("response").GetString();
            var result = new List<(string Question, string ExpectedAnswer)>();
            // Try to parse Q&A pairs
            var qaRegex = new Regex(@"(?:\d+[\).\s-]*)?\s*Question[:\-]?\s*(.+?)\s*Answer[:\-]?\s*(.+?)(?=(?:\n\d+[\).\s-]*\s*Question[:\-]?|\z))", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach (Match match in qaRegex.Matches(content))
            {
                var question = match.Groups[1].Value.Trim();
                var answer = match.Groups[2].Value.Trim();
                if (!string.IsNullOrWhiteSpace(question) && !string.IsNullOrWhiteSpace(answer))
                    result.Add((question, answer));
            }
            // Fallback: If regex fails, try to split by lines
            if (result.Count == 0)
            {
                var lines = content.Split('\n');
                for (int i = 0; i < lines.Length - 1; i++)
                {
                    var q = Regex.Replace(lines[i], @"^\s*\d+[\).\s-]*", "").Trim();
                    var a = Regex.Replace(lines[i + 1], @"^Answer[:\-]?\s*", "", RegexOptions.IgnoreCase).Trim();
                    if (!string.IsNullOrWhiteSpace(q) && !string.IsNullOrWhiteSpace(a))
                    {
                        result.Add((q, a));
                        i++;
                    }
                }
            }
            return result;
        }
    }
}
