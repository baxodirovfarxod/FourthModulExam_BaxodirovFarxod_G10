using Microsoft.EntityFrameworkCore;
using TelegramBot.Dal.Configurations;
using TelegramBot.Dal.Entities;

namespace TelegramBot.Dal;
public class MainContext : DbContext
{
    public DbSet<BotUser> BotUsers { get; set; }   

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=WIN-BNO54FDBS2G\\SQLEXPRESS;Database=TelegramBot;User Id=sa;Password=1;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BotUserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
