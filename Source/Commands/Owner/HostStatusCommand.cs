using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

namespace HBot.Commands.Owner {
    public class HostStatusCommand : BaseCommandModule {
        [Command("hoststatus")]
        [Description("Gets info about the host machine")]
        [Aliases("host", "hostinfo")]
        [Category(Category.Owner)]
        [RequireOwner]
        public async Task HostStatus(CommandContext Context) {
            DiscordEmbedBuilder Embed = new DiscordEmbedBuilder();
            Embed.WithColor(DiscordColor.Gold);
            if(!Environment.OSVersion.VersionString.Contains("Unix"))
                Embed.WithDescription($"```{ParseNF(Util.Shell.WinCmd("neofetch --disable title resolution theme icons gpu term --stdout").Replace(" \nOS:", "OS:"))}```");
            else {
                Embed.WithDescription($"```{ParseNF(Util.Shell.BashCmd("neofetch --disable title resolution theme icons gpu term --stdout").Replace(" \nOS:", "OS:"))}```");
            }
            await Context.ReplyAsync("", Embed.Build());
        }

        public string ParseNF(string neofetch) {
            string parsed = neofetch;
            parsed = parsed
                .Replace("OS: ",        "OS:            ")
                .Replace("Host: ",      "Host           ")
                .Replace("Kernel: ",    "Kernel:        ")
                .Replace("Uptime: ",    "Uptime:        ")
                .Replace("Packages: ",  "Packages:      ")
                .Replace("Shell: ",     "Shell:         ")
                .Replace("DE: ",        "DE:            ")
                .Replace("WM: ",        "WM:            ")
                .Replace("CPU: ",       "CPU:           ")
                .Replace("GPU: ",       "GPU:           ")
                .Replace("Memory: ",    "Memory:        ");
            return parsed;
        }
    }
}