using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

using HBot.Util;

namespace HBot.Commands.Owner
{
    public class BashExecCommand : BaseCommandModule
    {
        [Command("bashexec")]
        [Description("Execute a terminal command (*nix)")]
        [Usage("[command]")]
        [Category(Category.Owner)]
        [RequireOwner]
        public async Task Exec(CommandContext Context, [RemainingText]string command)
        {
            if(!Environment.OSVersion.VersionString.Contains("Unix"))
                throw new System.Exception("The bot must be running on Linux or macOS to use that command.");
            DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
            eb.WithTitle("Exec");
            eb.WithColor(DiscordColor.Gold);
            eb.AddField("Input", $"```sh\n{command}```");
            eb.AddField("Output", $"```sh\n{command.BashCmd()}```");
            await Context.ReplyAsync("", eb.Build());
        }
    }
}