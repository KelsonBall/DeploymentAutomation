using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class Project
    {
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("owner_url")]
        public string OwnerUrl { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("columns_url")]
        public string ColumnsUrl { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("number")]
        public long? Number { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("creator")]
        public User Creator { get; set; }

        [JsonProperty("created_at")]
        
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        
        public DateTimeOffset? UpdatedAt { get; set; }
    }

}
