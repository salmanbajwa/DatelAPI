using FluentValidation;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace DatelAPI.Validation
{
    public sealed class Package : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.RegisterSingleton<IValidatorFactory, ValidatorFactory>();
            container.Register(typeof(IValidator<>), new[] { typeof(Package).Assembly }, Lifestyle.Singleton);
        }
    }
}