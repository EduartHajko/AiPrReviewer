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

                Code snippet:
                ```
                {code}
                ```

                Please fix the code according to the reviewer’s comment.
                Return only the corrected code snippet.
                """;

            // Call OpenAI using CompleteChatAsync
            var response = await _chatClient.CompleteChatAsync(
                                   [
                                    ChatMessage.CreateUserMessage(prompt)
                                   ]
                               );

            //Extract the message text
            return response.Value.Content[0].Text;
        }
    }
}
