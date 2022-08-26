using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using WinBot.Commands.Attributes;
using WinBot.Util;

namespace WinBot.Commands.Main
{
    public class ServerInfoCommand : BaseCommandModule
    {
        [Command("serverinfo")]
        [Description("Gets info about the current server")]
        [Category(Category.Main)]
        [Aliases("guild", "guildinfo", "server")]
        public async Task ServerInfo(CommandContext Context, string mode = null)
        {
            if (mode == "emojis") {
                DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
                int animatedEmojis = 0;
                int staticEmojis = 0;
                foreach (DiscordEmoji emoji in Context.Guild.Emojis.Values) {
                    if (emoji.IsAnimated) animatedEmojis += 1;
                    else staticEmojis += 1;
                }
                eb.WithDescription($@"```cs
Emojis: {Context.Guild.Emojis.Count}
-[Anim]: {animatedEmojis}
-[Regular]: {staticEmojis}```");
                await Context.ReplyAsync("", eb.Build());
            } else if (mode == "extended") {
            DiscordGuild Guild = Context.Guild;
            DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
            int animatedEmojis = 0;
            int staticEmojis = 0;
            foreach (DiscordEmoji emoji in Context.Guild.Emojis.Values) {
                if (emoji.IsAnimated) animatedEmojis += 1;
                else staticEmojis += 1;
            }
            string[] channelTypes = new string[9] {
                "Category", "Group", "News", "Private", "Stage", "Store", "Text", "Unknown", "Voice"
            };
            int[] types = new int[9]{0, 0, 0, 0, 0, 0, 0, 0, 0};
            foreach (DiscordChannel channel in Guild.Channels.Values) {
                switch (channel.Type.ToString()) {
                    case "Category":
                        types[0] += 1;
                        break;
                    case "Group":
                        types[1] += 1;
                        break;
                    case "News":
                        types[2] += 1;
                        break;
                    case "Private":
                        types[3] += 1;
                        break;
                    case "Stage":
                        types[4] += 1;
                        break;
                    case "Store":
                        types[5] += 1;
                        break;
                    case "Text":
                        types[6] += 1;
                        break;
                    case "Unknown":
                        types[7] += 1;
                        break;
                    case "Voice":
                        types[8] += 1;
                        break;
                    
                }
            }

            string channelCounts = "";
            for (int i = 0; i < 9; i++) {
                if (types[i] > 0)
                    channelCounts += $" -[{channelTypes[i]}]: {types[i]}\n";
            }

            string guildTier = "";
            string[] limits = new string[7];
            switch (Guild.PremiumTier.ToString()) {
                case "None":
                    guildTier = "No level";
                    limits = new string[7]{"8 MB", "96 kbps", "100", "50", "50", "500,000", "5,000"};
                    break;
                case "Tier_1":
                    guildTier = "Level 1";
                    limits = new string[7]{"8 MB", "128 kbps", "200", "100", "100", "500,000", "5,000"};
                    break;
                case "Tier_2":
                    guildTier = "Level 2";
                    limits = new string[7]{"50 MB", "256 kbps", "300", "150", "150", "500,000", "5,000"};
                    break;
                case "Tier_3":
                    guildTier = "Level 3";
                    limits = new string[7]{"100 MB", "384 kbps", "500", "250", "250", "500,000", "5,000"};
                    break;
                case "Unknown":
                    guildTier = "Unknown";
                    break;
                default:
                    guildTier = Guild.PremiumTier.ToString();
                    break;
            }
            string limitsString = $@"```cs
            Attachment: {limits[0]}
            Bitrate: {limits[1]}
            Emojis: {limits[2]}
             - [Anim]: {limits[3]}
             - [Regular]: {limits[4]}
             ```";
            string contentFilter = "";
            switch (Guild.ExplicitContentFilter.ToString()) {
                case "AllMembers":
                    contentFilter = "All members";
                    break;
                case "Disabled":
                    contentFilter = "Disabled";
                    break;
                case "MembersWithoutRoles":
                    contentFilter = "Members without roles";
                    break;
                default:
                    contentFilter = "Unknown";
                    break;
            }

            int[] roles = new int[2]{0, 0};
            foreach (DiscordRole role in Guild.Roles.Values) {
                if (role.IsManaged) roles[0] += 1;
                else roles[1] += 1;
            }

            string features = "";
            foreach (string feature in Guild.Features) {
                features += feature + "\n";
            }

            features = parseFeatures(features);

            eb.WithColor(DiscordColor.Gold);
            eb.WithTitle(Guild.Name);
            eb.WithThumbnail(Guild.IconUrl);
            /*eb.AddField("Users", $"{Context.Guild.MemberCount}", true);
            eb.AddField("Created", MiscUtil.FormatDate(Context.Guild.CreationTimestamp), true);
            eb.AddField("Channels", $"{Context.Guild.Channels.Count}", true);
            eb.AddField("Owner", Context.Guild.Owner.Username, true);
            eb.AddField("Emojis", $"**Static:** {staticEmojis}\n**Animated:** {animatedEmojis}");*/
            eb.WithAuthor(Guild.Name, "", Guild.IconUrl);
            eb.AddField("Information", $@"
            **Created:** <t:{Guild.CreationTimestamp.ToUnixTimeSeconds()}:R>
            **->** <t:{Guild.CreationTimestamp.ToUnixTimeSeconds()}:f>
            **ID:** {Guild.Id}
            **Locale:** {Guild.PreferredLocale}
            **Nitro Tier:** {guildTier}
            **Owner:** {Guild.Owner.Mention}
            **Large Server:** {(Guild.IsLarge ? "Yes" : "No")}", true);

            eb.AddField("Moderation", $@"**AFK Timeout:** {Guild.AfkTimeout} seconds)
            **Content Filter:** {contentFilter}
            **Message Notifs:** {(Guild.DefaultMessageNotifications.ToString() == "AllMessages" ? "All messages" : "Mentions")}
            **MFA:** {(Guild.MfaLevel.ToString() == "Enabled" ? "Mandatory" : "Optional")}
            **Verification:** {Guild.VerificationLevel.ToString()}", true);

            eb.AddField("Channels", $@"**Default:** {Guild.GetDefaultChannel().Mention}
            **System:** {Guild.SystemChannel.Mention}");

            eb.AddField("Counts", $@"```cs
Channels: {Guild.Channels.Count}
{channelCounts}Emojis: {Guild.Emojis.Count}
 -[Anim]: {animatedEmojis}
 -[Regular]: {staticEmojis}
```", true);
            eb.AddField("Counts", $@"```cs
Boosts: {Guild.PremiumSubscriptionCount}
Members: {Guild.MemberCount}
Roles: {Guild.Roles.Count}
 - [Managed] {roles[0]}
 - [Regular] {roles[1]}
```", true);
            eb.AddField("Limits", limitsString);

            eb.AddField("Features", $@"```
{features}
```");
            
            eb.AddField("URLs", $"**[Icon Image]({Guild.IconUrl})**, **[Splash Image]({Guild.SplashUrl})**");

            eb.WithImageUrl(Guild.BannerUrl);
            
            await Context.ReplyAsync("", eb.Build());
            
        } else {
            await Context.ReplyAsync("h");
        }
        }

        public static string parseFeatures (string features) {
            features = features.Replace("INVITE_SPLASH", "Invite Splash");
            features = features.Replace("NEW_THREAD_PERMISSIONS", "New Thread Permissions");
            features = features.Replace("THREE_DAY_THREAD_ARCHIVE", "Three Day Thread Archive");
            features = features.Replace("ANIMATED_ICON", "Animated Icon");
            features = features.Replace("HAD_EARLY_ACTIVITIES_ACCESS", "Had Early Activities Access");
            features = features.Replace("EXPOSED_TO_ACTIVITIES_WTP_EXPERIMENT", "WTP Experiment");
            return features;
        }
    }
}
