using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Build
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("error")]
        public Error Error { get; set; }

        [JsonProperty("pusher")]
        public User Pusher { get; set; }

        [JsonProperty("commit")]
        public string Commit { get; set; }

        [JsonProperty("duration")]
        public long? Duration { get; set; }

        [JsonProperty("created_at")]
         public DateTimeOffset?  CreatedAt { get; set; }

        [JsonProperty("updated_at")]
         public DateTimeOffset?  UpdatedAt { get; set; }
    }


}
