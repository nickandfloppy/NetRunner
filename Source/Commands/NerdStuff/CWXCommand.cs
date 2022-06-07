using System;
using System.Net;
using System.Globalization;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using WinBot.Util;
using WinBot.Commands.Attributes;

namespace WinBot.Commands.NerdStuff
{
    public class CWXCommand : BaseCommandModule
    {
        [Command("cwx")]
        //[Description("Sends a map of radio MUF")]
        [Usage("[yyyy-mm-dd")]
        [Category(Category.NerdStuff)]
        public async Task CWX(CommandContext Context, string date = null)
        {
            DiscordEmbedBuilder Embed = new DiscordEmbedBuilder();
            DateTime result = DateTime.Now;
            DateTime firstRecord = new DateTime(2017, 03, 19);
            if (date == null) {
                string day = result.Day < 10 ? $"0{result.Day}" : $"{result.Day}";
                string month = result.Month < 10 ? $"0{result.Month}" : $"{result.Month}";
                date = $"{result.Year}-{month}-{day}";
            } else {
                try {
                    result = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                } catch {
                    await Context.ReplyAsync("Must be in yyyy-MM-dd format!");
               } finally {
                    string day = result.Day < 10 ? $"0{result.Day}" : $"{result.Day}";
                    string month = result.Month < 10 ? $"0{result.Month}" : $"{result.Month}";
                    date = $"{result.Year}-{month}-{day}";
                }
            }
            if (result <= firstRecord) { await Context.ReplyAsync("Date specified is before first record (2017-03-19)"); return; }
            else if (result > DateTime.Now) { await Context.ReplyAsync("Date specified is in the future"); return; }
            Embed.WithTitle($"CWX Outlook for {date}");
            Embed.WithColor(DiscordColor.Gold);
            Embed.WithDescription($"[Full Forecast](http://www.convectiveweather.co.uk/forecast.php?date={date})");
            Embed.WithImageUrl($"http://www.convectiveweather.co.uk/largethumb.php?date={date}");
            await Context.ReplyAsync("", Embed.Build());
        }
    }
}
