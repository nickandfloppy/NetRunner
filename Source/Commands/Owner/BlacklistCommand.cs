using System.IO;
using System.Linq;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using static WinBot.Util.ResourceManager;
using WinBot.Commands.Attributes;

using Newtonsoft.Json;

namespace WinBot.Commands.Staff
{
    public class BlacklistCommand : BaseCommandModule
    {
        [Command("blacklist")]
        [Description("Toggle blacklist on a user to prevent them from using the bot")]
        [Usage("[user]")]
        [Category(Category.Owner)]
        [RequireOwner]
        public async Task Blacklist(CommandContext Context, DiscordMember user = null)
        {
            if(!PermissionMethods.HasPermission(Context.Member.PermissionsIn(Context.Channel), Permissions.KickMembers) && Context.Member.Id != Bot.client.CurrentApplication.Owners.FirstOrDefault().Id)
                return;
            //if (!(Context.User.Id == 437970062922612737) && !(Context.User.Id == 584532167002947604) && !(Context.User.Id == 404817718542598154) && !(Context.User.Id == 256033911438573568) && !(Context.User.Id == 447380526798471170))
            //    return;

            // This is a clunky method of listing things but it works
            if(user == null) {
                string list = "";
                foreach(ulong blacklistuser in Global.blacklistedUsers) {
                    string username = $"{blacklistuser}";
                    try {
                    if(Context.Guild.GetMemberAsync(blacklistuser).Result != null)
                        username = Context.Guild.GetMemberAsync(blacklistuser).Result.Username;
                    } catch{}
                    list += username + "\n";
                }
                if(Global.blacklistedUsers.Count <= 0)
                    list = "There are no blacklisted users.";

                DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
                eb.WithTitle("Blacklist");
                eb.WithColor(DiscordColor.Gold);
                eb.WithDescription($"```\n{list}```");
                await Context.ReplyAsync("", eb.Build());
                return;
            }

            // Blacklist/unblacklist
            if(Global.blacklistedUsers.Contains(user.Id)) {
                Global.blacklistedUsers.Remove(user.Id);
                File.WriteAllText(GetResourcePath("blacklist", Util.ResourceType.JsonData), JsonConvert.SerializeObject(Global.blacklistedUsers, Formatting.Indented));
                await Context.ReplyAsync($"Unblacklisted {user.Username}#{user.Discriminator}!");
            }
            else {
                Global.blacklistedUsers.Add(user.Id);
                File.WriteAllText(GetResourcePath("blacklist", Util.ResourceType.JsonData), JsonConvert.SerializeObject(Global.blacklistedUsers, Formatting.Indented));
                await Context.ReplyAsync($"Blacklisted {user.Username}#{user.Discriminator}!");
            }
        }
    }
}