using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;


namespace HBot.Commands.Fun {
    public class SealCommand : BaseCommandModule {
        [Command("seal")]
        [Description("Gets a random seal photo")]
        [Category(Category.Fun)]
            public async Task Seal(CommandContext Context) {

            Random random = new Random();
            int randomNumber = random.Next(1, 83);

            string formattedNumber = randomNumber.ToString("D4");

            string imageName = $"{formattedNumber}";
            string encodedImageName = Uri.EscapeDataString(imageName);
        
            string url = $"https://focabot.github.io/random-seal/seals/{encodedImageName}.jpg";

            var eb = new DiscordEmbedBuilder();
            eb.WithColor(new DiscordColor("#007FFF"));
            eb.WithTitle("Here's your seal!");
            eb.WithImageUrl(url);
            eb.WithTimestamp(DateTime.Now);
            await Context.ReplyAsync("", eb.Build());
            await Task.Delay(1);
        }
    }
}