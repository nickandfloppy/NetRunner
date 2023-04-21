using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;

using HBot.Commands.Attributes;

namespace HBot.Commands.Staff {
    public class ModerationCommands : BaseCommandModule {
        [Command("kick")]
        [Description("Kicks a user from the server")]
        [RequirePermissions(DSharpPlus.Permissions.KickMembers)]
        [Category(Category.Staff)]
        public async Task KickCommand(CommandContext ctx, [Description("The user to kick")] DiscordMember target, [RemainingText, Description("The reason for the kick")] string reason = null) {
            if (target == ctx.User) {
                await ctx.ReplyAsync($"{ctx.User.Username}: ...You can't kick yourself. Wh.. Why'd you even try? Are you high or something?");
                return;
            }

            var embed = new DiscordEmbedBuilder()
            .WithTitle("Action: Kick")
            .WithDescription($"Kicked {target.Mention} ({target.Id})")
            .WithColor(DiscordColor.Red)
            .WithThumbnail(target.AvatarUrl)
            .WithFooter($"Kicked by {ctx.User.Username}");

            if (!string.IsNullOrWhiteSpace(reason))
                embed.AddField("Reason", reason);

            await Global.logChannel.SendMessageAsync(embed: embed.Build());
            await target.RemoveAsync(reason);
            await ctx.Message.RespondAsync($"Kicked {target.Mention} successfully.");
        }
    }
}