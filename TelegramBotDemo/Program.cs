using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotDemo;
using TelegramBotDemo.Models;
using VideoLibrary;

namespace TelegramBotDemo
{
    public class Program
    {
        static ITelegramBotClient botClient;
        static Dictionary<long, int> userPageTracker = new Dictionary<long, int>();
        static CancellationTokenSource cts;

        static async Task Main(string[] args)
        {
            const string TOKEN = "5911983733:AAEOQv0cQy5ITnaOUdYRRZDBywcEvRpgbms";
            botClient = new TelegramBotClient(TOKEN);

            var myBot = await botClient.GetMeAsync();
            Console.WriteLine($"Start listening for @{myBot.Username}");

            cts = new();

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
                );

            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();

        }

        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        static ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
        };

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => HandleMessageAsync(botClient, update, cancellationToken),
                UpdateType.Unknown => throw new NotImplementedException(),
                UpdateType.InlineQuery => throw new NotImplementedException(),
                UpdateType.ChosenInlineResult => throw new NotImplementedException(),
                UpdateType.CallbackQuery => HandleUpdateQueryAsync(botClient, update, cancellationToken),
                UpdateType.EditedMessage => throw new NotImplementedException(),
                UpdateType.ChannelPost => throw new NotImplementedException(),
                UpdateType.EditedChannelPost => throw new NotImplementedException(),
                UpdateType.ShippingQuery => throw new NotImplementedException(),
                UpdateType.PreCheckoutQuery => throw new NotImplementedException(),
                UpdateType.Poll => throw new NotImplementedException(),
                UpdateType.PollAnswer => throw new NotImplementedException(),
                UpdateType.MyChatMember => throw new NotImplementedException(),
                UpdateType.ChatMember => throw new NotImplementedException(),
                UpdateType.ChatJoinRequest => throw new NotImplementedException(),
            };
        }

        static async Task HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;
            var chatId = message.Chat.Id;

            if (messageText == "/music")
            {
                userPageTracker[chatId] = 1;
                UpdateQueryResults(chatId, null, 1);
            }
        }
        static void UpdateQueryResults(long chatId, int? messageId, int page)
        {
            var resultsPerPage = 10;

            var startIndex = (page - 1) * resultsPerPage;
            var endIndex = startIndex + resultsPerPage;

            ApiInfo api = FileService.GetApiInfo();

            var resultToSend = new List<InlineQueryResult>();

            api.entries.Skip(startIndex).Take(resultsPerPage).ToList().ForEach(result =>
                resultToSend.Add(
                    item: new InlineQueryResultArticle(
                        id: result.API, title: $"Page {page}",
                        inputMessageContent: new InputTextMessageContent($"{result.HTTPS}"))
                    {
                        Description = result.Description,
                    }));
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Previous", $"prev_{page}"),
                    InlineKeyboardButton.WithCallbackData("Next", $"next_{page}")
                }
            });

            if (messageId.HasValue)
            {
                botClient.EditMessageReplyMarkupAsync(chatId, messageId.Value, inlineKeyboard);
                botClient.EditMessageTextAsync(chatId, messageId.Value, $"Showing Page {page}:", replyMarkup: inlineKeyboard);
            }
            else
            {
                botClient.AnswerInlineQueryAsync(
                    chatId.ToString(),
                    results: resultToSend,
                    isPersonal: true,
                    cacheTime: 0
                    );
                botClient.SendTextMessageAsync(
                    chatId,
                    $"Showing Page {page}",
                    replyMarkup: inlineKeyboard);
            }

        }
        static async Task HandleUpdateQueryAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.CallbackQuery.Data == null)
                return;
            var callbackQuery = update.CallbackQuery;
            var chatId = callbackQuery.Message.Chat.Id;
            var messageId = callbackQuery.Message.MessageId;
            var callbackData = callbackQuery.Data;

            if(callbackData.StartsWith("next_"))
            {
                var currentPage = GetCurrentPage(chatId);
                var nextPage = currentPage + 1;
                UpdateQueryResults(chatId, messageId, nextPage);
            }
            else if(callbackData.StartsWith("prev_"))
            {
                var currentPage = GetCurrentPage(chatId);
                var nextPage = currentPage - 1;
                UpdateQueryResults(chatId, messageId, nextPage);
            }
        }
        static int GetCurrentPage(long chatId)
        {
            if(userPageTracker.ContainsKey(chatId))
            {
                return userPageTracker[chatId];
            }
            return 1;
        }

        static async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            _ = Task.CompletedTask;
        }
    }
}













    #region sandbox
    /*
    
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
    #endregion
/*
        else if (messageText == "/singleline")
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "single line buttons",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken
                );
        }

        //youtube dagi videoni olish
        else if (messageText.Contains("https://") && (messageText.Contains("youtube.com") || messageText.Contains("youtu.be")))
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

            }*//*
            await botClient.SendVideoAsync(update.Message.Chat.Id, video: "https://dd" + newText);


        }

        else if (update.CallbackQuery?.Data=="next")
        {
            
            var nextOffSet = offset+resultsPerRequest>api.count ? "" : (offset+resultsPerRequest).ToString();

            await botClient.AnswerInlineQueryAsync(update.InlineQuery.Id, resultsToSend.ToArray(), null, false, nextOffSet);
        }
        else
        {
            // Echo received message text
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "You send:\n" + messageText,
                cancellationToken: cancellationToken);
            

        }

*/


