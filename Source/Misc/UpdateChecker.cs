using System;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Serilog;

namespace HBot.Misc {
    public static class UpdateChecker {
        private static readonly HttpClient httpClient = new HttpClient();
        private const string API_URL = "http://localhost/api/hbot/check-update"; //Replace this with the live version once the updates are out

        public static async Task<(bool updateAvailable, string latestVersion, string releaseDate)> CheckForUpdate(string currentVersion) {
            try {
                var url = $"{API_URL}?version={currentVersion}";
                var response = await httpClient.GetAsync(url);

                if(!response.IsSuccessStatusCode) {
                    Log.Error($"Failed to check for update. Response code: {(int)response.StatusCode} ({response.StatusCode})");
                    return (false, "", "");
                }

                var content = await response.Content.ReadAsStringAsync();
                var updateInfo = JsonConvert.DeserializeObject<UpdateInfo>(content);

                if(updateInfo == null) {
                    Log.Error("Failed to deserialize update info");
                    return (false, "", "");
                }

                return (updateInfo.updateAvailable, updateInfo.latestVersion, updateInfo.releaseDate);
            }
            catch(Exception ex) {
                Log.Error(ex, "An error occurred while checking for update");
                return (false, "", "");
            }
        }

        private class UpdateInfo {
            [JsonProperty("update_available")]
            public bool updateAvailable { get; set; }

            [JsonProperty("latest_version")]
            public string latestVersion { get; set; }

            [JsonProperty("release_date")]
            public string releaseDate { get; set; }
        }
    }
}
