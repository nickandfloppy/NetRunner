using System;
using System.Net.Http;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

using Newtonsoft.Json;

namespace HBot.Commands.Fun
{
    public class CatCommand : BaseCommandModule
    {
        [Command("cat")]
        [Description("Gets a random cat photo")]
        [Category(Category.Fun)]
        public async Task Cat(CommandContext Context)
        {
            string json = "";
            // Download the json string from the API
            using(HttpClient http = new HttpClient())
            json = await http.GetStringAsync($"https://api.thecatapi.com/v1/images/search?api_key={Bot.config.apiKeys.catAPIKey}");
            dynamic output = JsonConvert.DeserializeObject(json); // Deserialize the string into a dynamic object

            // Create and send the embed
            var eb = new DiscordEmbedBuilder();
            eb.WithColor(new DiscordColor("#007FFF"));
            eb.WithTitle("Here's your random cat!");
            eb.WithImageUrl((string)output[0].url);
            eb.WithTimestamp(DateTime.Now);
            await Context.ReplyAsync("", eb.Build());
            await Task.Delay(1);
        }
    }
}