namespace HBot.Misc
{
    public class MCServer
    {
        public ulong guildID;
        public string address;
        public string dynmap;
        public string versions;
        public string crackedInfo;
        
        public MCServer(ulong guildID, string address, string dynmap, string versions) {
            this.guildID = guildID;
            this.address = address;
            this.dynmap = dynmap;
            this.versions = versions;
            this.crackedInfo = "No. It never will, just buy the game or stop asking.";
        }

        public MCServer(ulong guildID, string address, string versions) {
            this.guildID = guildID;
            this.address = address;
            this.versions = versions;
            this.dynmap = null;
            this.crackedInfo = "No. It never will, just buy the game or stop asking.";
        }

        public MCServer() {
            this.guildID = 0;
            this.address = "address";
            this.dynmap = "dynmap";
            this.versions = "versions";
            this.crackedInfo = "No. It never will, just buy the game or stop asking.";
        }

    }
}
