using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;


using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using WinBot.Misc;
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
			MCServer server = new MCServer();

			foreach (MCServer currentServer in Bot.config.minecraftServers) {
				if (Context.Guild.Id == currentServer.guildID) {
					server = currentServer;
					break;
				}
			}
			if (server.guildID == 0)
				throw new Exception("This server does not have a server configured");

			await Context.Channel.TriggerTypingAsync();

			// Download the server info
			string json = "";
			using(HttpClient http = new HttpClient())
				json = await http.GetStringAsync($"https://api.mcsrvstat.us/2/{server.address}");

			dynamic serverInfo = JsonConvert.DeserializeObject(json);
			
			// Format the info in an embed
			DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
			eb.WithColor(DiscordColor.Gold);
			
			// Set up the embed
			if((bool)serverInfo.online) {
				eb.WithThumbnail(Context.Guild.IconUrl);
				eb.WithTitle(WebUtility.HtmlDecode((string)serverInfo.motd.clean[0]));
				eb.AddField("Address", server.address, true);
				eb.AddField("Versions", server.versions, true);
				if (server.dynmap != null)
					eb.AddField("Dynmap", server.dynmap, true);
				eb.AddField("Online?", ((bool)serverInfo.online) ? "Yes" : "No", true);
				eb.AddField("Users Count", $"{(int)serverInfo.players.online}/{(int)serverInfo.players.max}", true);
				if((int)serverInfo.players.online > 0) {
					eb.AddField("Users", $"{string.Join('\n', serverInfo.players.list)}", true);
				}
				eb.AddField("Supports Cracked Accounts?", server.crackedInfo, true);
			}
			else {
				eb.WithTitle("Server is Offline!");
				eb.WithColor(DiscordColor.Red);
			}

			await Context.ReplyAsync("", eb.Build());
		}
	}
}

