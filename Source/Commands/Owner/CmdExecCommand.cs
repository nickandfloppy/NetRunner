using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using WinBot.Commands.Attributes;

using WinBot.Util;

namespace WinBot.Commands.Owner
{
    public class CmdExecCommand : BaseCommandModule
    {
        [Command("cmdexec")]
        [Description("Execute a command prompt command")]
        [Usage("[command]")]
        [Category(Category.Owner)]
        [RequireOwner]
        public async Task Exec(CommandContext Context, [RemainingText]string command)
        {
            DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
            eb.WithTitle("Command Prompt");
            eb.WithColor(DiscordColor.Gold);
            eb.AddField("Input", $"```bat\n{command}```");
            eb.AddField("Output", $"```bat\n{command.CmdPrmpt()}```");
            await Context.ReplyAsync("", eb.Build());
        }
    }
}