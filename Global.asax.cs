using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using FluentValidation;
using FluentValidation.WebApi;


namespace DatelAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = ConfigureContainer();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ConfigureWebApi(container);
        }


        private static Container ConfigureContainer()
        {
            var container = new Container();
            container.Options.DefaultLifestyle = Lifestyle.Scoped;
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.RegisterPackages(new[] { typeof(WebApiApplication).Assembly });
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            container.Verify();
            return container;
        }
        
        private static void ConfigureWebApi(Container container)
        {
            GlobalConfiguration.Configure(configuration =>
            {
                /*
                configuration.MapHttpAttributeRoutes();

                configuration.Formatters.Remove(configuration.Formatters.XmlFormatter);
                configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        ProcessDictionaryKeys = false,
                        OverrideSpecifiedNames = true
                    }
                };
                configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
                configuration.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                */
                configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

                FluentValidationModelValidatorProvider.Configure(configuration, provider => provider.ValidatorFactory = container.GetInstance<IValidatorFactory>());
            });
        }

    }
}
