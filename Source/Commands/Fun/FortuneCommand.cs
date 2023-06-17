using System;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using WinBot.Commands.Attributes;

using WinBot.Util;

namespace WinBot.Commands.Fun
{
	public class FortuneCommand : BaseCommandModule
	{
		[Command("fortune")]
		[Description("It's the BSD fortune program")]
		[Usage("fortune")]
		[Category(Category.Fun)]
		public async Task Exec(CommandContext Context)
		{
			string command = "C:\\cygwin\\bin\\fortune.exe -a";
			DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
			eb.WithTitle("Fortune");
			eb.WithColor(DiscordColor.Gold);
			eb.WithDescription($"```{command.CmdPrmpt()}```");
			await Context.ReplyAsync("", eb.Build());
		}
	}
}