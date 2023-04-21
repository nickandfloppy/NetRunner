using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using static HBot.Util.ResourceManager;
using HBot.Misc;
using HBot.Util;
using HBot.Commands.Attributes;

using MarkovSharp.TokenisationStrategies;

using Newtonsoft.Json;

namespace HBot.Commands.Fun {
    public class MarkovCommand : BaseCommandModule {
        [Command("markov")]
        [Aliases("mk")]
        [Description("Markov chains and things")]
        [Usage("[user] [length]")]
        [Category(Category.Fun)]
        public async Task Markov(CommandContext context, DiscordMember user = null, [RemainingText]int length = 5) {
            try {
                List<string> data;
                string sourceName;

                if (user == null) {
                    using (StreamReader sr = new StreamReader(GetResourcePath("randomMessages", Util.ResourceType.JsonData))) {
                        string json = await sr.ReadToEndAsync();
                        List<UserMessage> msgs = JsonConvert.DeserializeObject<List<UserMessage>>(json);
                        data = msgs.Select(m => m.content).ToList();
                    }
                    sourceName = "Source: User-Submitted Messages";
                }
                else {
                    User userData = UserData.GetOrCreateUser(user);
                    data = userData.messages;
                    sourceName = $"Source: {userData.username}";
                }

                if (length > 25) length = 25;
                else if (length < 0) length = 1;

                // Generate the markov text
                StringMarkov model = new StringMarkov(1);
                model.Learn(data);
                model.EnsureUniqueWalk = true;

                DiscordEmbedBuilder embed = new DiscordEmbedBuilder {
                    Author = new DiscordEmbedBuilder.EmbedAuthor { Name = sourceName },
                    Color = DiscordColor.Gold,
                    Footer = new DiscordEmbedBuilder.EmbedFooter { Text = $"{data.Count} messages are in data. Better results will be achieved with more messages." },
                    Description = string.Join(' ', model.Walk(length)).Replace("@", "").Truncate(4096)
                };

                await context.ReplyAsync(embed);
            }
            catch (Exception ex) {
                await context.ReplyAsync("Error: " + ex.Message + "\nStack Trace:" + ex.StackTrace);
            }
        }
    }
}