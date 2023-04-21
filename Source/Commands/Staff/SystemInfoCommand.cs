using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

namespace HBot.Commands.Staff {
    public class SystemInfoCommandModule : BaseCommandModule {
        [Command("systeminfo")]
        [Description("Reports system info about the bot's host")]
        [Category(Category.Staff)]
        [RequireUserPermissions(DSharpPlus.Permissions.ManageGuild)]
        public async Task SystemInfo(CommandContext ctx) {
            string osArchitecture = RuntimeInformation.OSArchitecture.ToString().ToLower();
            string frameworkVersion = Environment.Version.ToString();
            string processorCount = Environment.ProcessorCount.ToString();
            long memoryUsage = Process.GetCurrentProcess().WorkingSet64 / (1024 * 1024);
            string uptime = $"{TimeSpan.FromMilliseconds(Environment.TickCount64):hh\\:mm\\:ss}";

            DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
            eb.WithTitle("System Information");
            eb.WithColor(DiscordColor.Blue);
            eb.AddField($"**Operating System:**", RuntimeInformation.OSDescription, true);
            eb.AddField($"**Architecture:**", osArchitecture, true);
            eb.AddField($"**.NET Version:**", frameworkVersion, true);
            eb.AddField($"**Processor Count:**", processorCount, true);
            eb.AddField("**Memory Usage**", $"{memoryUsage} MB", true);
            eb.AddField($"**Uptime:**", uptime, true);

            await ctx.ReplyAsync("", eb.Build());
        }
    }
}