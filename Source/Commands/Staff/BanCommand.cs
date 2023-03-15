using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

namespace HBot.Commands.Staff {
    public class BanCommand : BaseCommandModule {
        [Command("ban")]
        [Description("Ban a bloke")]
        [RequirePermissions(DSharpPlus.Permissions.BanMembers)]
        [Category(Category.Staff)]
        public async Task Ban(CommandContext ctx, DiscordMember target, [RemainingText] string reason = null) {
            if (target.Id == ctx.User.Id) {
                await ctx.RespondAsync($"{ctx.User.Username}, ...You can't ban yourself.");
                return;
            }

            await target.BanAsync(0, reason);

            var embed = new DiscordEmbedBuilder()
            .WithTitle("Action: Ban")
            .WithDescription($"Banned {target.Mention} ({target.Id})")
            .WithColor(DiscordColor.Red)
            .WithThumbnail(target.AvatarUrl)
            .WithFooter($"Banned by {ctx.User.Username}");

            await Global.logChannel.SendMessageAsync(embed: embed.Build());
            await ctx.Message.RespondAsync($"Banned {target.Mention} successfully.");
        }
    }
}