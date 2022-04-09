using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;

using WinBot.Commands.Attributes;

namespace WinBot.Commands.Owner
{
    public class VoiceCommands : BaseCommandModule
    {
        [Command("join"), Description("Joins a voice channel.")]
        public async Task Join(CommandContext Context, DiscordChannel chn = null)
        {
            // check whether VNext is enabled
            var vnext = Context.Client.GetVoiceNext();
            if (vnext == null)
            {
                // not enabled
                await Context.RespondAsync("VNext is not enabled or configured.");
                return;
            }

            // check whether we aren't already connected
            var vnc = vnext.GetConnection(Context.Guild);
            if (vnc != null)
            {
                // already connected
                await Context.RespondAsync("Already connected in this guild.");
                return;
            }

            // get member's voice state
            var vstat = Context.Member?.VoiceState;
            if (vstat?.Channel == null && chn == null)
            {
                // they did not specify a channel and are not in one
                await Context.RespondAsync("You are not in a voice channel.");
                return;
            }

            // channel not specified, use user's
            if (chn == null)
                chn = vstat.Channel;

            // connect
            vnc = await vnext.ConnectAsync(chn);
            await Context.RespondAsync($"Connected to `{chn.Name}`");
        }

        [Command("leave"), Description("Leaves a voice channel.")]
        public async Task Leave(CommandContext Context)
        {
            // check whether VNext is enabled
            var vnext = Context.Client.GetVoiceNext();
            if (vnext == null)
            {
                // not enabled
                await Context.RespondAsync("VNext is not enabled or configured.");
                return;
            }

            // check whether we are connected
            var vnc = vnext.GetConnection(Context.Guild);
            if (vnc == null)
            {
                // not connected
                await Context.RespondAsync("Not connected in this guild.");
                return;
            }

            // disconnect
            vnc.Disconnect();
            await Context.RespondAsync("Disconnected");
        }
	}
}