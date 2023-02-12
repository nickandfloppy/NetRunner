using System.Linq;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using Serilog;

namespace HBot.Misc
{
    class No
    {
        public static void Init()
        {
            Bot.client.MessageCreated += OnMessage;
        }

        private static async Task OnMessage(DiscordClient sender, MessageCreateEventArgs e)
        {
            if(!e.Message.Content.Contains("fheiw87y3hfihgdsgf") && e.MentionedUsers.FirstOrDefault(x => x.Id == 283982771997638658) == null)
                return;

            DiscordWebhook hook = await e.Channel.CreateWebhookAsync("Starman0620");
            DiscordWebhookBuilder builder = new DiscordWebhookBuilder();
            builder.WithAvatarUrl(e.Guild.GetMemberAsync(363850072309497876).Result.GetAvatarUrl(ImageFormat.Png));
            builder.WithUsername("Starman0620");
            builder.WithContent("Yes.");         
            await builder.SendAsync(hook);   
            await hook.DeleteAsync();
        }
    }
}