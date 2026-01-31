using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TaskForceAdmiralLiveRPC
{
    public class GitHubUpdateChecker
    {
        private const string GITHUB_REPO = "Smiley-Devv/TaskForceAdmiralRPC"; // TODO: Update this
        private const string CURRENT_VERSION = "1.0.0";

        public event Action<UpdateInfo> UpdateAvailable;
        public event Action<string> StatusChanged;

        public async Task CheckForUpdates()
        {
            try
            {
                StatusChanged?.Invoke("Checking for updates...");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("TaskForceAdmiralRPC");

                    string apiUrl = $"https://api.github.com/repos/{GITHUB_REPO}/releases/latest";
                    var response = await client.GetStringAsync(apiUrl);

                    var release = JsonSerializer.Deserialize<GitHubRelease>(response);

                    if (release == null)
                    {
                        StatusChanged?.Invoke("No releases found");
                        return;
                    }

                    string latestVersion = release.tag_name.Replace("v", "");

                    if (CompareVersions(latestVersion, CURRENT_VERSION) > 0)
                    {
                        var updateInfo = new UpdateInfo
                        {
                            LatestVersion = latestVersion,
                            CurrentVersion = CURRENT_VERSION,
                            ReleaseNotes = release.body,
                            DownloadUrl = release.html_url,
                            PublishedDate = release.published_at
                        };

                        UpdateAvailable?.Invoke(updateInfo);
                        StatusChanged?.Invoke($"Update available: v{latestVersion}");
                    }
                    else
                    {
                        StatusChanged?.Invoke("You're up to date!");
                    }
                }
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke($"Update check failed: {ex.Message}");
            }
        }

        private int CompareVersions(string v1, string v2)
        {
            try
            {
                var version1 = new Version(v1);
                var version2 = new Version(v2);
                return version1.CompareTo(version2);
            }
            catch
            {
                return 0;
            }
        }

        private class GitHubRelease
        {
            public string tag_name { get; set; }
            public string name { get; set; }
            public string body { get; set; }
            public string html_url { get; set; }
            public DateTime published_at { get; set; }
        }
    }

    public class UpdateInfo
    {
        public string LatestVersion { get; set; }
        public string CurrentVersion { get; set; }
        public string ReleaseNotes { get; set; }
        public string DownloadUrl { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}