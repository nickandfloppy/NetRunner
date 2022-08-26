using System;
using System.Net;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using WinBot.Commands.Attributes;

using Newtonsoft.Json;

namespace WinBot.Commands.Fun
{
    public class PlexStatsCommand : BaseCommandModule
    {
        [Command("plexstats")]
        [Category(Category.Fun)]
        public async Task PlexStats(CommandContext Context)
        {
            string json = "";
            string json2 = "";
            // Grab the json string from the API

            using (WebClient client = new WebClient())
                json2 = client.DownloadString("https://tautulli.diskfloppy.me/api/v2?apikey=d1d9b5be5d994d758b6a5b27e081dbc5&cmd=get_server_friendly_name");
            dynamic out2 = JsonConvert.DeserializeObject(json2); // Deserialize the string into a dynamic object
            string servername = out2.response.data;

            using (WebClient client = new WebClient())
                json = client.DownloadString("https://tautulli.diskfloppy.me/api/v2?apikey=d1d9b5be5d994d758b6a5b27e081dbc5&cmd=get_libraries");
            dynamic output = JsonConvert.DeserializeObject(json); // Deserialize the string into a dynamic object

            string op = "";
            /*foreach (dynamic library in output.response.data) {
                op += $"**{library.section_name}** - {library.count}\n";
            }*/
            dynamic libraries = output.response.data;
            foreach (dynamic library in libraries){
                if (library.section_id != "2") {
                    if (library.section_id == "3") {
                        op += $"\n**{library.section_name}**\n{library.count} shows, {library.parent_count} series, {library.child_count} episodes";
                    } else {
                        op += $"\n**{library.section_name}**\n{library.count} artists, {library.parent_count} albums, {library.child_count} tracks";
                    }
                }
            }
            //op = $"**{music.section_name}**\n{music.count} artists, {music.parent_count} albums, {music.child_count} tracks";
            
            // Send the image in an embed
			DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
			eb.WithTitle($"Library stats for {servername}");
			eb.WithColor(DiscordColor.Gold);
            eb.WithTimestamp(DateTime.Now);
			//eb.WithFooter($"ID: {output.id}");
			eb.WithDescription(op);
			await Context.ReplyAsync("", eb.Build());
        }
    }
}