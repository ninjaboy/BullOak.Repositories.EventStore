using System;
using Microsoft.Extensions.Configuration;

namespace BullOak.Repositories.EventStore.Test.Integration
{
    public static class SettingsResolver
    {
        private const string DefaultAppsettingsJsonFile = "appsettings";
        private const string AppEnviromentNameVariable = "BO_ENVIRONMENT";

        // ReSharper disable once UnusedMember.Global
        public static Func<string, bool, string> GetSettingsResolver(
            string appsettingsBasePath = null,
            string appsettingsFile = DefaultAppsettingsJsonFile,
            string environmentVariablesPrefix = "BO_")
        {
            var configurationBuilder = new ConfigurationBuilder();

            if (!string.IsNullOrWhiteSpace(appsettingsBasePath))
            {
                configurationBuilder.SetBasePath(appsettingsBasePath);
            }

            if (!string.IsNullOrWhiteSpace(appsettingsFile))
            {
                configurationBuilder.AddJsonFile($"{appsettingsFile}.json");

                var appEnvironmentName = Environment.GetEnvironmentVariable(AppEnviromentNameVariable);
                if (!string.IsNullOrWhiteSpace(appEnvironmentName))
                {
                    configurationBuilder.AddJsonFile($"{appsettingsFile}.{appEnvironmentName}.json", optional: true);
                }
            }

            if (!string.IsNullOrEmpty(environmentVariablesPrefix))
            {
                configurationBuilder.AddEnvironmentVariables(environmentVariablesPrefix);
            }

            var configuration = configurationBuilder.Build();

            string settingsResolver(string name, bool isSecret)
            {
                var value = configuration.GetSection(name).Value;
                return value;
            }

            return settingsResolver;
        }
    }
}