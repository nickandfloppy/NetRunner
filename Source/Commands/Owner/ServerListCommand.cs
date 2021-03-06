using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using WinBot.Commands.Attributes;

using WinBot.Misc;
using WinBot;

using Serilog;

namespace WinBot.Commands.Owner
{
    public class ServerListCommand : BaseCommandModule
    {
        [Command("serverlist")]
        [Description("Lists guilds the bot is in")]
        [Category(Category.Owner)]
        [RequireOwner]
        public async Task ServerList(CommandContext Context)
        {
            DiscordEmbedBuilder Embed = new DiscordEmbedBuilder();
            Embed.WithColor(DiscordColor.Gold);
            string output = "";
            foreach (var guild in Bot.client.Guilds) {
                output += $"{guild.Value.Name}\n";
            }
            Embed.WithTitle("Servers");
            Embed.WithDescription(output);
            await Context.ReplyAsync("", Embed.Build());
        }
    }
}