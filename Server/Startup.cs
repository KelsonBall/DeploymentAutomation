using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using GithubWebhook;
using GithubWebhook.Events;
using Kelson.Common.Async;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouter(
                new RouteBuilder(app)
                    .MapRoute("/test/", HandleTestHook)
                    .MapRoute("/build/", HandleBuildHook)
                    .Build());

            Message.PrintRequestMessages()
                   .Confirm(errorCallback: e => throw e);
        }

        public class Message
        {
            public (ConsoleColor color, string text)[] Lines { get; set; }

            private static readonly ConcurrentQueue<Message> messages = new ConcurrentQueue<Message>();

            public static async Task PrintRequestMessages()
            {
                while (true)
                {
                    if (messages.TryDequeue(out Message message))
                    {
                        var foreground = Console.ForegroundColor;
                        foreach (var tc in message.Lines)
                        {
                            Console.ForegroundColor = tc.color;
                            Console.WriteLine();
                            Console.WriteLine(tc.text);
                        }
                        Console.ForegroundColor = foreground;
                    }
                    else
                        await Task.Delay(500);
                }
            }

            public static void Push(params (ConsoleColor color, string text)[] lines)
            {
                messages.Enqueue(new Message { Lines = lines });
            }
        }

        public async Task HandleTestHook(HttpContext context)
        {
            Message.Push((ConsoleColor.Yellow, "/test"), (ConsoleColor.White, DateTime.Now.ToLongTimeString()));

            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Request received");
        }

        public async Task HandleBuildHook(HttpContext context)
        {
            var hook = new GhWebhook(context.Request);

            Message.Push((ConsoleColor.Green, "/build"), (ConsoleColor.White, DateTime.Now.ToLongTimeString()), (ConsoleColor.White, hook.PayloadText));

            if (hook.PayloadObject is PushEvent push && push.Ref == "refs/heads/master")
                CloneBuildDeploy(push).Confirm(errorCallback: e => Message.Push((ConsoleColor.Red, "Deployment Error"), (ConsoleColor.White, e.Message)));

            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Request received");
        }

        public static async Task CloneBuildDeploy(PushEvent push)
        {
            string directory = push.Repository.FullName;


            if (Directory.Exists(directory))
                DeleteRepoDirectory(directory);
            Directory.CreateDirectory(directory);

            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"clone {push.Repository.Url}",
                WorkingDirectory = directory
            });

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                if (Directory.Exists(directory))
                    DeleteRepoDirectory(directory);
                Message.Push((ConsoleColor.Red, "Git Error"), (ConsoleColor.White, await process.StandardError.ReadToEndAsync()));

                throw new Exception("Git clone encountered an error");
            }

            process = Process.Start(new ProcessStartInfo
            {

            });
        }

        public static void DeleteRepoDirectory(string directory)
        {
            foreach (var subdir in Directory.EnumerateDirectories(directory))
                DeleteRepoDirectory(subdir);
            foreach (var file in Directory.EnumerateFiles(directory))
                File.Delete(file);
            Directory.Delete(directory, true);
        }
    }
}
