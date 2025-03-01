using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs;

internal static class TypeExtensions
{
    #region Methods

    internal static bool IsDecorator(this Type type)
    {
        return type.Implements<IDecorator>();
    }

    internal static bool Implements<TInterface>(this Type type) where TInterface : class
    {
        if (typeof(TInterface).IsInterface == false)
        {
            throw new ArgumentException("The type must be an interface.", nameof(TInterface));
        }

        return typeof(TInterface).IsAssignableFrom(type);
    }

    internal static bool IsFeature(this Type type)
    {
        return type.IsCommandFeature() || type.IsQueryFeature();
    }

    internal static bool IsCommandFeature(this Type type)
    {
        if (type.IsGenericType == false)
        {
            return false;
        }

        return type.GetGenericTypeDefinition() == typeof(ICommandFeature<>);
    }

    internal static bool IsQueryFeature(this Type type)
    {
        if (type.IsGenericType == false)
        {
            return false;
        }

        return type.GetGenericTypeDefinition() == typeof(IQueryFeature<,>);
    }

    #endregion
}