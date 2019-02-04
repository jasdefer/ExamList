using ExamListCore;
using ExamListCore.Implementations;
using ExamListCore.Interfaces;
using ExamListCore.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace ExamListConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Exam List Manager";

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // run app
            serviceProvider.GetService<App>().Run();
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            // add logging
            serviceCollection.AddLogging(configer => configer.AddConsole());

            // build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json", false)
                .Build();

            serviceCollection.AddOptions();
            serviceCollection.Configure<Settings>(configuration);

            serviceCollection.AddTransient<IBonusPointReader, KlausurtrainerPointReader>();
            serviceCollection.AddTransient<IExamListPrinter, ExamListLatexPrinter>();
            serviceCollection.AddTransient<IRoomReader, RoomReader>();
            serviceCollection.AddTransient<IStudentDistribution, RandomStudentDistribution>();
            serviceCollection.AddTransient<IStudentReader, StineStudentReader>();
            serviceCollection.AddTransient<IRoomListPrinter, DefaultRoomListPrinter>();
            serviceCollection.AddTransient<ExamListManager>();

            // add app
            serviceCollection.AddTransient<App>();
        }
    }
}
