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

            string course = configuration["CourseName"];

            serviceCollection.AddTransient<IExamListTask, ExamListExecutor>();

            serviceCollection.AddTransient<IRoomReader>(
                p => new DefaultRoomReader(configuration[$"{course}:Room"],
                p.GetService<ILogger<DefaultRoomReader>>()));

            serviceCollection.AddTransient<IBonusPointReader>(
                p => new KlausurtrainerReader(configuration[$"{course}:KlausurtrainerPoints"],
                p.GetService<ILogger<KlausurtrainerReader>>()));

            serviceCollection.AddTransient<IExamListPrinter>(
                p => new LatexExamPrinter(configuration[$"{course}:Output"],
                p.GetService<ILogger<LatexExamPrinter>>()));

            serviceCollection.AddTransient<IStudentReader>(
                p => new StineStudentReader(configuration[$"{course}:ExamParticipants"],
                configuration[$"{course}:CourseParticipants"],
                p.GetService<ILogger<StineStudentReader>>()));

            serviceCollection.AddTransient<ExamListManager>();
        }

    }
}