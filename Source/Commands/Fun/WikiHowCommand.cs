using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

using RestSharp;

using Newtonsoft.Json;

namespace HBot.Commands.Fun {
    public class WikiHowCommand : BaseCommandModule
    {
        [Command("wikihow")]
        [Description("Gets a random out of context wikihow image")]
        [Category(Category.Fun)]
        public async Task WikiHow(CommandContext Context) {
            var client = new RestClient("https://hargrimm-wikihow-v1.p.rapidapi.com/images?count=1");
            var request = new RestRequest();
            request.AddHeader("x-rapidapi-key", Bot.config.apiKeys.wikihowAPIKey);
            request.AddHeader("x-rapidapi-host", "hargrimm-wikihow-v1.p.rapidapi.com");
            RestResponse response = (RestResponse)client.Execute(request);
            dynamic resp = JsonConvert.DeserializeObject(response.Content);

            // Create and send the embed
            var eb = new DiscordEmbedBuilder();
            eb.WithColor(DiscordColor.Gold);
            eb.WithTitle("Here's your random WikiHow image!");
            eb.WithImageUrl((string)resp["1"]);
            eb.WithTimestamp(DateTime.Now);
            await Context.ReplyAsync("", eb.Build());
            await Task.Delay(1);
        }
    }
}