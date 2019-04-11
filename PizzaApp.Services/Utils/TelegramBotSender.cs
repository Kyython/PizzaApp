using PizzaApp.Services.Abstract;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace PizzaApp.Services.Utils
{
    public class TelegramBotSender: Sender
    {
        private static readonly string token = "638892701:AAFuc9Da_o0-JQW3W1hc7J2sli-i6PAW_gI";

        private readonly TelegramBotClient telegramBot = new TelegramBotClient(token);

        public void SendSms()
        {
            telegramBot.OnMessage += MessageReceived;
            telegramBot.OnMessageEdited += MessageReceived;

            telegramBot.StartReceiving(Array.Empty<UpdateType>());
            Console.ReadLine();
            telegramBot.StopReceiving();
        }

        private async void MessageReceived(object sender, MessageEventArgs e)
        {
            switch (e.Message.Text)
            {
                case "/getcode":
                    await telegramBot.SendTextMessageAsync(e.Message.Chat.Id, "Код подтверждения: " + Message);
                    break;
                case "/start":
                    await telegramBot.SendTextMessageAsync(e.Message.Chat.Id, @"/getcode-получение кода подтверждения для регистрации");
                    break;
            }
        }
    }
}
