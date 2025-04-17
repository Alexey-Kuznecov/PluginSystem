
using PluginSystem.Abstractions.Commands;

namespace PluginSystem.Core
{
    public class CommandParameters : ICommandParameters
    {
        private readonly Dictionary<string, object?> _parameters = new();

        public void Set(string key, object? value)
        {
            _parameters[key] = value;
        }

        //public T? Get<T>(string key)
        //{
        //    return _parameters.TryGetValue(key, out var value)
        //        ? (T?)Convert.ChangeType(value, typeof(T))
        //        : default;
        //}

        public bool Contains(string key) => _parameters.ContainsKey(key);

        public T Get<T>(string key)
        {
            if (_parameters.TryGetValue(key, out var value))
            {
                if (value is T typedValue)
                {
                    return typedValue;
                }

                try
                {
                    if (typeof(T).IsNullableType())
                    {
                        if (value == null)
                        {
                            return default(T);
                        }
                        var underlyingType = Nullable.GetUnderlyingType(typeof(T));
                        return (T)Convert.ChangeType(value, underlyingType);
                    }

                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch (InvalidCastException)
                {
                    // Возвращаем значение по умолчанию, если преобразование не удалось
                    return default;
                }
            }

            return default;
        }
    }

    // Метод расширения для проверки на Nullable тип
    public static class TypeExtensions
    {
        public static bool IsNullableType(this Type type)
        {
            return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
        }
    }
}

