using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FluentValidation
{
    public class ValidatorLocator
    {
        private static readonly ConcurrentDictionary<Type, IEnumerable<IValidator>> _validatorLocatorCache =
            new ConcurrentDictionary<Type, IEnumerable<IValidator>>();

        private readonly IServiceProvider _serviceProvider;

        public ValidatorLocator(IServiceProvider serviceProvider) =>
            _serviceProvider = serviceProvider;

        public IEnumerable<IValidator> GetValidators(Type modelType) =>
            _validatorLocatorCache.GetOrAdd(
                modelType, 
                x => 
                {
                    var validatorType = typeof(IValidator<>).MakeGenericType(modelType);
                    var enumerableType = typeof(IEnumerable<>).MakeGenericType(validatorType);
                    var validators = new List<IValidator>();

                    foreach(var validator in (IEnumerable)_serviceProvider.GetService(enumerableType))
                        validators.Add((IValidator)validator);

                    return validators.ToArray();
                }
            );
    }
}
