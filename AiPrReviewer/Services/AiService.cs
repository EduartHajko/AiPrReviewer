using OpenAI.Chat;

namespace AiPrReviewer.Services
{
    public class AiService
    {
        private readonly ChatClient _chatClient;

        public AiService(string apiKey)
        {
            // Initialize ChatClient with model + API key
            _chatClient = new ChatClient("gpt-4o", apiKey);
        }

        public async Task<string> SolveComment(string comment, string code)
        {
            var prompt = $"""
    Reviewer comment:
    {comment}

    Original code snippet:
    ```
    {code}
    ```

    🧠 Task:
    Apply the reviewer’s requested changes to the provided code.

    ⚙️ Output rules:
    - Return the full, corrected code snippet if multiple changes or structural modifications are needed.
    - If only a few lines must be modified, you may return just those updated lines.
    - Do NOT include explanations, markdown formatting, or commentary.
    - Preserve code style and indentation.
    - If no changes are needed, reply with exactly: NO CHANGE
    """;

            var response = await _chatClient.CompleteChatAsync(
            [
                ChatMessage.CreateUserMessage(prompt)
            ]);

            return response.Value.Content[0].Text.Trim();
        }

        public async Task<string> GetPerformanceAnalysis(string prompt)
        {
            var response = await _chatClient.CompleteChatAsync(
                [
                    ChatMessage.CreateUserMessage(prompt)
                ]
            );
            return response.Value.Content[0].Text;
        }


    }
}
