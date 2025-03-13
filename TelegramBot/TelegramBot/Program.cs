using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Bll.Services;
using TelegramBot.Dal;
using TelegramBot.Dal.Entities;

namespace TelegramBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            // DI konteynerga xizmatlarni qo'shish
            serviceCollection.AddDbContext<MainContext>();
            serviceCollection.AddScoped<IBotUserServices, BotUserServices>();
            serviceCollection.AddSingleton<TelegramBot>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Scope ochish
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MainContext>();

            // Migrations qo‘llash
            dbContext.Database.Migrate();

            // Telegram botni ishga tushirish
            var telegramBot = scope.ServiceProvider.GetRequiredService<TelegramBot>();
            await telegramBot.StartBot();
        }
    }
}
