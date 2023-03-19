using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

namespace HBot.Commands.Main {
    public class RoleCommand : BaseCommandModule {
        [Command("role")]
        [Description("Gives the user a specified role.")]
        [Usage("role")]
        [Category(Category.Main)]
        public async Task Role(CommandContext ctx, string roleName) { 
            if (ctx.Guild != Global.targetGuild) {
                throw new System.Exception($"This command can only be run in {Global.targetGuild.Name}.");
            }

            DiscordRole role = ctx.Guild.Roles.FirstOrDefault(r => r.Value.Name == roleName).Value;

            // Check if the role exists
            if (role == null) {
                await ctx.RespondAsync($"The {roleName} role does not exist.");
                return;
            }

            // Check if the role is one of the allowed roles
            if (roleName != "Announcements" && roleName != "Blog Posts" && roleName != "HIDNet Updates" && roleName != "Polls") {
                await ctx.RespondAsync("You can only add the Announcements, Blog Posts, HIDNet Updates, or Polls role.");
                return;
            }

            // Add the role to the user
            await ctx.Member.GrantRoleAsync(role);

            // Send a confirmation message
            await ctx.RespondAsync($"You have been given the {roleName} role!");
        }
    }
}