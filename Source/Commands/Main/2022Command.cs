using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using WinBot.Commands.Attributes;

namespace WinBot.Commands.Main
{
    public class YearCommand : BaseCommandModule
    {
        [Command("2022")]
        [Description("Gets the remaining time in 2022")]
        [Category(Category.Fun)]
        public async Task Ping(CommandContext Context)
        {
            var timeSpan = new DateTime(2023, 1, 1).Subtract(DateTime.Now);
            await Context.ReplyAsync($"There are {Math.Round((double)timeSpan.TotalDays,2)} days left in 2022. That's {Math.Round((double)timeSpan.TotalHours,2)} hours, {Math.Round((double)timeSpan.TotalMinutes,2)} minutes, or {Math.Round((double)timeSpan.TotalSeconds,2)} seconds.");
        }
    }
}