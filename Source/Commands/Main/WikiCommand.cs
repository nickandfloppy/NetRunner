using System.Linq;
using System.Threading.Tasks;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using HBot.Commands.Attributes;

using Genbox.Wikipedia;
using Genbox.Wikipedia.Objects;

namespace HBot.Commands.Main {
    public class WikiCommand : BaseCommandModule {
        [Command("wiki")]
        [Description("Search wikipedia")]
        [Usage("[query]")]
        [Category(Category.Main)]
        public async Task Wiki(CommandContext Context, [RemainingText] string query) {
            if(string.IsNullOrWhiteSpace(query)) {
                throw new System.Exception("You must provide a search query!");
            }

            using WikipediaClient wikiclient = new WikipediaClient();
    
			WikiSearchRequest req = new WikiSearchRequest(query);
			req.Limit = 1;
    
			WikiSearchResponse resp = await wikiclient.SearchAsync(req);
			
			foreach (SearchResult s in resp.QueryResult.SearchResults) {
					await Context.ReplyAsync($"{s.Url}".Replace(" ", "_"));
					return;
			}
			
			await Context.ReplyAsync("No results.");;
        }
    }
}