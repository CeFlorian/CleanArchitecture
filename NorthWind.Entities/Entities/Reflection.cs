using System.Reflection;

namespace NorthWind.Entities.Entities
{
    public class Reflection
    {
        public static T MapObjects<T>(object source)
        {
            Type sourceType = source.GetType();
            Type targetType = typeof(T);

            var targetInstance = Activator.CreateInstance(targetType);

            foreach (var targetProperty in targetType.GetProperties())
            {
                PropertyInfo sourceProperty = sourceType.GetProperty(targetProperty.Name);

                if (sourceProperty != null && sourceProperty.PropertyType == targetProperty.PropertyType)
                {
                    object value = sourceProperty.GetValue(source);
                    targetProperty.SetValue(targetInstance, value);
                }
            }

            return (T)targetInstance;
        }
    }
}
