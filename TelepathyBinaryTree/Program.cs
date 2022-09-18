using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TelepathyBinaryTree.Services.BinaryTreeService;

class Program
{
    public static void Main(String[] args)
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddScoped<IBinaryTreeService, BinaryTreeService>();
                services.AddScoped<IRunnnerAppService, RunnnerAppService>();
            })
            .Build();

        var svc = ActivatorUtilities.CreateInstance<RunnnerAppService>(host.Services);
        svc.Run();

        Console.ReadLine();
    }
}