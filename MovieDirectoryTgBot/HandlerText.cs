using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace MovieDirectoryTgBot
{
    /// <summary>
    /// Обработчик сообщений в виде текста
    /// </summary>
    public class HandlerText
    {
        /// <summary>
        /// Обработка текста /start
        /// </summary>
        public ReplyKeyboardMarkup StartBot()
        {
            var keyboard = new ReplyKeyboardMarkup
            (
                //Создание кнопок в клаиатуре
                new List<KeyboardButton[]>()
                {
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Выбрать случайный фильм")
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardButton("Вывести все фильмы")
                    }
                }
            )
            {
                // автоматическое изменение размера клавиатуры
                ResizeKeyboard = true,
            };

            return keyboard;
        }
    }
}
