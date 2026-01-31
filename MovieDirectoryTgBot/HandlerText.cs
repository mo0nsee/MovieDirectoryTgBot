using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MovieDirectoryTgBot;
/// <summary>
/// Обработчик сообщений в виде текста
/// </summary>
public class HandlerText : IHandlerText
{
    //Полученное сообщение
    private Message _message;

    public HandlerText(Message message)
    {
        _message = message;
    }

    /// <summary>
    /// Обработка текста /start
    /// </summary>
    private ReplyKeyboardMarkup StartBot()
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

    public async Task HandlerCommand()
    {
        //Создание клавиатуры при запуске бота
        if (_message.Text == "/start")
            await HandlerStart();
        else if (_message.Text == "Выбрать случайный фильм")
            await RandomMovie();

    }
    /// <summary>
    /// Обработка команды "Start"
    /// </summary>
    /// <returns></returns>
    private async Task HandlerStart()
    {
        await Program.BotClient.SendMessage(_message.Chat.Id, $"Приветствую, рад познакомиться с тобой {_message.Chat.Username}", replyMarkup: StartBot());
    }

    private async Task RandomMovie()
    {
        Random _random = new Random();
        var randomMovie = Program.ListMovie[_random.Next(0, Program.ListMovie.Count())];
        await Program.BotClient.SendMessage(_message.Chat.Id, $"Случайный фильм:\n{randomMovie.TitleRu}\n{randomMovie.TitleOriginal}\n{randomMovie.DateYear}");
    }
}