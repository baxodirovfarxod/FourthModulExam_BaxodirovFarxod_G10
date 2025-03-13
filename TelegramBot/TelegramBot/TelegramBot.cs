using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Bll.Services;
using TelegramBot.Dal.Entities;

namespace TelegramBot;

public class TelegramBot
{
    private readonly IBotUserServices botUserService;
    private static readonly string botToken = "7704075141:AAEcIT6UEBzFQSllwqNrnK1MC3rasEOMK-I";
    private readonly ITelegramBotClient botClient = new TelegramBotClient(botToken);

    public TelegramBot(IBotUserServices botUserService)
    {
        this.botUserService = botUserService;
    }

    public async Task StartBot()
    {
        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync
            );

        Console.WriteLine("Bot is started");
        Console.ReadKey();
    }

    private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message)
        {
            var user = update.Message.Chat;
            var message = update.Message;
            if (message.Text == "/start")
            {
                var savingUser = new BotUser()
                {
                    ChatId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                var menu = await StartMenu();

                await botClient.SendTextMessageAsync(
                chatId: user.Id,
                text: "options tanlang",
                parseMode: ParseMode.Markdown,
                replyMarkup: menu
                );

                await botUserService.AddTelegramUserAsync(savingUser);
            }
            else if (message.Text.StartsWith("Send All Users : ") && user.Id == 7636487012)
            {
                var users = await botUserService.GetAllTelegramUsersAsync();
                foreach (var u in users)
                {
                    if (u.ChatId != 7636487012)
                    {
                        await bot.SendTextMessageAsync(u.ChatId, message.Text.Remove(0, 17));
                    }
                }
            }
            else if (message.Text == "Get data")
            {
                var userInfo = await botUserService.GetByTelegramIdAsync(user.Id);
                if (userInfo.LastName is null)
                {
                    await bot.SendTextMessageAsync(user.Id, "LastName kiritilmagan!");
                }
                if (userInfo.Email is null)
                {
                    await bot.SendTextMessageAsync(user.Id, "Email kiritilmagan!");
                }
                if (userInfo.PhoneNumber is null)
                {
                    await bot.SendTextMessageAsync(user.Id, "PhoneNumber kiritilmagan!");
                }
                if (userInfo.Address is null)
                {
                    await bot.SendTextMessageAsync(user.Id, "Address kiritilmagan!");
                }
                if (userInfo.DateOfBirth == DateTime.MinValue)
                {
                    await bot.SendTextMessageAsync(user.Id, "DateOfBirth kiritilmagan!");
                }
                else
                {
                    var text = $"TelegramId: {userInfo.ChatId}\n" +
                        $"FirstName: {userInfo.FirstName}\n" +
                        $"LastName: {userInfo.LastName}\n" +
                        $"Email: {userInfo.Email}\n" +
                        $"PhoneNumber: {userInfo.PhoneNumber}\n" +
                        $"Address: {userInfo.Address}\n" +
                        $"DateOfBirth: {userInfo.DateOfBirth}\n";
                    await bot.SendTextMessageAsync(userInfo.ChatId, text);
                }
            }
            else if (message.Text == "Delete data")
            {
                var userInfo = await botUserService.GetByTelegramIdAsync(user.Id);
                userInfo.FirstName = null;
                userInfo.LastName = null;
                userInfo.Email = null;
                userInfo.PhoneNumber = null;
                userInfo.Address = null;
                userInfo.DateOfBirth = DateTime.MinValue;
                await botUserService.UpdateTelegramUserAsync(userInfo);
                await bot.SendTextMessageAsync(user.Id, "Barcha malumot o'chirildi!");
            }
            else if (message.Text == "Fill Data")
            {
                var userInfo = await botUserService.GetByTelegramIdAsync(user.Id);
                if (userInfo.FirstName is null)
                {
                    await bot.SendTextMessageAsync(user.Id, "First name kiriting: (FirstName: Palonchiyev) shu formatda");
                }
                if (userInfo.LastName is null)
                {
                    await bot.SendTextMessageAsync(user.Id, "Last name kiriting: (LastName: Palonchiyev) shu formatda");
                }
                if (userInfo.Address is null)
                {
                    await bot.SendTextMessageAsync(user.Id, "Address kiriting: (Address: ) shu formatda");
                }
                if (userInfo.Email is null)
                {
                    await bot.SendTextMessageAsync(user.Id, "Email kiriting: (Email: ) shu formatda");
                }
                if (userInfo.PhoneNumber is null)
                {
                    await bot.SendTextMessageAsync(user.Id, "PhoneNumber kiriting: (PhoneNumber: +998112223344) shu formatda");
                }
                if (userInfo.DateOfBirth == DateTime.MinValue)
                {
                    await bot.SendTextMessageAsync(user.Id, "DateOfBirth kiriting: (yyyy.mm.dd) shu formatda");
                }
            }
            else if (message.Text.StartsWith("LastName: "))
            {
                var userInfo = await botUserService.GetByTelegramIdAsync(user.Id);
                userInfo.LastName = message.Text.Remove(0, 10);
                await botUserService.UpdateTelegramUserAsync(userInfo);
            }
            else if (message.Text.StartsWith("FirstName: "))
            {
                var userInfo = await botUserService.GetByTelegramIdAsync(user.Id);
                userInfo.FirstName = message.Text.Remove(0, 11);
                await botUserService.UpdateTelegramUserAsync(userInfo);
            }
            else if (message.Text.StartsWith("Address: "))
            {
                var userInfo = await botUserService.GetByTelegramIdAsync(user.Id);
                userInfo.Address = message.Text.Remove(0, 9);
                await botUserService.UpdateTelegramUserAsync(userInfo);
            }
            else if (message.Text.StartsWith("Email: "))
            {
                var userInfo = await botUserService.GetByTelegramIdAsync(user.Id);
                userInfo.Email = message.Text.Remove(0, 7);
                if (!userInfo.Email.EndsWith("@gmail.com"))
                {
                    await bot.SendTextMessageAsync(user.Id, "Xato !");
                }
                else
                {
                    await botUserService.UpdateTelegramUserAsync(userInfo);
                }
            }
            else if (message.Text.StartsWith("PhoneNumber: "))
            {
                var userInfo = await botUserService.GetByTelegramIdAsync(user.Id);
                userInfo.PhoneNumber = message.Text.Remove(0, 13);
                await botUserService.UpdateTelegramUserAsync(userInfo);
            }
            else if (message.Text.StartsWith("DateOfBirth"))
            {
                var userInfo = await botUserService.GetByTelegramIdAsync(user.Id);
                userInfo.DateOfBirth = DateTime.Parse(message.Text.Remove(0, 12));
                await botUserService.UpdateTelegramUserAsync(userInfo);
            }
        }
    }

    private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Error: {exception.Message}");
    }

    private async Task<ReplyKeyboardMarkup> StartMenu()
    {
        var keyboard = new ReplyKeyboardMarkup(
            new KeyboardButton("Fill Data"),
            new KeyboardButton("Get data"),
            new KeyboardButton("Delete data")
        )
        {
            ResizeKeyboard = true
        };

        return keyboard;
    }
}
