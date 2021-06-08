using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SimpleRemoteThermo2MQTTApp
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

            string plantId = configuration.GetValue<string>("remoteThermoPlantId");

            if (string.IsNullOrEmpty(plantId))
            {
                logger.LogError("This tool requires a <plantId> parameter to connect to the RemoteThermo services.");
                return;
            }

            string baseUrlComponent = configuration.GetValue<string>("remoteThermoBaseUrlComponent");

            if (string.IsNullOrEmpty(baseUrlComponent))
            {
                logger.LogError("This tool requires a <baseUrlComponent> parameter to connect to the RemoteThermo services.");
                return;
            }

            string mqttBrokerHost = configuration.GetValue<string>("mqttBrokerHost");

            if (string.IsNullOrEmpty(mqttBrokerHost))
            {
                logger.LogError("This tool requires a <mqttBrokerHost> parameter to connect to the MQTT Broker.");
                return;
            }

            string mqttBrokerPortStr = configuration.GetValue<string>("mqttBrokerPort");
            int mqttBrokerPort = 1883; //default value

            if (string.IsNullOrEmpty(mqttBrokerPortStr))
            {
                logger.LogError("This tool requires a <mqttBrokerPort> parameter to connect to the MQTT Broker.");
                return;
            }
            else
            {
                var parsed = int.TryParse(mqttBrokerPortStr, out mqttBrokerPort);

                if (!parsed)
                {
                    logger.LogError("This tool requires a valid integer as <mqttBrokerPort> parameter to connect to the MQTT Broker.");
                    return;
                }
            }

            string mqttBrokerBaseTopic = configuration.GetValue<string>("mqttBrokerBaseTopic");

            if (string.IsNullOrEmpty(mqttBrokerBaseTopic))
            {
                logger.LogError("This tool requires a <mqttBrokerBaseTopic> parameter to connect to the MQTT Broker and listen + publish to the right topic.");
                return;
            }




            var remoteThermoClientLogger = loggerFactory.CreateLogger<RemoteThermoClient.RemoteThermoClient>();


            remoteThermoClient = new RemoteThermoClient.RemoteThermoClient(userName, password, plantId, baseUrlComponent, remoteThermoClientLogger);


            //now just start an infinite loop 



        }
    }
}
