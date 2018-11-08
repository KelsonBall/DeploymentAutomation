using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

            PrintRequestMessages()
                .ContinueWith(t =>
                {
                    if (t.IsFaulted)
                        throw t.Exception;
                });
        }

        public class Message
        {
            public ConsoleColor[] Colors { get; set; }
            public string[] Text { get; set; }
        }

        private readonly ConcurrentQueue<Message> messages = new ConcurrentQueue<Message>();

        public async Task PrintRequestMessages()
        {
            while (true)
            {
                if (messages.TryDequeue(out Message message))
                {
                    var foreground = Console.ForegroundColor;
                    foreach (var tc in message.Text.Zip(message.Colors, (t, c) => (t, c)))
                    {
                        Console.ForegroundColor = tc.c;
                        Console.WriteLine();
                        Console.WriteLine(tc.t);
                    }
                    Console.ForegroundColor = foreground;
                }
                else
                    await Task.Delay(500);
            }
        }

        public async Task HandleTestHook(HttpContext context)
        {
            messages.Enqueue(new Message
            {
                Colors = new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White },
                Text = new string[] { "Test", DateTime.Now.ToShortTimeString() },
            });

            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Request received");
        }

        public async Task HandleBuildHook(HttpContext context)
        {
            string json = null;
            using (var reader = new StreamReader(context.Request.Body))
                json = await reader.ReadToEndAsync();

            messages.Enqueue(new Message
            {
                Colors = new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.White },
                Text = new string[] { context.Request.Path, DateTime.Now.ToShortTimeString(), json },
            });

            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Request received");
        }
    }
}
