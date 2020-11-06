using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JobParser.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JobParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>()
                        .AddAutoMapper(typeof(Worker))
                        .AddSingleton(new ParserService(
                            hostContext.Configuration.GetSection("LoginUserName").Value,
                            hostContext.Configuration.GetSection("LoginPassword").Value))
                        .AddSingleton(new RemoteRepository(
                            hostContext.Configuration.GetSection("Url").Value));
                });
    }
}
