using System.Linq;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using HBot.Commands.Attributes;

namespace HBot.Commands.Shithole_Exclusive {
    public class VoteSubscribeCommand : BaseCommandModule {
        // TODO: Move this to the config file, will probably do it in 1.5.0
        private const long ShitholeId = 955969771994742874;

        [Command("voting-sub")]
        [Description("Subscribes or unsubscribes you to pings for polls")]
        [Category(Category.Shithole_Exclusive)]
        public async Task VoteSubscribe(CommandContext ctx) {
            
            if (ctx.Guild.Id != ShitholeId) {
                throw new System.Exception("This command cannot be ran in this server; it is exclusive to HIDEN's Shithole.");
            }

            var role = ctx.Guild.GetRole(982089257692839956);

            if (ctx.Member.Roles.Contains(role)) {
                await ctx.Member.RevokeRoleAsync(role);
                await ctx.RespondAsync("Unsubscribed from polling pings.");
                return;
            }

            await ctx.Member.GrantRoleAsync(role);
            await ctx.RespondAsync("Succesfully subscribed to polling pings!");
        }
    }
}