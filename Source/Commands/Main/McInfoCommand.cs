using System;
using System.Net.Http;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using WinBot.Commands.Attributes;

using Newtonsoft.Json;

namespace WinBot.Commands.Main
{
    public class McInfoCommand : BaseCommandModule
    {
        [Command("mcinfo")]
        [Description("Gets information on the Minecraft server")]
        [Category(Category.Main)]
        public async Task McInfo(CommandContext Context)
        {
            string server = "";
            string dynmap = "";
            string versions = "";
            if (Context.Guild.Id == 764493398983049246) {
                server = "mc.nick99nack.com";
                dynmap = "http://mc.nick99nack.com/";
                versions = "1.7.2 -> 1.16.5";
            } else if (Context.Guild.Id == 955969771994742874) {
                server = "hiden.pw";
                dynmap = "https://mc.hiden.pw/";
                versions = "1.7.x -> 1.18.x";
            } else if (Context.Guild.Id == 936271948927881276) {
                server = "mc.nick99nack.com:25560";
                versions = "1.16.5";
            }else
                throw new Exception("This server does not have a server configured");

            await Context.Channel.TriggerTypingAsync();

            // Download the server info
            string json = "";
            using(HttpClient http = new HttpClient())
                json = await http.GetStringAsync($"https://api.mcsrvstat.us/2/{server}");

            dynamic serverInfo = JsonConvert.DeserializeObject(json);
            
            // Format the info in an embed
            DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
            eb.WithColor(DiscordColor.Gold);
            
			// Set up the embed
            if((bool)serverInfo.online) {
                eb.WithThumbnail(Context.Guild.IconUrl);
                eb.WithTitle((string)serverInfo.motd.clean[0]);
                eb.AddField("Address", server, true);
                eb.AddField("Versions", versions, true);
                if (dynmap != "")
		            eb.AddField("Dynmap", dynmap, true);
                eb.AddField("Online?", ((bool)serverInfo.online) ? "Yes" : "No", true);
                eb.AddField("Users Count", $"{(int)serverInfo.players.online}/{(int)serverInfo.players.max}", true);
                if((int)serverInfo.players.online > 0) {
					eb.AddField("Users", $"{string.Join('\n', serverInfo.players.list)}", true);
				}
                eb.AddField("Supports Cracked Accounts?", "No. It never will, just buy the game or stop asking.", true);
            }
            else {
                eb.WithTitle("Server is Offline!");
                eb.WithColor(DiscordColor.Red);
            }

            await Context.ReplyAsync("", eb.Build());
        }
    }
}
