using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using WinBot.Commands.Attributes;

using WinBot.Util;

namespace WinBot.Commands.Owner
{
    public class PSExecCommand : BaseCommandModule
    {
        [Command("psexec")]
        [Description("Execute a PowerShell command")]
        [Usage("[command]")]
        [Category(Category.Owner)]
        [RequireOwner]
        public async Task Exec(CommandContext Context, [RemainingText]string command)
        {
            DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
            eb.WithTitle("PowerShell");
            eb.WithColor(DiscordColor.Gold);
            eb.AddField("Input", $"```bat\n{command}```");
            eb.AddField("Output", $"```bat\n{command.PSPrmpt()}```");
            await Context.ReplyAsync("", eb.Build());
        }
    }
}