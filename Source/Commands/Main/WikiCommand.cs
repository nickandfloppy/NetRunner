using System.Linq;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using HBot.Commands.Attributes;

using WikipediaNet;
using WikipediaNet.Objects;

namespace HBot.Commands.Main
{
    public class WikiCommand : BaseCommandModule
    {
        [Command("wiki")]
        [Description("Search wikipedia")]
        [Usage("[query]")]
        [Category(Category.Main)]
        public async Task Wiki(CommandContext Context, [RemainingText] string query)
        {
            if(string.IsNullOrWhiteSpace(query))
                throw new System.Exception("You must provide a search query!");

            Wikipedia wiki = new Wikipedia();
            wiki.Limit = 1;
            QueryResult results = wiki.Search(query);

            if (results.SearchInfo.TotalHits < 1) {
                await Context.ReplyAsync($"Error: There are no results for that query.");
                return;
            }

            await Context.ReplyAsync(results.Search.First().Url.ToString().Replace(" ", "_"));
        }
    }
}