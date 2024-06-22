using System;
using System.Linq.Expressions;

namespace DotNetCraft.TipsAndTricks.Reflections.Example01
{
    public static class PropertyAccessor<T>
    {
        public static Func<T, TResult> CreateGetter<TResult>(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(propertyName));

            var parameter = Expression.Parameter(typeof(T), "instance");
            var property = Expression.Property(parameter, propertyName);
            var convert = Expression.Convert(property, typeof(TResult));
            var lambda = Expression.Lambda<Func<T, TResult>>(convert, parameter);

            return lambda.Compile();
        }

        public static Action<T, TResult> CreateSetter<TResult>(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(propertyName));

            var parameterExpression = Expression.Parameter(typeof(T), "instance");
            var parameterValue = Expression.Parameter(typeof(TResult), "value");

            var property = Expression.Property(parameterExpression, propertyName);
            var convert = Expression.Convert(parameterValue, property.Type);
            var assign = Expression.Assign(property, convert);

            var lambda = Expression.Lambda<Action<T, TResult>>(assign, parameterExpression, parameterValue);

            return lambda.Compile();
        }
    }
}
