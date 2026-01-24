using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace MovieDirectoryTgBot
{
    /// <summary>
    /// Класс обработчик системных ошибок
    /// </summary>
    public class HandlerSystemsError
    {
        /// <summary>
        /// Обработчик ошибок связанных с сообщениями
        /// </summary>
        public Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
