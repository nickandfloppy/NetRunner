using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using WinBot.Commands.Attributes;

namespace WinBot.Commands.Main
{
    public class WarnCommand : BaseCommandModule
    {
        [Command("warn")]
        [Description("Warn a user")]
        [Usage("[user] [reason]")]
        [Category(Category.Staff)]
        [RequireUserPermissions(DSharpPlus.Permissions.KickMembers)]
        public async Task Warn(CommandContext Context, DiscordMember user, [RemainingText] string reason = null)
        {
            if (reason == null)
                reason = "No reason given";
                
            DiscordEmbedBuilder warnEmbed = new DiscordEmbedBuilder();
            DateTime now = DateTime.Now;
            string date = $"{now.Day}/{now.Month}/{now.Year}";
            warnEmbed.WithTitle($":warning: {Context.Guild.Name} · Warn");
            warnEmbed.WithDescription($"**You've been warned for the following reason:**```{reason}```");
            warnEmbed.WithFooter($"Punishment · ${date}", Context.Guild.IconUrl);
            warnEmbed.WithColor(new DiscordColor("#ffff00"));
            await user.SendMessageAsync(warnEmbed);
            await Context.RespondAsync($"`{user.Username}#{user.Discriminator}` has been warned for {reason}");
        }
    }
}