using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

namespace HBot.Commands.Fun {
    public class LoveCommand : BaseCommandModule {
        [Command("love")]
        [Aliases("affinity")]
        [Description("Calculates the love affinity you have for another person.")]
        [Category(Category.Fun)]
        public async Task LoveCommandAsync(CommandContext ctx, [Description("[mention | id | username]")] DiscordMember user) {
            if (user == null || ctx.User.Id == user.Id) {
                var members = await ctx.Guild.GetAllMembersAsync();
                //user = members.Where(m => m.Id != ctx.User.Id).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            }

            if (user.Id == ctx.User.Id) {
                await ctx.RespondAsync("Someone's a narcissist. Ping someone else instead.");
                return;
            }

            if (user == null) {
                await ctx.RespondAsync("Please mention another user.");
                return;
            }

            var love = new Random().NextDouble() * 100;
            var loveIndex = (int)Math.Floor(love / 10);
            var loveLevel = "💖".Repeat(loveIndex) + "💔".Repeat(10 - loveIndex);

            var embed = new DiscordEmbedBuilder();
            embed.WithColor(new DiscordColor("#ffb6c1"));
            embed.WithTitle($"☁ Here's how much {ctx.Member.DisplayName} loves {user.DisplayName}:");
            embed.WithDescription($"💟 {Math.Floor(love)}%\n\n{loveLevel}");
            embed.WithFooter($"This info may or may not correct. {ctx.Guild.Name}, {Bot.client.CurrentUser.Username}, and/or {Bot.client.CurrentApplication.Owners.First().Username} are not responsible for any collateral damage. :P");

            await ctx.Channel.SendMessageAsync(embed);
        }
    }

    public static class StringExtensions {
        public static string Repeat(this string s, int n) {
            return new string(Enumerable.Range(0, n).SelectMany(_ => s).ToArray());
        }
    }
}
