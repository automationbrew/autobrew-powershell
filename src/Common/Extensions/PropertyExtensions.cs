namespace AutoBrew.PowerShell
{
    using System.Collections.Generic;
    using Models;

    /// <summary>
    /// Provides extensions for interacting with properties.
    /// </summary>
    public static class PropertyExtensions
    {
        /// <summary>
        /// Safely get the value of the given property, or return the default if no value is present in the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The extensions dictionary to search</param>
        /// <param name="property">The property to search for</param>
        /// <returns>The value stored in the dictionary, or the default if no value is specified.</returns>
        public static TValue GetProperty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey property)
        {
            return dictionary.ContainsKey(property) ? dictionary[property] : default;
        }

        /// <summary>
        /// Safely get the given property for the model.
        /// </summary>
        /// <param name="model">An instance of a class that implements the <see cref="IExtensibleModel"/> interface.</param>
        /// <param name="propertyType">The type of property to be obtained.</param>
        /// <returns>The value of the property in the given model, or null if the property is not set</returns>
        public static string GetProperty(this IExtensibleModel model, ExtendedPropertyType propertyType)
        {
            string result = null;

            if (model.IsPropertySet(propertyType))
            {
                result = model.ExtendedProperties.GetProperty(propertyType);
            }

            return result;
        }

        /// <summary>
        /// Determines if the given property has a value.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <param name="dictionary">The extensions dictionary to search</param>
        /// <param name="property">The property to search for</param>
        /// <returns><c>true</c> if the property has a value; otherwise <c>false</c>.</returns>
        public static bool IsPropertySet<TKey>(this IDictionary<TKey, string> dictionary, TKey property)
        {
            return dictionary.ContainsKey(property) && !string.IsNullOrEmpty(dictionary[property]);
        }

        /// <summary>
        /// Determines if the given property is set in the model.
        /// </summary>
        /// <param name="model">An instance of a class that implements the <see cref="IExtensibleModel" /> interface.</param>
        /// <param name="propertyType">The type of property to be checked.</param>
        /// <returns><c>true</c> if the property is set, otherwise <c>false</c>.</returns>
        public static bool IsPropertySet(this IExtensibleModel model, ExtendedPropertyType propertyType)
        {
            bool result = false;

            if (propertyType != ExtendedPropertyType.UnknownFutureValue)
            {
                result = model.ExtendedProperties.IsPropertySet(propertyType);
            }

            return result;
        }

        /// <summary>
        /// Removes the property with the specified type.
        /// </summary>
        /// <param name="model">An instance of a class that implements the <see cref="IExtensibleModel" /> interface.</param>
        /// <param name="propertyType">The type of property to be removed.</param>
        /// <returns><c>true</c> if the property was successfully removed; otherwise, <c>false</c>. When the specified property does not exist <c>false</c> will be returned.</returns>
        public static bool RemoveProperty(this IExtensibleModel model, ExtendedPropertyType propertyType)
        {
            return model.ExtendedProperties.Remove(propertyType);
        }

        /// <summary>
        /// Replace the value of the given property with a comma separated list of strings.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <param name="dictionary">The extensions dictionary to search</param>
        /// <param name="property">The property to search for</param>
        /// <param name="values">The strings to store in the property</param>
        public static void SetProperty<TKey>(this IDictionary<TKey, string> dictionary, TKey property, params string[] values)
        {
            if (values == null || values.Length == 0)
            {
                if (dictionary.ContainsKey(property))
                {
                    dictionary.Remove(property);
                }
            }
            else
            {
                dictionary[property] = string.Join(",", values);
            }
        }

        /// <summary>
        /// Safely set the given property for the model to the given values. If more than one value is provided, values are 
        /// represented as a comma-separated list.
        /// </summary>
        /// <param name="model">An instance of a class that implements the <see cref="IExtensibleModel"/> interface.</param>
        /// <param name="propertyType">The type of property to be updated.</param>
        /// <param name="values">The value to set for the property.</param>
        public static void SetProperty(this IExtensibleModel model, ExtendedPropertyType propertyType, params string[] values)
        {
            if (propertyType != ExtendedPropertyType.UnknownFutureValue)
            {
                model.ExtendedProperties.SetProperty(propertyType, values);
            }
        }
    }
}