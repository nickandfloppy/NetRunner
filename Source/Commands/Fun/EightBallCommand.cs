using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using WinBot.Util;
using WinBot.Commands.Attributes;

namespace WinBot.Commands.Fun
{
    public class EightBallCommand : BaseCommandModule
    {
        [Command("8ball")]
        [Description("Let the magic 8-ball give you advice.")]
        [Usage("[question]")]
        [Aliases("eightball")]
        [Category(Category.Fun)]
        public async Task EightBall(CommandContext Context, [RemainingText] string question)
        {
            // Select a random answer
            Random r = new Random();  
            int dex = r.Next(answers.Length);  
            // Send an embed
            DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
            eb.WithTitle($"🎱 {answers[dex]}");
            // Set the embed's color
            if (dex <= 9) 
                eb.WithColor(DiscordColor.Green);
            else if (dex <= 14) 
                eb.WithColor(DiscordColor.Yellow);
            else if (dex <= 19) 
                eb.WithColor(DiscordColor.Red);
            await Context.ReplyAsync("", eb.Build());
        }

        static string[] answers = new string[] {
            // Possible answers the magic fortune-telling eight ball can give.
            "It is certain.", "It is decidedly so.", "Without a doubt.", "Yes definitely.", "You may rely on it.", "As I see it, yes.", "Most likely.", "Outlook good.", "Yes.", "Signs point to yes.", 
            "Reply hazy, try again.", "Ask again later.", "Better not tell you now.", "Cannot predict now.", "Concentrate and ask again.",
            "Don't count on it.", "My reply is no.", "My sources say no.", "Outlook not so good.", "Very doubtful."
        };

    }
}