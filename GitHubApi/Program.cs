using System.IO;
using GitHubModel;
using Newtonsoft.Json;


namespace GitHubApi
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = File.ReadAllText("example.json");
            var model = JsonConvert.DeserializeObject<EventModel>(json);

        }
    }
}
