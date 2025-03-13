using Microsoft.EntityFrameworkCore;
using TelegramBot.Dal;
using TelegramBot.Dal.Entities;

namespace TelegramBot.Bll.Services;

public class BotUserServices : IBotUserServices
{
    public readonly MainContext mainContext;

    public BotUserServices(MainContext mainContext)
    {
        this.mainContext = mainContext;
    }

    public async Task AddTelegramUserAsync(BotUser telegramUser)
    {
        var user = await GetByTelegramIdAsync(telegramUser.ChatId);

        if (user != null)
        {
            return;
        }

        await mainContext.BotUsers.AddAsync(telegramUser);
        await mainContext.SaveChangesAsync();
    }

    public async Task<List<BotUser>> GetAllTelegramUsersAsync()
    {
        return await mainContext.BotUsers.ToListAsync();
    }

    public async Task<BotUser> GetByTelegramIdAsync(long chatId)
    {
        var user = await mainContext.BotUsers.FirstOrDefaultAsync(t => t.ChatId == chatId);

        if (user == null)
        {
            Console.WriteLine($"{chatId} lik user databaseda yo'q !");
        }

        return user;
    }

    public async Task UpdateTelegramUserAsync(BotUser botUser)
    {
        var user = await GetByTelegramIdAsync(botUser.ChatId);
        if (user == null)
        {
            Console.WriteLine($"{botUser.ChatId} lik foydalanuvchi topilmadi");
            return;
        }

        user.FirstName = botUser.FirstName;
        user.LastName = botUser.LastName;
        user.PhoneNumber = botUser.PhoneNumber;
        user.Email = botUser.Email;
        user.Address = botUser.Address;
        user.DateOfBirth = botUser.DateOfBirth;

        await mainContext.SaveChangesAsync();
    }
}
