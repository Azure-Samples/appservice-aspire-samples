using System.Text;
using System.Text.Json;

namespace AspireWithOpenAI.Web;

public class ChatApiClient(HttpClient httpClient)
{
    public async Task<Message> GetChatResponse(ChatRequest chatRequest)
    {
        const string genericErrorMessage = "I'm currently unable to process your request. Please try again shortly.";
        const string emptyResponseMessage = "I couldn't generate a reply this time. Please try again or rephrase your question.";

        try
        {
            string chatRequestJson = JsonSerializer.Serialize(chatRequest);

            using var httpContent = new StringContent(chatRequestJson, Encoding.UTF8, "application/json");
            var httpResponse = await httpClient.PostAsync("/chat", httpContent);

            if (!httpResponse.IsSuccessStatusCode)
            {
                return new Message { IsAssistant = true, Content = genericErrorMessage };
            }

            var message = await httpResponse.Content.ReadFromJsonAsync<Message>();

            return message?.Content is null
                ? new Message { IsAssistant = true, Content = emptyResponseMessage }
                : message;
        }
        catch (Exception)
        {
            return new Message { IsAssistant = true, Content = genericErrorMessage };
        }
    }
}

public record ChatRequest(List<Message> Messages);

public class Message
{
    public required bool IsAssistant { get; set; }
    public required string Content { get; set; }
}