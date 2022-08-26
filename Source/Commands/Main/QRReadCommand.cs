using System.Web;
using System.Net;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using WinBot.Commands.Attributes;
using WinBot.Util;

using Newtonsoft.Json;

namespace WinBot.Commands.Fun
{
    public class QRCommand : BaseCommandModule
    {
        [Command("qrread")]
        [Description("Decodes the provided QR code")]
        [Category(Category.Main)]
        public async Task QR(CommandContext Context, [RemainingText] string imageURL = null)
        {
            if (imageURL == null) return;
            string encodedURL = HttpUtility.UrlEncode(imageURL);
            string json = "";
            // Grab the json string from the API
            using (WebClient client = new WebClient())
                json = client.DownloadString($"http://api.qrserver.com/v1/read-qr-code/?fileurl={encodedURL}");
            dynamic output = JsonConvert.DeserializeObject(json); // Deserialize the string into a dynamic object

            string type = output[0]["type"];
            string data = output[0]["symbol"][0]["data"];
            if (data.Length > 255) data = MiscUtil.Truncate(data, 255);

            // Send the image in an embed
			DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
			eb.WithTitle("QR Reader");
			eb.WithColor(DiscordColor.Gold);
			eb.WithFooter($"Type: {type}");
            eb.AddField("Data", data);
			eb.WithThumbnail(imageURL);
			await Context.ReplyAsync("", eb.Build());
        }
    }
}