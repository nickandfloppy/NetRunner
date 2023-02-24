using System.Linq;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using HBot.Commands.Attributes;

namespace HBot.Commands.Shithole_Exclusive {
    public class NetUpSubscribeCommand : BaseCommandModule {
        private const long ShitholeId = 955969771994742874;

        [Command("netup-sub")]
        [Description("Subscribes or unsubscribes you to HIDNet-related announcement pings")]
        [Category(Category.Shithole_Exclusive)]
        public async Task NetUpSubscribe(CommandContext ctx) {
            
            if (ctx.Guild.Id != ShitholeId)
                throw new System.Exception("This command cannot be ran in this server; it is exclusive to HIDEN's Shithole.");

            var role = ctx.Guild.GetRole(1008109359903023175);

            if (ctx.Member.Roles.Contains(role)) {
                await ctx.Member.RevokeRoleAsync(role);
                await ctx.RespondAsync("Unsubscribed from HIDNet announcement pings.");
                return;
            }

            await ctx.Member.GrantRoleAsync(role);
            await ctx.RespondAsync("Succesfully subscribed to HIDNet announcement pings!");
        }
    }
}