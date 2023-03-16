using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

namespace HBot.Commands.Main {
    public class ColorCommand : BaseCommandModule {
        [Command("color")]
        [Description("Gets info about the given hex color")]
        [Usage("[hex value]")]
        [Category(Category.Main)]
        public async Task Color(CommandContext Context, [RemainingText] string hexString = null) {
            if (hexString == null) await Context.ReplyAsync("No hex value specified!");
            else {
                DiscordColor color;
                try {
                    color = new DiscordColor(hexString);
                } catch {
                    await Context.ReplyAsync("Must be in format #000000 (# is optional) and be valid HEX");
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
