/*using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
namespace LessonTelegramBot
{
    public class Program
    {
        static ITelegramBotClient botClient;
        static async Task Main(string[] args)
        {
            botClient = new TelegramBotClient("5911983733:AAEOQv0cQy5ITnaOUdYRRZDBywcEvRpgbms");

            var me = await botClient.GetMeAsync();

            await Console.Out.WriteLineAsync(me.Username + " " + me.FirstName);

            botClient.OnMessage += On_Message;

            botClient.StartReceiving();

            Console.ReadLine();
        }
        private static async void On_Message(object? sender, MessageEventArgs e)
        {
            var text = e.Message.Text;

            if (text == "/start")
            {
                await botClient.SendTextMessageAsync(e.Message.Chat.Id, $"Assalomu alaykum {e.Message.Chat.FirstName}");
            }
            else if (text == "/sticker")
            {
                await botClient.SendStickerAsync(
                chatId: e.Message.Chat.Id,
                sticker: "https://github.com/TelegramBots/book/raw/master/src/docs/sticker-dali.webp");

            }
            else if (text == "/video")
            {
                await botClient.SendVideoAsync(e.Message.Chat.Id, "https://github.com/TelegramBots/book/raw/master/src/docs/video-bulb.mp4", width: 700, height: 800);
            }
            else if (text == "/photo")
            {
                await botClient.SendPhotoAsync(
                    chatId: e.Message.Chat.Id,
                    photo: "https://upload.wikimedia.org/wikipedia/commons/4/41/Sunflower_from_Silesia2.jpg",
                    caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
                    parseMode: ParseMode.Html);
            }
            else if (text.StartsWith("https://www.instagram.com"))
            {
                //await botClient.SendTextMessageAsync(e.Message.Chat.Id, "Downloading📥", replyToMessageId: e.Message.MessageId);

                var newText = text.Substring(12);

                await botClient.SendVideoAsync(e.Message.Chat.Id, video: "https://dd" + newText);
            }
        }
    }
}*/

//6076037138:AAEhpp31iSK3c1X6bogk9afnMiOKpEJoY7o

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotDemo;
using VideoLibrary;

string TOKEN = "5911983733:AAEOQv0cQy5ITnaOUdYRRZDBywcEvRpgbms";
var botClient = new TelegramBotClient(TOKEN);
var myBot = await botClient.GetMeAsync();
Console.WriteLine(myBot);


using CancellationTokenSource cts = new();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();







async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    InlineKeyboardMarkup inlineKeyboard = new(new[]
{
    new[]
    {
        InlineKeyboardButton.WithCallbackData(text:"1.1", callbackData:"11"),
        InlineKeyboardButton.WithCallbackData(text:"1.2", callbackData:"12"),
    },
    new []
    {
        InlineKeyboardButton.WithCallbackData(text: "2.1", callbackData: "21"),
        InlineKeyboardButton.WithCallbackData(text: "2.2", callbackData: "22"),
    },
    new[]
    {
        InlineKeyboardButton.WithSwitchInlineQuery(text:"switch inline query"),
        InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(
            text:"switch to next query in chat"),

    }

});
    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
    {
        new KeyboardButton[] { "Help me", "Call me ☎️" },
    })
    {
        ResizeKeyboard = true
    };



    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
    await Console.Out.WriteLineAsync(messageText);
    /*
    // Echo received message text
    Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "You send:\n" + messageText,
        cancellationToken: cancellationToken);
    */

    //sticker send
    /*Message sentmessage = await botClient.SendStickerAsync(
    chatId: chatId,
    sticker: "https://github.com/TelegramBots/book/raw/master/src/docs/sticker-dali.webp",
    cancellationToken: cancellationToken);
    */
    /*
        Message sentmessage = await botClient.SendVideoAsync(
        chatId: chatId,
        //photo: "https://github.com/TelegramBots/book/blob/master/src/docs/photo-ara.jpg",
        video: "https://github.com/TelegramBots/book/blob/master/src/docs/video-hawk.mp4",
        cancellationToken: cancellationToken);
    */
    /*
    //send photo

        Message sentmessage = await botClient.SendPhotoAsync(
        chatId: chatId,
        photo:("https://images.wallpapersden.com/image/download/the-legend-of-heroes-trails-of-cold-steel-hd_bWptZ2yUmZqaraWkpJRmbmdlrWZlbWU.jpg"),
        caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
        parseMode: ParseMode.Html,
        cancellationToken: cancellationToken);
    */
    try
    {
        //salomlashish
        if (messageText == "/start")
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: $"Salom {update.Message.ForwardSenderName}\n" +
            $"{message.Chat.Bio}\n" +
            $"{message.Chat.FirstName} " +
            $"{message.Chat.LastName}",
            replyMarkup:inlineKeyboard,
            cancellationToken: cancellationToken);



            //FolderSearch.SearchForFolder();
        }
        else if(messageText=="/singleline")
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "single line buttons",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken:cancellationToken
                );
        }

        //youtube dagi videoni olish
        else if(messageText.Contains("https://") && (messageText.Contains("youtube.com") || messageText.Contains("youtu.be")))
        {
            YouTube youTube = new YouTube();
            YouTubeVideo youTubeVideo = youTube.GetVideo(messageText);
            Stream ytStream = youTubeVideo.Stream();

            
            var name = youTubeVideo.FullName;


            await botClient.SendVideoAsync(
                chatId: chatId,
                video: ytStream,
                caption: name
                );
            FileSaver fileSaver = new FileSaver(name, ytStream);
            fileSaver.Save();

        }

        //instagram
        else if (messageText.StartsWith("https://www.instagram.com"))
        {
            //await botClient.SendTextMessageAsync(e.Message.Chat.Id, "Downloading📥", replyToMessageId: e.Message.MessageId);

            var newText = messageText.Substring(12);
            /*using(HttpClient client = new HttpClient())
            {
                var stream = await client.GetStreamAsync(messageText);
                *//*FileSaver fileSaver = new FileSaver("instagramfile", stream);
                fileSaver.Save();*//*

            }*/
            await botClient.SendVideoAsync(update.Message.Chat.Id, video: "https://dd" + newText);
            
            
        }
    }
    catch(Exception ex)
    {
        await Console.Out.WriteLineAsync(ex.Message);
        return;
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
