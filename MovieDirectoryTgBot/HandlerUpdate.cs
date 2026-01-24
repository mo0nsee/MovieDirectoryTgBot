using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MovieDirectoryTgBot
{
    /// <summary>
    /// Обработчик обновление в чате (сообщений, команд и тд)
    /// </summary>
    public class HandlerUpdate
    {
        public async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Обязательно ставим блок try-catch, чтобы наш бот не "падал" в случае каких-либо ошибок
            try
            {
                // Сразу же ставим конструкцию switch, чтобы обрабатывать приходящие Update
                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            switch (update.Message.Type)
                            {
                                case MessageType.Text:
                                {
                                    HandlerText(update.Message);
                                    return;
                                }
                                default:
                                {
                                    await Program.botClient.SendMessage(
                                        update.Message.Chat.Id,
                                        "Используй только текст!");
                                    return;
                                }
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Обработка сообщений в виде текст
        /// </summary>
        /// <returns></returns>
        private async Task HandlerText(Message message)
        {
            //Создание клавиатуры при запуске бота
            if (message.Text == "/start")
            {
                HandlerText handlerText = new HandlerText();
                await Program.botClient.SendMessage(message.Chat.Id, $"Приветствую, рад познакомиться с тобой {message.Chat.Username}", replyMarkup: handlerText.StartBot());
            }
        }
    }
}
