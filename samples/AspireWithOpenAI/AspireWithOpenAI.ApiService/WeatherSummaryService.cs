using Microsoft.Extensions.AI;

namespace AspireWithOpenAI.ApiService
{
    public class WeatherSummaryService(IChatClient chatClient, ILogger<WeatherSummaryService> logger)
    {
        internal async Task<Message> GetWeatherSummary(ChatRequest request)
        {
            try
            {
                List<ChatMessage> history = CreateHistoryFromRequest(request);

                ChatResponse response = await chatClient.GetResponseAsync(history);

                var content = string.Join(" ", response.Messages);

                logger.LogInformation("Received chat response: {Response}", content);

                return new Message()
                {
                    IsAssistant = response.Messages[0].Role == ChatRole.Assistant,
                    Content = content
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during chat operation.");
                throw;
            }
        }

        private List<ChatMessage> CreateHistoryFromRequest(ChatRequest request) =>
        [
            new ChatMessage(ChatRole.System,
                """
                You are an AI assistant that classifies a single temperature reading.
                Task: Output EXACTLY ONE WORD chosen ONLY from this fixed list:
                Frigid, Freezing, Cold, Cool, Mild, Warm, Hot, Scorching
                If no valid temperature is found: output Undeterministic (exact spelling).

                Extraction:
                - Scan the latest user message (ignore earlier ones) for a numeric value (integer or decimal) optionally followed by °, a space, or directly followed by C, F, Celsius, or Fahrenheit (case-insensitive).
                - If both a Celsius and Fahrenheit value appear for the SAME reading (e.g. '51 C (123 F)'), prefer the Celsius number.
                - If multiple candidate numbers exist, select the one that has a temperature unit or symbol closest to it; if still ambiguous choose the LAST valid one.
                - Accept values only if they fall within realistic air temperature ranges:
                  Celsius: -100 to 80
                  Fahrenheit: -150 to 180
                - If the number has unit F / Fahrenheit convert to Celsius: (F - 32) * 5 / 9 (use decimal math). Round to nearest whole number before classification.

                Classification (based on Celsius value after any conversion):
                  c <= -10          => Frigid
                  -9 <= c <= -1    => Freezing
                  0  <= c <= 5      => Cold
                  6  <= c <= 14     => Cool
                  15 <= c <= 21     => Mild
                  22 <= c <= 27     => Warm
                  28 <= c <= 34     => Hot
                  c >= 35           => Scorching

                Output rules:
                - Output ONLY the selected word (no quotes, no punctuation, no explanation).
                - Never invent a word not in the list.
                - Do NOT output both Celsius and Fahrenheit or any numbers.
                - Ensure very high Celsius values like 40, 45, 50 classify as Scorching.

                Examples:
                Input: "Temp: -25 C"            => Frigid
                Input: "Today 15°C"             => Mild
                Input: "It will reach 72 F"     => Warm  (72 F ≈ 22 C)
                Input: "High 91F low 70F"       => Hot   (91 F ≈ 33 C; choose last valid => 70 F ≈ 21 C -> Mild; but highest? DO NOT pick highest; pick LAST => 70 F ≈ 21 C => Mild)
                Correction: Actually follow rule: pick LAST valid value -> 70 F => 21 C => Mild
                Input: "51 C (123 F)"           => Scorching
                Input: "No temperature listed"  => Undeterministic
                """),
            .. from message in request.Messages
               select new ChatMessage(message.IsAssistant ? ChatRole.Assistant : ChatRole.User, message.Content),
        ];
    }
}

public record ChatRequest(List<Message> Messages);
public class Message
{
    public required bool IsAssistant { get; set; }
    public required string Content { get; set; }
}