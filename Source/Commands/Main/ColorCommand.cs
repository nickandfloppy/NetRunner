
using System;
using System.Timers;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using WinBot.Commands.Attributes;

namespace WinBot.Commands.Main
{
    public class ColorCommand : BaseCommandModule
    {
        [Command("color")]
        [Description("Remind you about something")]
        [Usage("[hex value]")]
        [Category(Category.Main)]
        public async Task Remind(CommandContext Context, [RemainingText] string hexString = null)
        {
            if (hexString == null) await Context.ReplyAsync("No hex value specified!");
            else {
                DiscordColor color;
                try {
                    color = new DiscordColor(hexString);
                } catch {
                    await Context.ReplyAsync("Must be in format #000000 (# is optional) and be valid hex");
                    return;
                }
		        DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
                eb.WithThumbnail($"https://singlecolorimage.com/get/{hexString}/100x100");
                if (!hexString.StartsWith("#")) hexString = $"#{hexString}";
                eb.WithTitle($"Color Info");
                eb.WithDescription($"**HEX Value:** {hexString.ToUpper()}\n**RGB Value:** ({color.R}, {color.G}, {color.B})\n**Color Value:** {color.Value}");
                eb.WithColor(color);
                await Context.ReplyAsync("", eb.Build());
            }
        }
    }
}
