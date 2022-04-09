using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using WinBot.Commands.Attributes;

namespace WinBot.Commands.Main
{
    public class UptimeCommand : BaseCommandModule
    {
        [Command("uptime")]
        [Description("Gets the bot's uptime")]
        [Category(Category.Main)]
        public async Task Ping(CommandContext Context)
        {
            /*await Context.ReplyAsync($"Uptime: **{Convert.ToInt32(Bot.sw.ElapsedMilliseconds)}**");*/
			
			TimeSpan t = TimeSpan.FromMilliseconds(Convert.ToInt32(Bot.sw.ElapsedMilliseconds));
			string uptime = "";
		    if (t.Days != 00) uptime += $"{Convert.ToString(t.Days)}d ";
			if (t.Hours != 00) uptime += $"{Convert.ToString(t.Hours)}h ";
			if (t.Minutes != 00) uptime += $"{Convert.ToString(t.Minutes)}m ";
			if (t.Seconds != 00) uptime += $"{Convert.ToString(t.Seconds)}s";

			await Context.ReplyAsync($"Uptime: **{uptime}**");
        }
    }
}