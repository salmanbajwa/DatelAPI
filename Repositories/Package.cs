#undef test

using SimpleInjector.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleInjector;
using System.Configuration;
using DatelAPI.Areas.Config;
using DatelAPI.Areas.Data;
using Serilog;
using DatelAPI.Areas.Logger;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace DatelAPI.Repositories
{
    public sealed class Package : IPackage
    {
        public void RegisterServices(Container container)
        {

            GetConfiguration(container);

            container.Register<IHubData, HubData>();
            container.Register<ISageData, SageData>();


            var loggerConfiguration = new LoggerConfiguration()
                        .ReadFrom.AppSettings();

            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.AppSettings()
                        .CreateLogger();
            
            Log.Information("Test");


            var telemetryConfiguration = TelemetryConfiguration
                 .CreateDefault();

#if test
            telemetryConfiguration.InstrumentationKey = ConfigurationManager.AppSettings["InstrumentationKeyTest"];
#else
            telemetryConfiguration.InstrumentationKey = ConfigurationManager.AppSettings["InstrumentationKeyLive"];
#endif

            loggerConfiguration.WriteTo
                 .ApplicationInsights(telemetryConfiguration,
                     TelemetryConverter.Events);

            container.Register<DatelAPI.Areas.Logger.ILogger, SerilogLogger>();

            container.RegisterInstance(Log.Logger);

            var telemetryClient = new TelemetryClient(telemetryConfiguration);
            container.RegisterInstance(telemetryClient);
            //services.AddSingleton(telemetryClient);


            var repositories =
                from type in typeof(Package).Assembly.ExportedTypes
                where type.IsClass && !type.IsAbstract && type.Name.EndsWith("Repository") && !type.Name.Contains("Cached")
                select new { ServiceType = type.GetInterfaces().First(), ImplementationType = type };

            foreach (var repository in repositories) container.Register(repository.ServiceType, repository.ImplementationType);
        }

        public static void GetConfiguration(Container container)
        {
#if test

            var CTSConnectionString = ConfigurationManager.ConnectionStrings["CTSConnectionString"].ConnectionString;
            var SageConnectionString = ConfigurationManager.ConnectionStrings["SageConnectionString"].ConnectionString;
#else
            var CTSConnectionString = ConfigurationManager.ConnectionStrings["CTSConnectionStringLive"].ConnectionString;
            var SageConnectionString = ConfigurationManager.ConnectionStrings["SageConnectionStringLive"].ConnectionString;
#endif

            var SageLogging = Convert.ToBoolean(ConfigurationManager.AppSettings["SageLogging"]);
            var DatelSystemID = ConfigurationManager.AppSettings["DatelSystemID"];

            var CTSConnectionStringLive = ConfigurationManager.ConnectionStrings["CTSConnectionStringLive"].ConnectionString;
            var SageConnectionStringLive = ConfigurationManager.ConnectionStrings["SageConnectionStringLive"].ConnectionString;

            container.Register<IConfigProvider> (() => new ConfigProvider(CTSConnectionString, SageConnectionString, SageLogging, DatelSystemID, CTSConnectionStringLive, SageConnectionStringLive), Lifestyle.Singleton);
        }

    }


}