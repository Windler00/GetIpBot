using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using System.Text.Json;

namespace GetIpBot
{
    public class BotClient
    {
        static readonly HttpClient httpClient = new HttpClient();
        public void StartPooling()
        {
            TelegramBotClient client = new TelegramBotClient($"{Init.config.ApiToken}");

            client.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync
            );
        }

        async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            if (messageText == "/getip")
            {
                HttpResponseMessage response = await httpClient.GetAsync("https://api.ipify.org?format=json");
                string responseBody = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(responseBody);
                var ip = json.RootElement.GetProperty("ip");
                foreach (var user in Init.UsersList)
                {
                    if(user.Id == chatId.ToString())
                    {
                        string ipString = ip.ToString();
                        Message sentMessage = await client.SendTextMessageAsync(
                            chatId: chatId,
                            text: ipString,
                            cancellationToken: cancellationToken);
                    }
                }
            }
        }

        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
