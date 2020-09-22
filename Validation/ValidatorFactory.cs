using System;
using FluentValidation;
using SimpleInjector;

namespace DatelAPI.Validation
{
    public sealed class ValidatorFactory : ValidatorFactoryBase
    {
        private readonly Container _container;

        public ValidatorFactory(Container container)
        {
            _container = container;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            try
            {
                return (IValidator)_container.GetInstance(validatorType);
            }
            catch (ActivationException)
            {
                return null;
            }
        }
    }
}