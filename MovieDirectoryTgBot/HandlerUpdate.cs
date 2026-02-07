using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static MovieDirectoryTgBot.HandlerText;

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
                                    if(!Program.UserStates.ContainsKey(update.Message.Chat.Id))
                                        Program.UserStates.Add(update.Message.Chat.Id, UserState.None);
                                    HandlerText handlerText = new HandlerText(update.Message);
                                    await handlerText.HandlerCommand();
                                    return;
                                }
                                default:
                                {
                                    await Program.BotClient.SendMessage(
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
    }
}
