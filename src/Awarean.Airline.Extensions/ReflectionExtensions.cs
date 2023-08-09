using System.Reflection;
using Awarean.Airline.Domain.ValueObjects.Attributes;

namespace Awarean.Airline.Extensions;

public static class ReflectionExtensions
{
    public static bool IsPrimitiveOrConvertible(this Type type)
    {
        // Check if the type is a primitive type or if it's a type that can be assigned from a primitive type.
        if (type.IsPrimitive || typeof(IConvertible).IsAssignableFrom(type))
            return true;

        return false;
    }

    public static bool HasConvertionToPrimitive(this Type type)
    {
        if (type.IsPrimitive)
            return true;

        // Check for a custom explicit operator to a primitive type.
        var convertionMethod = GetConventionMethodOrDefault(type);

        if (convertionMethod != null)
        {
            var conversionType = convertionMethod.ReturnType;
            return conversionType.IsPrimitive || typeof(IConvertible).IsAssignableFrom(conversionType);
        }

        return false;
    }

    /// <Summary>
    /// Searchs in the type for a method marked with the <see cref="PersistenceConvertionAttribute" /> 
    /// in the class and hierarchy and returns it. Otherwise it searchs for a static implicit or explicit operator that converts
    /// to a primitive type.
    /// <exception cref="InvalidOperationException">When a method with the attribute or an static operator is not found</exception>
    /// <exception cref="InvalidOperationException">When more than one matching static convertion method is found.</exception>
    /// </Summary>
    public static MethodInfo GetConvertionMethod(this Type type)
    {
        var method = GetConventionMethodOrDefault(type) 
            ?? throw new InvalidOperationException(
                    $"Did not found a public static method marked with {nameof(PersistenceConvertionAttribute)}" +
                    $"nor an static operator converting to a primitive type for type {type.FullName}.");
        
        return method;
    }

    private static MethodInfo? GetConventionMethodOrDefault(Type type)
    {
        MethodInfo? method = GetPersistenceConvertionMethod(type);

        if (method is not null)
            return method;

        method = GetStaticConvertionMethod(type);

        return method;
    }

    private static BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.IgnoreCase;

    private static MethodInfo? GetPersistenceConvertionMethod(Type type)
        => type.GetMethods(bindingFlags).SingleOrDefault(x => x.GetCustomAttribute<PersistenceConvertionAttribute>() is not null);

    private static MethodInfo? GetStaticConvertionMethod(Type type)
    {
        var staticBindingFlags = bindingFlags | BindingFlags.Static;
        try
        {
            return
                TryGetImplicitOperatorForType(type, staticBindingFlags)
                ?? TryGetExplicitOperatorForType(type, staticBindingFlags);
        }
        catch (AmbiguousMatchException ame)
        {
            throw new InvalidOperationException(
                "Found more than one static operator methods that converts to primitive types. "
                + $"Please mark the appropriate convertion method from type {type.FullName} with {nameof(PersistenceConvertionAttribute)}"
                + "to explicit show how the object should be converted");
        }
    }

    private static MethodInfo? TryGetExplicitOperatorForType(Type type, BindingFlags flags)
        => type.GetMethod("op_Explicit", flags, null, new[] { type }, null);

    private static MethodInfo? TryGetImplicitOperatorForType(Type type, BindingFlags flags)
        => type.GetMethod("op_Implicit", flags, null, new[] { type }, null);
}
