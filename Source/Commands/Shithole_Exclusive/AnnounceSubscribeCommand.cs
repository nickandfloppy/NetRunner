using System.Linq;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using HBot.Commands.Attributes;

namespace HBot.Commands.Shithole_Exclusive {
    public class AnnounceSubscribeCommand : BaseCommandModule {
        private const long ShitholeId = 955969771994742874;

        [Command("announce-sub")]
        [Description("Subscribes or unsubscribes you to announcement pings")]
        [Category(Category.Shithole_Exclusive)]
        public async Task Subscribe(CommandContext ctx) {
            
            if (ctx.Guild.Id != ShitholeId)
                throw new System.Exception("This command can only be run in HIDEN's Shithole.");

            var role = ctx.Guild.GetRole(957028369885691924);

            if (ctx.Member.Roles.Contains(role)) {
                await ctx.Member.RevokeRoleAsync(role);
                await ctx.RespondAsync("Unsubscribed from general announcement pings.");
                return;
            }

            await ctx.Member.GrantRoleAsync(role);
            await ctx.RespondAsync("Succesfully subscribed to general announcement pings!");
        }
    }
}