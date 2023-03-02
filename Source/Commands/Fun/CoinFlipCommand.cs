using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using HBot.Commands.Attributes;

namespace HBot.Commands.Fun {
    public class CoinFlipCommand : BaseCommandModule {
        [Command("coinflip")]
        [Description("Flip a coin")]
        [Usage("coinflip")]
        [Category(Category.Fun)]
        public async Task CoinFlip(CommandContext Context) {
            var msg = await Context.ReplyAsync(":coin: Flipping a coin...");
            System.Threading.Thread.Sleep(600);
            await msg.ModifyAsync(messages[new Random().Next(0, messages.Count)]);
        }
        public List<string> messages = new List<string>() {
            ":coin: It landed on heads!",
            ":coin: It landed on tails!",
            ":coin: It landed on the side. o_O"
        };
    }
}