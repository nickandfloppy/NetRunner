using System.Threading.Tasks;
using System.Net.Http;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using WinBot.Commands.Attributes;

using Newtonsoft.Json;

namespace WinBot.Commands.fun {
	public class InsultCommand : BaseCommandModule {
        [Command("insult")]
		[Description("Insults you. Fun.")]
		[Usage("insult")]
		[Category(Category.Fun)]
		public async Task Insult(CommandContext Context) {
			DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
            // Get the thing
            string json = "";
            using(HttpClient http = new HttpClient())
                json = await http.GetStringAsync($"http://evilinsult.com/generate_insult.php?lang=en&type=json");

            dynamic insultText = JsonConvert.DeserializeObject(json);  // Deserialize the thang
                // Send the thing
                eb.WithTitle("You got insulted. Ha...");
                eb.WithColor(new DiscordColor("#02cc09"));
                eb.WithDescription($"{insultText.insult}");
		
			await Context.ReplyAsync("", eb.Build());
		}
	}
}
