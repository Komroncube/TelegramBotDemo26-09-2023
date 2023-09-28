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