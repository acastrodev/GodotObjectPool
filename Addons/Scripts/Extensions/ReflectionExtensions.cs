using System;

public static class ReflectionExtensions
{
    public static Type GetPropertyValue<Type>(this object @object, string propertyName) 
    {
        object value = @object.GetType().GetProperty(propertyName)?.GetValue(@object, null);

        if (value == null)
        {
            throw new ArgumentException("Cannot find property name.", propertyName);
        }

        Type result;

        if (value is Type)
        {
            result = (Type)Convert.ChangeType(value, value.GetType());
        }
        else
        {
            throw new InvalidCastException($"Cannot cast {value} into type {typeof(Type)}");
        }

        return result;
    }
}