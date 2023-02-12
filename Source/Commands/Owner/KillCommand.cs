using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using HBot.Commands.Attributes;

using HBot.Misc;

using Serilog;

namespace HBot.Commands.Owner
{
    public class KillCommand : BaseCommandModule
    {
        [Command("kill")]
        [Description("Kills the bot")]
        [Category(Category.Owner)]
        [RequireOwner]
        public async Task Kill(CommandContext Context)
        {
			await Context.ReplyAsync("Shutting down...");
			Log.Information("Shutdown triggered by command");
			//DailyReportSystem.CreateBackup();
            UserData.SaveData();
			Environment.Exit(0);
        }
    }
}