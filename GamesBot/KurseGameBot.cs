using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Exceptions;
using Newtonsoft.Json;
using GamesBot.Client;

namespace GamesBot
{
    public class KurseGameBot
    {
        GameCLient client0 = new GameCLient();
        GameCLient client1 = new GameCLient();
        GameCLient client2 = new GameCLient();
        TelegramBotClient botClient = new TelegramBotClient("5397917539:AAGGmNVeI8nNaErohR8toFe9m4ZEz-3jXok");
        CancellationToken cancellationToken = new CancellationToken();
        ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } };
        public async Task Start()
        {
            botClient.StartReceiving(HandlerUpdateAsync, HandlerError, receiverOptions, cancellationToken);
            var botMe = await botClient.GetMeAsync();
            Console.WriteLine($"Бот {botMe.Username} почав працювати");
            Console.ReadKey();
        }

        private Task HandlerError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Помилка в телеграм бот АПІ: \n {apiRequestException.ErrorCode}" +
                $"\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                await HandlerMessageAsync(botClient, update.Message);
            }
        }

        private async Task HandlerMessageAsync(ITelegramBotClient botClient, Message message)
        {
            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Виберіть команду /keyboard");
                return;
            }
                
            ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                    new[]
                        {
                        new KeyboardButton [] { "/Інформація ігри по ID", "/Вивести список ігор"},
                        new KeyboardButton [] { "/Залишити відгук", "/Видалити всі відгуки та залишити новий" },
                        new KeyboardButton [] { "/Видалити відгуки" }
                        }
                    )
                {
                    ResizeKeyboard = true
                };

            switch (message.Text)
            {
                case "/Інформація ігри по ID":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "/ID Завантажено!");

                    var client0 = new GameCLient();
                    client0.GameInformationById();

                    var ggame = GameCLient.result;
                        await botClient.SendPhotoAsync(message.Chat.Id, ggame.Data.Header_Image, ggame.Data.Name);
                    break;


                case "/Вивести список ігор":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Список різних ігор: ");

                    var _client = new GameCLient();
                    _client.GameList();



                    foreach (var game in GameCLient.Result)
                    {
                        await botClient.SendPhotoAsync(message.Chat.Id, game.Data.Header_Image, game.Data.Name);
                    }
                    break;

                case "/Залишити відгук":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Ваш відгук завантажено");
                    client1.Postcomment();
                    break;

                case "/Видалити всі відгуки та залишити новий":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Відгуки видалені. Ваш відгук завантажено ");
                    
                    client2.Putcomment();

                    break;

                case "/Видалити відгуки":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Всі відгуки видалені");
                    GameCLient client3 = new GameCLient();
                    client3.Deletecomments();
                    break;

                case "/keyboard":
                    break;

                default:
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Я Вас не зрозумів! \n");
                    break;
            }
            await botClient.SendTextMessageAsync(message.Chat.Id, "Виберіть пункт меню:", replyMarkup: replyKeyboardMarkup);
        }

    }
}