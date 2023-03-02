using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

namespace HBot.Commands.Main {
    public class AvatarCommand : BaseCommandModule {
        [Command("avatar")]
        [Description("Gets a user's avatar")]
        [Aliases("pfp", "php")]
        [Usage("[user]")]
        [Category(Category.Main)]
        public async Task Avatar(CommandContext Context, [RemainingText] DiscordMember user) {
            if (user == null) 
                user = Context.Message.Author as DiscordMember;

            DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
            eb.WithTitle($"{user.DisplayName}'s avatar");
            eb.WithColor(DiscordColor.Gold);
            if (user.AvatarUrl != null)
                eb.WithImageUrl(user.AvatarUrl);
            else {
                eb.WithImageUrl(user.DefaultAvatarUrl);
            }
            await Context.RespondAsync("", eb.Build());
        }
    }
}
