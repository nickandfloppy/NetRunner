using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using HBot.Commands.Attributes;

namespace HBot.Commands.Owner {
    public class EvalCommand: BaseCommandModule {
        [Command("eval")]
        [Aliases("ev")]
        [Description("It's an eval command.")]
        [Usage("[C# Code]")]
        [Category(Category.Owner)]
        [RequireOwner]
        public async Task Eval(CommandContext ctx, [RemainingText] string code) {
            // Set up the script options
            var options = ScriptOptions.Default
                .WithImports("System", "System.Linq", "System.Collections.Generic", "DSharpPlus", "DSharpPlus.CommandsNext", "DSharpPlus.Entities", "DSharpPlus.EventArgs")
                .WithReferences(GetReferencedAssemblies());

            // Evaluate the code
            try {
                var result = await CSharpScript.EvaluateAsync(code, options, globals: new EvalGlobals(ctx));
                if (result != null) {
                    // Send the result as a message
                    var embed = new DiscordEmbedBuilder()
                        .WithTitle("Eval Result")
                        .WithColor(DiscordColor.Gold)
                        .WithDescription($"```\n{result}\n```")
                        .Build();
                    await ctx.RespondAsync(embed: embed);
                }
            } catch (Exception ex) {
                // Send the error message as a message
                var embed = new DiscordEmbedBuilder()
                    .WithTitle("Eval Error")
                    .WithColor(DiscordColor.Red)
                    .WithDescription($"```\n{ex.Message}\n```")
                    .Build();
                await ctx.RespondAsync(embed: embed);
            }
        }

        private static IEnumerable < MetadataReference > GetReferencedAssemblies() {
            var assemblies = new List < Assembly > {
                Assembly.GetEntryAssembly(),
                typeof (object).Assembly,
                typeof (Enumerable).Assembly,
                typeof (DiscordClient).Assembly,
                typeof (CommandContext).Assembly,
                typeof (CommandEventArgs).Assembly
            };
            return assemblies.Select(x => MetadataReference.CreateFromFile(x.Location));
        }
    }

    public class EvalGlobals {
        public CommandContext Context {
            get;
        }
        public DiscordGuild Guild => Context.Guild;
        public DiscordChannel Channel => Context.Channel;
        public DiscordUser User => Context.User;
        public DiscordMessage Message => Context.Message;

        public EvalGlobals(CommandContext ctx) {
            Context = ctx;
        }
    }
}