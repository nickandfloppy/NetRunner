using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using WinBot.Util;
using WinBot.Commands.Attributes;

namespace WinBot.Commands.Main
{
    public class WhoisCommand : BaseCommandModule
    {
        [Command("whois")]
        [Description("Gets basic info about a user")]
        [Aliases("user")]
        [Usage("[user]")]
        [Category(Category.Main)]
        public async Task Whois(CommandContext Context, [RemainingText] DiscordMember user)
        {
            if (user == null) 
                user = Context.Message.Author as DiscordMember;

            try {
                // Set up the embed
                DiscordEmbedBuilder Embed = new DiscordEmbedBuilder();
                Embed.WithColor(DiscordColor.Gold);   

                // Basic user info
                if (user.AvatarUrl != null) {
                    Embed.WithThumbnail(user.AvatarUrl);
                    Embed.WithAuthor(user.Username, null, user.AvatarUrl);
                }
                else {
                    Embed.WithThumbnail(user.DefaultAvatarUrl);
                    Embed.WithAuthor(user.Username, null, user.DefaultAvatarUrl);
                }
                string isBot = user.IsBot ? "Yes" : "No";
                string isOwner = user.IsOwner ? "Yes" : "No";

                Embed.AddField("**Information**", $"**ID:** {user.Id.ToString()}\n**Bot:** {isBot}\n**Badges:** {ParseFlags(user.Flags)}", true);

                // Embed dates
                Embed.AddField("**Joined**", $"**Discord:** {(int)DateTime.Now.Subtract(user.CreationTimestamp.DateTime).TotalDays} days ago\n**->**<t:{user.CreationTimestamp.ToUnixTimeSeconds()}:f>\n**Guild:** {(int)DateTime.Now.Subtract(user.JoinedAt.DateTime).TotalDays} days ago\n**->**<t:{user.JoinedAt.ToUnixTimeSeconds()}:f>", true);

                // User roles
                string roles = "";
                int roleCount = 0;
                foreach (DiscordRole role in user.Roles) {
                    roles += role.Mention + ", ";
                    roleCount += 1;
                }
                roles = roles.Substring(0, roles.Length - 2);
                Embed.AddField("Guild Specific", $"**Nickname:** {user.Nickname}\n**Roles ({roleCount}): {roles}**\n**Owner:** {isOwner}");

                await Context.ReplyAsync("", Embed.Build());
            }
            catch (Exception ex) {
                await Context.ReplyAsync("Error: " + ex.Message + "\nStack Trace:" + ex.StackTrace);
            }
        }

        public static string ParseFlags(Enum flags) {
            string flagString = flags.ToString();
            flagString = flagString.Replace("BugHunterLevelOne", "<:bughunter_1:980875953301508137> ");
            flagString = flagString.Replace("BugHunterLevelTwo", "<:bughunter_2:980875953192468630> ");
            flagString = flagString.Replace("DiscordCertifiedModerator", "");
            flagString = flagString.Replace("DiscordEmployee", "<:staff:980875953263751168> ");
            flagString = flagString.Replace("DiscordPartner", "<:partner:980875953339236372> ");
            flagString = flagString.Replace("EarlySupporter", "<:early_supporter:980875953213431828> ");
            flagString = flagString.Replace("HouseBalance", "<:balance:980875953116938290> ");
            flagString = flagString.Replace("HouseBravery", "<:bravery:980875953167290429> ");
            flagString = flagString.Replace("HouseBrilliance", "<:brilliance:980875953217630208> ");
            flagString = flagString.Replace("HypeSquadEvents", "<:hypesquad_events:980875953263759441> ");
            flagString = flagString.Replace("System", "");
            flagString = flagString.Replace("TeamUser", "");
            flagString = flagString.Replace("VerifiedBot", "");
            flagString = flagString.Replace("VerifiedBotDeveloper", "<:developer:980875953301508139> ");
            
            return flagString.Substring(0, flagString.Length - 1);
        }
    }
}