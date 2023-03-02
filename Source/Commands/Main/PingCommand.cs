using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using HBot.Commands.Attributes;

namespace HBot.Commands.Main {
    public class PingCommand : BaseCommandModule {
        [Command("ping")]
        [Description("Gets the bots latency to Discord")]
        [Category(Category.Main)]
        public async Task Ping(CommandContext Context) {
            await Context.ReplyAsync($"🏓 Pong! Latency is **{Bot.client.Ping}ms**.");
        }
    }
}