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
        [Usage("[mention | id | username]")]
        [Category(Category.Fun)]
        public async Task LoveCommandAsync(CommandContext context, DiscordMember user) {
            if (user == null || user.IsCurrent) {
                var members = await context.Guild.GetAllMembersAsync();
                user = members.Where(m => m.Id != context.User.Id).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            }

            if (user.IsCurrent) {
                await context.RespondAsync("Someone's a narcissist. Ping someone else instead.");
                return;
            }

            var lovePercent = Math.Floor(new Random().NextDouble() * 100);
            var loveIndex = (int)Math.Floor(lovePercent / 10);
            var loveLevel = "💖".Repeat(loveIndex) + "💔".Repeat(10 - loveIndex);

            var embed = new DiscordEmbedBuilder {
                Color = new DiscordColor("#ffb6c1"),
                Title = $"☁ Here's how much {context.Member.DisplayName} loves {user.DisplayName}:",
                Description = $"💟 {lovePercent}%\n\n{loveLevel}",
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    Text = $"This info may or may not correct. {context.Guild.Name}, {Bot.client.CurrentUser.Username}, and/or {Bot.client.CurrentApplication.Owners.First().Username} are not responsible for any collateral damage. :P"
                }
            };

            await context.Channel.SendMessageAsync(embed);
        }
    }

    public static class StringExtensions {
        public static string Repeat(this string s, int n) {
            return new string(Enumerable.Range(0, n).SelectMany(_ => s).ToArray());
        }      
    }
}
