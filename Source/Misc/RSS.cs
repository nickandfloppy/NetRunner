using System.IO;
using System.Timers;
using System.Threading.Tasks;
using System.Collections.Generic;

using DSharpPlus.Entities;

using CodeHollow.FeedReader;

using Newtonsoft.Json;

using Serilog;

using HBot.Util;

namespace HBot.Misc {
    public class RSS {
        public static List<string> sentItems = new List<string>();

        public static async Task Init() {
            try {
                // Load cached items
                if (ResourceManager.ResourceExists("rss.cache"))
                    sentItems = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(ResourceManager.GetResourcePath("rss.cache")));

                // Set up the timer
                Timer t = new Timer(43200000);
                t.AutoReset = true;
                t.Elapsed += async (object sender, ElapsedEventArgs e) => {
                    await FetchItems();
                };
                t.Start();

                // Do an initial fetch of items
                await FetchItems();

                Log.Write(Serilog.Events.LogEventLevel.Information, "RSS service started");
            }
            catch (System.Exception ex) {
                Log.Write(Serilog.Events.LogEventLevel.Information, ex.Message);
            }
        }

        public static async Task FetchItems() {
            try {
                // Setup
                var feed = await FeedReader.ReadAsync("http://localhost/blog/rss"); //Replace with the live site when the updates are out

                DiscordChannel additions = await Bot.client.GetChannelAsync(Bot.config.ids.rssChannel);

                foreach (FeedItem item in feed.Items) {
                    // Don't send an item twice
                    if (sentItems.Contains(item.Id))
                        continue;

                    // Create and send the embed
                    DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
                    eb.WithTitle(item.Title);
                    eb.WithUrl(item.Link);
                    eb.WithColor(DiscordColor.Gold);
                    eb.WithDescription(item.Description);
                    eb.WithFooter($"Uploaded: {item.PublishingDate}");
                    await additions.SendMessageAsync("", eb.Build());

                    // Cache the item so it isn't sent in the next fetch
                    sentItems.Add(item.Id);

                    await Task.Delay(1024);
                }
                File.WriteAllText(ResourceManager.GetResourcePath("rss.cache"), JsonConvert.SerializeObject(sentItems, Formatting.Indented));
            }
            catch (System.Exception ex) {
                Log.Write(Serilog.Events.LogEventLevel.Information, ex.Message);
            }
        }
    }
}
