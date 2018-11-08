using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class ProjectColumn
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("project_url")]
        public string ProjectUrl { get; set; }

        [JsonProperty("cards_url")]
        public string CardsUrl { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("created_at")]
        
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        
        public DateTimeOffset? UpdatedAt { get; set; }
    }

}
