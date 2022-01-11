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
			string answer = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", 
                        t.Days,
						t.Hours, 
                        t.Minutes, 
                        t.Seconds);
			await Context.ReplyAsync($"Uptime: **{answer}**");
        }
    }
}