using System;
using System.Net;
using System.Web;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using WinBot.Commands.Attributes;

using Newtonsoft.Json;

namespace WinBot.Commands.Fun
{
    public class WWSearchCommand : BaseCommandModule
    {
        [Command("wwsearch")]
        [Description("Gets a random picture of rory")]
        [Category(Category.Main)]
        public async Task WWSearch(CommandContext Context, [RemainingText] string query = null)
        {
            if (query == null) {
                await Context.ReplyAsync("No query specified!");
                return;
            }
            query = query.Replace("@", "@~");
            string encQ = HttpUtility.UrlEncode(query);
            string json = "";
            string output = "";
            string pl;
            
            // Grab the json string from the API
            using (WebClient client = new WebClient())
                json = client.DownloadString($"http://wwapi.diskfloppy.me/search?query={encQ}");
            dynamic results = JsonConvert.DeserializeObject(json); // Deserialize the string into a dynamic object
            int count = 0;
			DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
            foreach (dynamic result in results) count += 1;
            if (count == 1) pl = "result";
            else pl = "results";
            if (count > 10) {
                eb.WithTitle($"Top 10 results in WinWorld for \"{query}\"");
                for (int i = 0; i < 10; i++) {
                    output += $"[**{results[i].title}**]({results[i].url})\n";
                }
            } else {
                foreach (dynamic result in results) {
                    eb.WithTitle($"{count} {pl} in WinWorld for \"{query}\"");
                    output += $"[**{result.title}**]({result.url})\n";
                }
            }
            // Send the image in an embed
            eb.WithDescription(output);
            eb.WithFooter($"{count} results total (first page, max 25)", "http://www.nickandfloppy.com/bookmarks/icons/winworld.png");
			eb.WithColor(DiscordColor.Gold);
            if (count != 0) {await Context.ReplyAsync("", eb.Build()); return;}
            DiscordEmbedBuilder emb = new DiscordEmbedBuilder();
            emb.WithTitle($"No results for `{query}`");
            emb.WithColor(DiscordColor.Gold);
            await Context.ReplyAsync("", emb.Build());
        }
    }
}