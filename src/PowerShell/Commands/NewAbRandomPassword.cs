namespace AutoBrew.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Security;
    using System.Security.Cryptography;

    /// <summary>
    /// Cmdlet that provides the ability to generate a random password.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "AbRandomPassword")]
    [OutputType(typeof(SecureString))]
    public class NewAbRandomPassword : ModuleCmdlet
    {
        /// <summary>
        /// An array of punctuation characters that can be used when generating a random password.
        /// </summary>
        private static readonly char[] Punctuations = "!@#$%^&*()_-+=[{]};:>|./?".ToCharArray();

        /// <summary>
        /// Gets or sets the length of the password.
        /// </summary>
        [Parameter(HelpMessage = "The number of characters in the generated password. The length must be between 8 and 128 characters.", Mandatory = false)]
        [ValidateRange(8, 128)]
        public int? Length { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of special characters in the password.
        /// </summary>
        [Parameter(HelpMessage = "The minimum number of non-alphanumeric characters (such as @, #, !, %, &, and so on) in the generated password. The number must be 2 or more.", Mandatory = false)]
        [ValidateRange(2, 32)]
        public int? NumberOfNonAlphanumericCharacters { get; set; }

        /// <summary>
        /// Performs the actions associated with the command.
        /// </summary>
        protected override void PerformCmdlet()
        {
            int length = Length ?? 8;
            int numberOfNonAlphanumericCharacters = NumberOfNonAlphanumericCharacters ?? 2;

            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] byteBuffer = new byte[length];
            string generated = null;

            rng.GetBytes(byteBuffer);

            char[] characterBuffer = new char[length];
            int count = 0;

            for (int iter = 0; iter < length; iter++)
            {
                int i = byteBuffer[iter] % 87;

                if (i < 10)
                {
                    characterBuffer[iter] = (char)('0' + i);
                }
                else if (i < 36)
                {
                    characterBuffer[iter] = (char)('A' + i - 10);
                }
                else if (i < 62)
                {
                    characterBuffer[iter] = (char)('a' + i - 36);
                }
                else
                {
                    characterBuffer[iter] = Punctuations[i - 62];
                    count++;
                }
            }

            if (count >= numberOfNonAlphanumericCharacters)
            {
                generated = new string(characterBuffer);
            }

            if (string.IsNullOrEmpty(generated))
            {
                Random rand = new();

                for (int j = 0; j < numberOfNonAlphanumericCharacters - count; j++)
                {
                    int k;

                    do
                    {
                        k = rand.Next(0, length);
                    }
                    while (!char.IsLetterOrDigit(characterBuffer[k]));

                    characterBuffer[k] = Punctuations[rand.Next(0, Punctuations.Length)];
                }

                generated = new string(characterBuffer);
            }

            WriteObject(generated.AsSecureString());
        }
    }
}