using Microsoft.EntityFrameworkCore;
using MovieDirectoryTgBot;
using System.Collections.ObjectModel;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static Telegram.Bot.TelegramBotClient;

public class Program
{
    public static MoviesApplicationContext ConnectDB = new MoviesApplicationContext();
    /// <summary>
    /// Список фильмов подгруженных с бд
    /// </summary>
    public static ObservableCollection<Movie> ListMovie
    {
        get; set;
    }

    // Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
    public static ITelegramBotClient BotClient;

    // Это объект с настройками работы бота. Здесь мы будем указывать, какие типы Update мы будем получать, Timeout бота и так далее.
    private static ReceiverOptions _receiverOptions;

    static async Task Main()
    {
        // Присваиваем нашей переменной значение, в параметре передаем Token, полученный от BotFather
        BotClient = new TelegramBotClient("8245403560:AAGaMLRZlTq49iw5o68BiA0-dBjPgJFXsh8"); 
        _receiverOptions = new ReceiverOptions // Также присваем значение настройкам бота
        {
            AllowedUpdates = new[] // Тут указываем типы получаемых Update`ов, о них подробнее расказано тут https://core.telegram.org/bots/api#update
            {
                UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
                UpdateType.CallbackQuery // Кнопка под чатом (Inline кнопка)
            },
            // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
            // True - не обрабатывать, False (стоит по умолчанию) - обрабаывать
            DropPendingUpdates = true,
        };

        using var cts = new CancellationTokenSource();

        HandlerUpdate handlerUpdate = new HandlerUpdate();
        HandlerSystemsError handlerSystemsError = new HandlerSystemsError();
        BotClient.StartReceiving(handlerUpdate.UpdateHandler, handlerSystemsError.ErrorHandler, _receiverOptions, cts.Token); // Запускаем бота

        var me = await BotClient.GetMe(); // Создаем переменную, в которую помещаем информацию о нашем боте.
        Console.WriteLine($"{me.FirstName} запущен!");

        WriteDataDB();

        await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно
    }

    /// <summary>
    /// Загрузка данных с бд
    /// </summary>
    private static void WriteDataDB()
    {
        ConnectDB.Movies.Load();
        ListMovie = ConnectDB.Movies.Local.ToObservableCollection();
    }
}