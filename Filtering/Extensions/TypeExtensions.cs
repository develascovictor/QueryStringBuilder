using System;
using System.Linq;

namespace Filtering.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetPropertyType<T>(this string propertyName) where T : class
        {
            var propertyType = (Type) null;
            var instance = Activator.CreateInstance<T>();
            var properties = instance.GetType().GetProperties();

            if (propertyName.Contains("."))
            {
                var navigationPropertyName = propertyName.Split('.')[0];
                var navigationChildPropertyName = propertyName.Split('.')[1];

                var navigationPropertyType = properties.FirstOrDefault(p => string.Equals(p.Name, navigationPropertyName, StringComparison.OrdinalIgnoreCase))?.PropertyType;

                if (navigationPropertyType != null)
                {
                    var navigationPropertyInstance = Activator.CreateInstance(navigationPropertyType);
                    propertyType = navigationPropertyInstance.GetType().GetProperties().FirstOrDefault(p => string.Equals(p.Name, navigationChildPropertyName, StringComparison.OrdinalIgnoreCase))?.PropertyType;
                }
            }

            else
            {
                propertyType = properties.FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase))?.PropertyType;
            }

            return propertyType;
        }
    }
}