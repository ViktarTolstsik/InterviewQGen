# InterviewQGen

A modern, fully local interview question generator made entirely via Cursor with a Blazor Server frontend and .NET backend, powered by a local LLM (Ollama). No OpenAI or paid APIs required.

## Features
- Generate relevant interview questions based on your job description
- Stylish, modern Blazor Server UI
- 100% local: no data leaves your machine
- Uses Ollama with the Gemma 3B model (or any compatible local LLM)

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Ollama](https://ollama.com/) (for local LLM)
- (Recommended) Visual Studio 2022+ or VS Code

## Getting Started

### 1. Clone the repository
```sh
git clone https://github.com/YOUR_USERNAME/InterviewQGen.git
cd InterviewQGen
```

### 2. Set up Ollama and the LLM
- [Install Ollama](https://ollama.com/download)
- Pull the Gemma 3B model (or your preferred model):
  ```sh
  ollama pull gemma:3b
  ```
- Start Ollama (it usually runs automatically on install):
  ```sh
  ollama serve
  ```

### 3. Build and run the app
```sh
dotnet build
# Run the Blazor Server frontend
cd InterviewQGen.Web
dotnet run
```

The app will be available at `https://localhost:5001` (or the port shown in the console).

## Usage
1. Enter a job description in the textarea.
2. Click "Generate Questions".
3. Wait for the progress bar to complete.
4. View the generated questions as stylish cards.

## Project Structure
- `InterviewQGen.App/` - .NET backend logic (question generation, LLM integration)
- `InterviewQGen.Web/` - Blazor Server frontend
- `.gitignore` - Excludes build artifacts, user files, and secrets
- `README.md` - This file

## Customization
- To use a different local LLM, update the Ollama model name in the backend service.
- Prompt engineering can be adjusted in the backend for different question styles.

## License
MIT

---
