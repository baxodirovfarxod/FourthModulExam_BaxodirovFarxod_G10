using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramBot.Dal.Entities;

namespace TelegramBot.Dal.Configurations;

public class BotUserConfiguration : IEntityTypeConfiguration<BotUser>
{
    public void Configure(EntityTypeBuilder<BotUser> builder)
    {
        builder.ToTable("BotUsers");

        builder.HasKey(t => t.BotUserId);

        builder.Property(t => t.ChatId)
            .IsRequired();

        builder.Property(t => t.FirstName)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(t => t.LastName)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(t => t.PhoneNumber)
            .IsRequired(false);

        builder.Property(t => t.Address)
            .IsRequired(false)
            .HasMaxLength(200);

        builder.Property(t => t.Email)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(t => t.DateOfBirth)
            .IsRequired();
    }
}
