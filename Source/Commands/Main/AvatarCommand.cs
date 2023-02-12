using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Util;
using HBot.Commands.Attributes;

namespace HBot.Commands.Main
{
    public class AvatarCommand : BaseCommandModule
    {
        [Command("avatar")]
        [Description("Gets a user's avatar")]
        [Aliases("pfp", "php")]
        [Usage("[user]")]
        [Category(Category.Main)]
        public async Task Avatar(CommandContext Context, [RemainingText] DiscordMember user)
        {
            if (user == null) 
                user = Context.Message.Author as DiscordMember;

            try {
                if (user.AvatarUrl != null)
                    await Context.ReplyAsync(user.AvatarUrl);
                else
                    await Context.ReplyAsync(user.DefaultAvatarUrl);
            }
            catch (Exception ex) {
                await Context.ReplyAsync("Error: " + ex.Message + "\nStack Trace:" + ex.StackTrace);
            }
        }
    }
}
