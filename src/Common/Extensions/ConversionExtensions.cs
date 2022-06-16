namespace AutoBrew.PowerShell
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// Provides extensions for converting objects.
    /// </summary>
    public static class ConversionExtensions
    {
        /// <summary>
        /// Converts the string value to an instance of <see cref="SecureString"/>.
        /// </summary>
        /// <param name="value">The string value to be converted.</param>
        /// <returns>An instance of <see cref="SecureString"/> that represents the string.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is empty or null.
        /// </exception>
        public static SecureString AsSecureString(this string value)
        {
            SecureString secureValue = new();

            value.AssertNotEmpty(nameof(value));

            foreach (char c in value)
            {
                secureValue.AppendChar(c);
            }

            secureValue.MakeReadOnly();

            return secureValue;
        }

        /// <summary>
        /// Converts a secure string to a regular string.
        /// </summary>
        /// <param name="secureString">Specifies the secure string to convert.</param> 
        /// <returns>The string that was converted from a secure string </returns>
        public static string AsString(this SecureString secureString)
        {
            secureString.AssertNotNull(nameof(secureString));

            IntPtr stringPtr = IntPtr.Zero;

            try
            {
                stringPtr = Marshal.SecureStringToBSTR(secureString);
                return Marshal.PtrToStringBSTR(stringPtr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(stringPtr);
            }
        }
    }
}