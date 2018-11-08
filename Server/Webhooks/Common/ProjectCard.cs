using System;
using Newtonsoft.Json;

namespace GithubWebhook.Common
{
    public partial class ProjectCard
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("column_url")]
        public string ColumnUrl { get; set; }

        [JsonProperty("content_url")]
        public string ContentUrl { get; set; }

        [JsonProperty("column_id")]
        public long? ColumnId { get; set; }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("user")]
        public User Creator { get; set; }

        [JsonProperty("created_at")]
        
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        
        public DateTimeOffset? UpdatedAt { get; set; }
    }

}
