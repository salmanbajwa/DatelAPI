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

namespace DatelAPI.Repositories
{
    public sealed class Package : IPackage
    {
        public void RegisterServices(Container container)
        {

            GetConfiguration(container);

            container.Register<IHubData, HubData>();
            container.Register<ISageData, SageData>();


            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.AppSettings()
                        .CreateLogger();
            
            Log.Information("Test");
            

            container.Register<DatelAPI.Areas.Logger.ILogger, SerilogLogger>();

            container.RegisterInstance(Log.Logger);


            var repositories =
                from type in typeof(Package).Assembly.ExportedTypes
                where type.IsClass && !type.IsAbstract && type.Name.EndsWith("Repository") && !type.Name.Contains("Cached")
                select new { ServiceType = type.GetInterfaces().First(), ImplementationType = type };

            foreach (var repository in repositories) container.Register(repository.ServiceType, repository.ImplementationType);
        }

        public static void GetConfiguration(Container container)
        {
            var CTSConnectionString = ConfigurationManager.ConnectionStrings["CTSConnectionString"].ConnectionString;
            var SageConnectionString = ConfigurationManager.ConnectionStrings["SageConnectionString"].ConnectionString;
            var SageLogging = Convert.ToBoolean(ConfigurationManager.AppSettings["SageLogging"]);
            var DatelSystemID = ConfigurationManager.AppSettings["DatelSystemID"];

            container.Register<IConfigProvider> (() => new ConfigProvider(CTSConnectionString, SageConnectionString, SageLogging, DatelSystemID), Lifestyle.Singleton);
        }

    }


}