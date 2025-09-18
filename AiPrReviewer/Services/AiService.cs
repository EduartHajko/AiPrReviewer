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
        Reviewer comment: {comment}

        Original code snippet:
        ```
        {code}
        ```

        ✅ Task: Apply ONLY the requested fix.
        ⚠️ Output rules:
        - Return ONLY the exact line(s) that must be changed or added.
        - Do NOT repeat the full method or unrelated code.
        - If no change is needed, reply with "NO CHANGE".
        """;

            var response = await _chatClient.CompleteChatAsync(
                [
                    ChatMessage.CreateUserMessage(prompt)
                ]
            );

            return response.Value.Content[0].Text.Trim();
        }

    }
}
