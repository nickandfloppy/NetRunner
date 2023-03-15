using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

using HBot.Util;

namespace WinBot.Commands.Fun {
    public class FortuneCommand : BaseCommandModule {
        [Command("fortune")]
        [Description("It's the BSD fortune program")]
        [Category(Category.Fun)]
        public async Task Exec(CommandContext Context) {
            DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
			eb.WithTitle("BSD Fortune program... but Discord");
            eb.WithColor(DiscordColor.Gold);
            if(!Environment.OSVersion.VersionString.Contains("Unix"))
                eb.WithDescription($"```{Shell.WinCmd("C:\\cygwin64\\bin\\fortune.exe -a")}```");
            else {
                eb.WithDescription($"```{Shell.BashCmd("/usr/bin/fortune -a")}```");
            }
            await Context.ReplyAsync("", eb.Build());
        }
    }
}