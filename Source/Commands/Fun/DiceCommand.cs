using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;

using HBot.Commands.Attributes;

namespace HBot.Commands.Fun {
    public class RollCommand : BaseCommandModule {
        [Command("roll")]
        [Description("Rolls a die")]
        [Category(Category.Fun)]
        public async Task Roll(CommandContext ctx) {
            var random = new Random();
            var result = random.Next(1, 7);
            await ctx.Channel.SendMessageAsync($":game_die: **{ctx.Member.Username}**, you rolled a **{result}**!");
        }
    }
}