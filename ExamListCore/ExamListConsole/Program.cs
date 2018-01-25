using ExamList;
using ExamList.Implementations;
using ExamList.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ExamListConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Run app
            (serviceProvider.GetService<IExamListTask>()).Run();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Add logging
            serviceCollection.AddLogging((config) => { config.AddConsole(); });

            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton(configuration);

            serviceCollection.AddTransient<IExamListTask, ExamListExecutor>();

            serviceCollection.AddTransient<IRoomReader>(
                p => new DefaultRoomReader(configuration["Paths:Room"],
                p.GetService<ILogger<DefaultRoomReader>>()));

            serviceCollection.AddTransient<IBonusPointReader>(
                p => new KlausurtrainerReader(configuration["Paths:KlausurtrainerPoints"],
                p.GetService<ILogger<KlausurtrainerReader>>()));

            serviceCollection.AddTransient<IExamListPrinter>(
                p => new LatexExamPrinter(configuration["Paths:Output"]));

            serviceCollection.AddTransient<IStudentReader>(
                p => new StineStudentReader(configuration["Paths:ExamParticipants"],
                configuration["Paths:CourseParticipants"],
                p.GetService<ILogger<StineStudentReader>>()));

            serviceCollection.AddTransient<ExamListManager>();
        }

    }
}