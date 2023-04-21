// I *will* make this staff only if this gets abused. Do not dumb.
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using HBot.Commands.Attributes;

namespace HBot.Commands.Fun {
    public class SayCommand : BaseCommandModule {
        [Command("say")]
        [Description("Makes the bot repeat your message.")]
        [Category(Category.Fun)]

        public async Task Say(CommandContext context, [RemainingText] string message) {
            if (message.Contains("@everyone") || message.Contains("@here") || context.Message.MentionEveryone) {
                await context.Message.RespondAsync("Do not dumb. Not dumb area here.");
                return;
            }
            
            await context.Message.DeleteAsync();
            await context.Channel.SendMessageAsync(message);
        }
    }
}