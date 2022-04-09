using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using WinBot.Commands.Attributes;

namespace WinBot.Commands.Main
{
    public class BabenCommand : BaseCommandModule
    {
        [Command("baben")]
        [Description("Gets the bots latency to Discord")]
        [Category(Category.Main)]
        public async Task Baben(CommandContext Context)
        {
            await Context.ReplyAsync($"BABEN <a:flushed_ball:932432088337117184>");
        }
    }
}