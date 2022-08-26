using System.Diagnostics;

namespace WinBot.Util
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
		
		public static string CmdPrmpt(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "C:\\Windows\\System32\\cmd.exe",
                    Arguments = $"/c \"{escapedArgs}\"",
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
		
		public static string DOSPrmpt(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "C:\\Windows\\System32\\cmd.exe",
                    Arguments = $"/c \"command.com /C {escapedArgs}\"",
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
		
		public static string PSPrmpt(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "C:\\Windows\\system32\\WindowsPowerShell\\v1.0\\powershell.exe",
                    Arguments = $"/c \"{escapedArgs}\"",
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