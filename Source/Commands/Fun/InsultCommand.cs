using System.Threading.Tasks;
using System.Net.Http;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

using Newtonsoft.Json;

namespace HBot.Commands.fun {
    public class InsultCommand : BaseCommandModule {
        [Command("insult")]
        [Description("Insults you. Fun.")]
        [Category(Category.Fun)]
        public async Task Insult(CommandContext Context) {
            DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
            // Get the thing
            string json = "";
            using(HttpClient http = new HttpClient())
            json = await http.GetStringAsync($"http://evilinsult.com/generate_insult.php?lang=en&type=json");

            dynamic insultText = JsonConvert.DeserializeObject(json);  // Deserialize the thang
                // Build the thang
                eb.WithTitle("Get insulted... loser.");
                eb.WithColor(DiscordColor.Gold);
                eb.WithDescription($"{insultText.insult}");
                
            // Send the thing
			await Context.ReplyAsync("", eb.Build());
		}
	}
}
