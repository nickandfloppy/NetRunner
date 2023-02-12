using System.Linq;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using HBot.Commands.Attributes;

namespace HBot.Commands.Main
{
    public class SubscribeCommand : BaseCommandModule
    {
        [Command("subscribe")]
        [Description("Subscribes or unsubscribes you to announcement pings")]
        [Category(Category.Main)]
        public async Task Subscribe(CommandContext ctx)
        {
            var role = ctx.Guild.GetRole(860597033736208404);
            if (ctx.Member.Roles.Contains(role))
            {
                await ctx.Member.RevokeRoleAsync(role);
                await ctx.RespondAsync("Unsubscribed from announcement pings!");
                return;
            }

            await ctx.Member.GrantRoleAsync(role);
            await ctx.RespondAsync("Succesfully subscribed to announcement pings!");
        }
    }
}