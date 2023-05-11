using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using HBot.Commands.Attributes;
using Newtonsoft.Json.Linq;

namespace HBot.Commands.Fun {
    public class CatCommand : BaseCommandModule {
        [Command("cat")]
        [Description("Gets a random cat photo")]
        [Category(Category.Fun)]
        public async Task Cat(CommandContext Context) {
            string json = "";
            using (HttpClient http = new HttpClient()) {
                try {
                    json = await http.GetStringAsync("http://localhost/api/hbot/random-cat");
                }
                catch (Exception ex) {
                    await Context.Channel.SendMessageAsync($"Error fetching cat image: {ex.Message}");
                    return;
                }
            }

            JObject data = JObject.Parse(json);
            if (data == null || string.IsNullOrEmpty(data.Value<string>("img_url"))) {
                await Context.Channel.SendMessageAsync("Error fetching cat image: the API shat likely itself");
                return;
            }

            try {
                byte[] imageData = Convert.FromBase64String(data.Value<string>("img_url").Split(',')[1]);
                using (var ms = new MemoryStream(imageData)) {
                    var attachment = new DSharpPlus.Entities.DiscordMessageBuilder().WithFile("cat.jpg", ms);
                    await Context.Channel.SendMessageAsync(attachment); //Have to remove the "Here's your cat!" message for now because of DPP bullshittery
                }
            }
            catch (Exception ex) {
                await Context.Channel.SendMessageAsync($"Error sending cat image: {ex.Message}");
                return;
            }
        }
    }
}
