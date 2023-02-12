using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using Newtonsoft.Json;

using Serilog;

using static HBot.Util.ResourceManager;

namespace HBot.Misc
{
    public class MCServer
    {
        public ulong guildID;
        public string address;
        public string dynmap;
        public string versions;
        
        public MCServer(ulong guildID, string address, string dynmap, string versions) {
            this.guildID = guildID;
            this.address = address;
            this.dynmap = dynmap;
            this.versions = versions;
        }

        public MCServer(ulong guildID, string address, string versions) {
            this.guildID = guildID;
            this.address = address;
            this.versions = versions;
            this.dynmap = null;
        }

        public MCServer() {
            this.guildID = 0;
            this.address = "address";
            this.dynmap = "dynmap";
            this.versions = "versions";
        }

    }
}
