using System.Net;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

using Newtonsoft.Json;

namespace HBot.Commands.Fun
{
    public class BsodCommand : BaseCommandModule
    {
        [Command("bsod")]
        [Description("It does the thing™")]
        [Category(Category.Fun)]
        public async Task BSOD(CommandContext Context)
        {
            // I should actually add shit to this at some point - HIDEN
            await Context.ReplyAsync("succ is dead, no succ");
        }
    }
}