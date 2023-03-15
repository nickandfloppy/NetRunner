using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;

using HBot.Commands.Attributes;

namespace HBot.Commands.Main {
    public class ReportCommand : BaseCommandModule {

        [Command("report")]
        [Description("Report a member, set them up for an ass kicking")]
        [Category(Category.Main)]

        public async Task Report(CommandContext ctx, [Description("mention/ID")] DiscordMember member, [RemainingText, Description("reason")] string reason = null) {

            if (ctx.Guild != Global.targetGuild) {
                throw new System.Exception($"This command can only be run in {Global.targetGuild.Name}.");
            }

            if (ctx.Channel.IsPrivate) {
                await ctx.Message.DeleteAsync();
                return;
            }

            if (member == null) {
                await ctx.RespondAsync("I couldn't find that person.");
                return;
            }

            if (member.Permissions.HasPermission(Permissions.BanMembers)) {
                await ctx.RespondAsync("I can't report that member because they have permission to ban members.");
                return;
            }

            if (member.IsBot) {
                await ctx.RespondAsync("I can't report that member because they are a bot.");
                return;
            }

            if (string.IsNullOrWhiteSpace(reason)) {
                await ctx.RespondAsync("Please provide a reason for the report");
                return;
            }

            var embed = new DiscordEmbedBuilder {
                Color = new DiscordColor("#ff0000"),
                Timestamp = System.DateTime.UtcNow,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    Text = ctx.Guild.Name,
                    IconUrl = ctx.Guild.IconUrl
                },
                Author = new DiscordEmbedBuilder.EmbedAuthor {
                    Name = "Reported member",
                    IconUrl = member.AvatarUrl
                },
                Description = $"**- Member:** {member.Mention} ({member.Id})\n" +
                              $"**- Reported by:** {ctx.Member.Mention}\n" +
                              $"**- Reported in:** {ctx.Channel.Mention}\n" +
                              $"**- Reason:** {reason}"
            };

            await Global.reportsChannel.SendMessageAsync($"@here", embed);
            await ctx.Message.RespondAsync($"Reported {member.Mention} successfully. The mods will look at it shortly.");
        }
    }
}