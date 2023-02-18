using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

using HBot.Util;

namespace HBot.Commands.Owner
{
    public class CmdExecCommand : BaseCommandModule
    {
        [Command("cmdexec")]
        [Description("Execute a terminal command (Windows)")]
        [Usage("[command]")]
        [Category(Category.Owner)]
        [RequireOwner]
        public async Task Exec(CommandContext Context, [RemainingText]string command)
        {
            if(!Environment.OSVersion.VersionString.Contains("Microsoft Windows NT"))
                throw new System.Exception("The bot must be running on Windows to use that command.");
            DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
            eb.WithTitle("Command Prompt");
            eb.WithColor(DiscordColor.Gold);
            eb.AddField("Input", $"```sh\n{command}```");
            eb.AddField("Output", $"```sh\n{command.CmdPrmpt()}```");
            await Context.ReplyAsync("", eb.Build());
        }
    }
}