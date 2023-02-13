using System.Linq;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using HBot.Commands.Attributes;

namespace HBot.Commands.Shithole_Exclusive {
    public class BlogSubscribeCommand : BaseCommandModule {
        private const long ShitholeId = 955969771994742874;

        [Command("blog-sub")]
        [Description("Subscribes or unsubscribes you to pings for polls")]
        [Category(Category.Shithole_Exclusive)]
        public async Task Subscribe(CommandContext ctx) {
            
            if (ctx.Guild.Id != ShitholeId)
                throw new System.Exception("This command can only be run in HIDEN's Shithole.");

            var role = ctx.Guild.GetRole(1052534190568116264);

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