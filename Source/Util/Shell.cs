using System.Diagnostics;

namespace HBot.Util
{
    public static class Shell
    {
        public static string Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            if (result.Length < 1024)
				return result;
			else{
				string baseUrl = "http://paste.nick99nack.com/";
				var hasteBinClient = new HasteBinClient(baseUrl);
				HasteBinResult HBresult = hasteBinClient.Post(result).Result;
				return $"{baseUrl}{HBresult.Key}";
			}
        }
		
    }
}