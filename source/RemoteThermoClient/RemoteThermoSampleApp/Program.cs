using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace RemoteThermoSampleApp
{
    class Program
    {
        private static IConfiguration configuration;
        private static ILogger logger;

        private static RemoteThermoClient.RemoteThermoClient remoteThermoClient;

        static async Task Main(string[] args)
        {
            configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables()
              .AddCommandLine(args)
              .Build();

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("NonHostConsoleApp.Program", LogLevel.Debug)
                    .AddConsole();
            });

            logger = loggerFactory.CreateLogger<Program>();

            logger.LogInformation("Hello Thermo!");

            //checking configuration requirements
            string userName = configuration.GetValue<string>("remoteThermoUserName");

            if (string.IsNullOrEmpty(userName))
            {
                logger.LogError("This tool requires a <userName> parameter to authenticate to the RemoteThermo services.");
                return;
            }

            string password = configuration.GetValue<string>("remoteThermoPassword");

            if (string.IsNullOrEmpty(password))
            {
                logger.LogError("This tool requires a <password> parameter to authenticate to the RemoteThermo services.");
                return;
            }

            string plantName = configuration.GetValue<string>("remoteThermoPlantName");

            if (string.IsNullOrEmpty(plantName))
            {
                logger.LogError("This tool requires a <plantName> parameter to connect to the RemoteThermo services.");
                return;
            }

            string baseUrlComponent = configuration.GetValue<string>("remoteThermoBaseUrlComponent");

            if (string.IsNullOrEmpty(baseUrlComponent))
            {
                logger.LogError("This tool requires a <baseUrlComponent> parameter to connect to the RemoteThermo services.");
                return;
            }


            var remoteThermoClientLogger = loggerFactory.CreateLogger<RemoteThermoClient.RemoteThermoClient>();

            remoteThermoClient = new RemoteThermoClient.RemoteThermoClient(userName, password, plantName, baseUrlComponent, remoteThermoClientLogger);


            var plants = await remoteThermoClient.GetPlants();

            logger.LogInformation(plants.ToJson());



            var plantData = await remoteThermoClient.GetPlantData();

            logger.LogInformation(plantData.ToJson());



            var plantSettings = await remoteThermoClient.GetPlantSettrings();

            logger.LogInformation(plantSettings.ToJson());

            var schedule = await remoteThermoClient.GetSchedule();

            logger.LogInformation(schedule.ToJson());

        }
    }
}
