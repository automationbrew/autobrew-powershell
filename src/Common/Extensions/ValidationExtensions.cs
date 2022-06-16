namespace AutoBrew.PowerShell
{
    using System;
    using System.Globalization;
    using Properties;

    /// <summary>
    /// Provides extension for validating input.
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Ensures that a given object is not null. Throws an exception otherwise.
        /// </summary>
        /// <param name="objectToValidate">The object we are validating.</param>
        /// <param name="caption">The name to report in the exception.</param>
        public static void AssertNotNull(this object objectToValidate, string caption)
        {
            if (objectToValidate == null)
            {
                throw new ArgumentNullException(caption);
            }
        }

        /// <summary>
        /// Ensures that a string is not empty. Throws an exception if so.
        /// </summary>
        /// <param name="nonEmptyString">The string to validate.</param>
        /// <param name="caption">The name to report in the exception.</param>
        public static void AssertNotEmpty(this string nonEmptyString, string caption = null)
        {
            if (string.IsNullOrWhiteSpace(nonEmptyString))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.AssertStringNotEmptyInvalidError, caption ?? Resources.AssertStringNotEmptyInvalidPrefix));
            }
        }

        /// <summary>
        /// Ensures that a given number is greater than zero. Throws an exception otherwise.
        /// </summary>
        /// <param name="number">The number to validate.</param>
        /// <param name="caption">The name to report in the exception.</param>
        public static void AssertPositive(this int number, string caption = null)
        {
            if (number <= 0)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.AssertNumberPositiveInvalidError, caption ?? Resources.AssertNumberPositiveInvalidPrefix));
            }
        }

        /// <summary>
        /// Ensures that a given number is greater than zero. Throws an exception otherwise.
        /// </summary>
        /// <param name="number">The number to validate.</param>
        /// <param name="caption">The name to report in the exception.</param>
        public static void AssertPositive(this decimal number, string caption = null)
        {
            if (number <= 0)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.AssertNumberPositiveInvalidError, caption ?? Resources.AssertNumberPositiveInvalidPrefix));
            }
        }
    }
}