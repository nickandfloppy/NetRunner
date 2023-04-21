using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using MarkovSharp.TokenisationStrategies;

using HBot.Util;
using HBot.Commands.Attributes;

namespace HBot.Commands.Fun {
    public class MarkovLyricsCommand : BaseCommandModule {
        [Command("lyrics")]
        [Description("Get markov chains lyrics")]
        [Usage("[artist] [lines]")]
        [Category(Category.Fun)]
        public async Task MarkovLyrics(CommandContext Context, string input = "all", int lines = 5) {
            DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
            string[] artists = null;

            if (input.ToLower() == "list") {
                artists = Directory.GetFiles("Resources/Lyrics")
                    .Select(f => Path.GetFileNameWithoutExtension(f))
                    .ToArray();
                eb.WithColor(DiscordColor.Gold)
                    .WithDescription($"```\n{string.Join(' ', artists)} all```");
            } else {
                string filePath = $"Resources/Lyrics/{input.ToLower()}.txt";
                if (!File.Exists(filePath) && input.ToLower() != "all")
                    throw new Exception("Invalid artist! Run 'lyrics list' to get a list of available artists");

                // Get input text
                List<string> txt = new List<string>();
                if (input.ToLower() == "all") {
                    foreach (string file in Directory.GetFiles("Resources/Lyrics"))
                        txt.AddRange(File.ReadAllLines(file));
                    eb.WithDescription("Training...\nThis will take a little while.");
                    await Context.Channel.TriggerTypingAsync();
                } else {
                    txt = File.ReadAllLines(filePath).ToList();
                }

                if (lines > 25)
                    throw new Exception("You cannot have more than 25 lines!");

                // Generate the markov text
                StringMarkov model = new StringMarkov(1);
                model.Learn(txt);
                model.EnsureUniqueWalk = true;

                // Create and send lyric embed
                eb.WithTitle($"Generated `{input}` Lyrics")
                    .WithColor(DiscordColor.Gold)
                    .WithDescription($"```\n{string.Join('\n', model.Walk(lines)).Truncate(4096)}```")
                    .WithFooter($"Trained on {txt.Count} lines of lyrical hell");
            }

            await Context.ReplyAsync(eb);
        }
    }
}