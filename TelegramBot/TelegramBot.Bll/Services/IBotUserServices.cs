using TelegramBot.Dal.Entities;

namespace TelegramBot.Bll.Services;

public interface IBotUserServices
{
    Task AddTelegramUserAsync(BotUser telegramUser);
    Task UpdateTelegramUserAsync(BotUser telegramUser);
    Task<BotUser> GetByTelegramIdAsync(long id);
    Task<List<BotUser>> GetAllTelegramUsersAsync();
}