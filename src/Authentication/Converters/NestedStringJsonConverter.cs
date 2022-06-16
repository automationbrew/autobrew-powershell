namespace AutoBrew.PowerShell.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Provides the ability to convert a nested string of JSON to a specific resource.
    /// </summary>
    /// <typeparam name="TResource">The type of resource handled by the converter.</typeparam>
    internal class NestedStringJsonConverter<TResource> : JsonConverter<TResource>
    {
        /// <summary>
        /// Converts the JSON to the desired resource.
        /// </summary>
        /// <param name="reader">The stream that provides read-only access to UTF-8 encoded JSON text.</param>
        /// <param name="typeToConvert">The type for the desired resource.</param>
        /// <param name="options">The options to be used with the JSON serializer.</param>
        /// <returns>The resource that was deserialized from JSON.</returns>
        public override TResource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<TResource>(reader.GetString(), options);
        }

        /// <summary>
        /// Writes a specified value as JSON.
        /// </summary>
        /// <param name="writer">The stream to be used when writing the JSON.</param>
        /// <param name="value">The value to be written as JSON to the stream.</param>
        /// <param name="options">THe options to be used with the JSON serializer.</param>
        public override void Write(Utf8JsonWriter writer, TResource value, JsonSerializerOptions options)
        {
            /* Intentionally blank because this converter is only intended to be used to deserialize nested strings of JSON. */
        }
    }
}