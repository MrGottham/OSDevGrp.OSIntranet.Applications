using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;

namespace OSDevGrp.OSIntranet.Core.TestHelpers
{
    public class ConverterBaseTestHelper
    {
        public IEnumerable<ConverterBase> GetConverters(Assembly assembly)
        {
            NullGuard.NotNull(assembly, nameof(assembly));

            return assembly.GetTypes()
                .Where(type => type.BaseType != null && type.BaseType == typeof(ConverterBase) && type.GetConstructor(new Type[0]) != null)
                .Select(converterBaseType =>
                {
                    ConstructorInfo constructorInfo = converterBaseType.GetConstructor(new Type[0]);
                    try
                    {
                        // ReSharper disable PossibleNullReferenceException
                        return (ConverterBase) constructorInfo.Invoke(new object[0]);
                        // ReSharper restore PossibleNullReferenceException
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException ?? ex;
                    }
                })
                .ToList();
        }

        public string IsConfigurationValid(IEnumerable<ConverterBase> converterBaseCollection)
        {
            NullGuard.NotNull(converterBaseCollection, nameof(converterBaseCollection));

            IList<string> configurationErrorCollection = converterBaseCollection.AsParallel()
                .Select(IsConfigurationValid)
                .Where(configurationError => string.IsNullOrWhiteSpace(configurationError) == false)
                .ToList();

            if (configurationErrorCollection.Count == 0)
            {
                return null;
            }

            StringBuilder resultBuilder = new StringBuilder();
            foreach (string configurationError in configurationErrorCollection)
            {
                if (resultBuilder.Length > 0)
                {
                    resultBuilder.AppendLine();
                }

                resultBuilder.AppendLine(configurationError);
            }

            return resultBuilder.ToString();
        }

        private string IsConfigurationValid(ConverterBase converterBase)
        {
            NullGuard.NotNull(converterBase, nameof(converterBase));

            FieldInfo mapperFieldInfo = converterBase.GetType().GetField("Mapper", BindingFlags.Instance | BindingFlags.NonPublic);
            if (mapperFieldInfo == null)
            {
                throw new MissingFieldException($"Unable to find the field named 'Mapper' on '{converterBase.GetType().Name}'.", "Mapper");
            }

            Mapper mapper = (Mapper) mapperFieldInfo.GetValue(converterBase);
            try
            {
                mapper.ConfigurationProvider.AssertConfigurationIsValid();

                return null;
            }
            catch (AutoMapperConfigurationException ex)
            {
                return ex.Message;
            }
        }
    }
}