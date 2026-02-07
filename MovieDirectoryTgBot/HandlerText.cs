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
    private ReplyKeyboardMarkup _keyboardStart;

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
        _keyboardStart = new ReplyKeyboardMarkup
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
                    new KeyboardButton("Вывести все фильмы определенного жанра")
                }
            }
        )
        {
            // автоматическое изменение размера клавиатуры
            ResizeKeyboard = true,
        };

        return _keyboardStart;
    }
    /// <summary>
    /// Обработка определенных текстовых команд
    /// </summary>
    public async Task HandlerCommand()
    {
        if (Program.UserStates[_message.Chat.Id] == UserState.None)
        {
            //Создание клавиатуры при запуске бота
            if (_message.Text == "/start")
                await HandlerStart();
            else if (_message.Text == "Выбрать случайный фильм")
                await RandomMovie();
            else if (_message.Text == "Вывести все фильмы определенного жанра")
                await SelectButtonGenreListMovie();
        }
        else if(Program.UserStates[_message.Chat.Id] == UserState.WaitingForGenre)
        {
            await ShowGenreListMovie();
        }
    }
    /// <summary>
    /// Обработка команды "Start"
    /// </summary>
    private async Task HandlerStart()
    {
        await Program.BotClient.SendMessage(_message.Chat.Id, $"Приветствую, рад познакомиться с тобой {_message.Chat.Username}", replyMarkup: StartBot());
    }
    /// <summary>
    /// Выбор случайного фильма
    /// </summary>
    private async Task RandomMovie()
    {
        Random _random = new Random();
        var randomMovie = Program.ListMovie[_random.Next(0, Program.ListMovie.Count())];
        await Program.BotClient.SendMessage(_message.Chat.Id, $"Случайный фильм:\n{randomMovie.TitleRu}\n{randomMovie.TitleOriginal}\n{randomMovie.DateYear}");
    }
    /// <summary>
    /// Выбор пункта списка фильмов по жанрам
    /// </summary>
    /// <returns></returns>
    private async Task SelectButtonGenreListMovie()
    {
        var _keyboardGenreMovie = new ReplyKeyboardMarkup
        (
            //Создание кнопок в клаиатуре
            new List<KeyboardButton[]>()
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("Отменить действие")
                }
            }
        )
        {
            // автоматическое изменение размера клавиатуры
            ResizeKeyboard = true,
        };
        await Program.BotClient.SendMessage(_message.Chat.Id, $"Введите нужный жанр", replyMarkup: _keyboardGenreMovie);
        Program.UserStates[_message.Chat.Id] = UserState.WaitingForGenre;
    }
    /// <summary>
    /// Вывод списка фильмов определенных жанров
    /// </summary>
    /// <returns></returns>
    private async Task ShowGenreListMovie()
    {
        if (_message.Text == "Отменить действие")
            await Program.BotClient.SendMessage(_message.Chat.Id, $"Действие отменено", replyMarkup: StartBot());
        else
        {
            await SelectGenre(_message.Text);
        }

        Program.UserStates[_message.Chat.Id] = UserState.None;
    }
    /// <summary>
    /// Выбор жанра
    /// </summary>
    private async Task SelectGenre(string genre)
    {
        if (!string.IsNullOrWhiteSpace(genre))
        {
            var listMovie = Program.ListMovie.Where(movie => movie.Genre.ToLower().Contains(genre.ToLower().Trim())).ToList();
            if (listMovie.Count == 0)
            {
                await Program.BotClient.SendMessage(_message.Chat.Id, $"Действие отменено", replyMarkup: StartBot());
                return;
            }
            else
            {
                await SendFileGenreListMovie(genre, listMovie);
                await Program.BotClient.SendMessage(_message.Chat.Id, $"Файл со списком фильмов отправлен", replyMarkup: StartBot());
            }
        }
        else
            await Program.BotClient.SendMessage(_message.Chat.Id, $"Действие отменено", replyMarkup: StartBot());
    }
    /// <summary>
    /// Отправка файла с фильмом одного жанра
    /// </summary>
    private async Task SendFileGenreListMovie(string genre, List<Movie> listMovie)
    {
        //Формирование данных
        string resultList = "";
        foreach (var movie in listMovie)
        {
            resultList += $"Название:{movie.TitleRu}\\{movie.TitleOriginal} Дата выхода:{movie.DateYear}\n";
        }
        if (resultList[resultList.Length - 1] == '\n')
            resultList = resultList.Remove(resultList.Length - 1);

        await SendFileTXT(resultList, genre);
    }

    /// <summary>
    /// Отправка текстового файла
    /// </summary>
    private async Task SendFileTXT(string content, string fileName)
    {
        // Сохраняем в временный файл
        string tempPath = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempPath, content);
        try
        {
            using (var stream = File.OpenRead(tempPath))
            {
                var inputFile = InputFile.FromStream(stream, $"{fileName}.txt");
                await Program.BotClient.SendDocument(_message.Chat.Id, inputFile);
            }
        }
        finally
        {
            File.Delete(tempPath);
        }
    }
}