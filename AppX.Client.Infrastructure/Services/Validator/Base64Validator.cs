namespace AppX.Client.Infrastructure.Services.Validator
{
    public class Base64Validator
    {
        // Validate whether the input string is a valid Base64 string
        public static bool IsValidBase64(string input)
        {
            try
            {
                // Attempt to decode the input string
                byte[] bytes = Convert.FromBase64String(input);
                return true; // If successful, it's a valid Base64 string
            }
            catch (FormatException)
            {
                return false; // If an exception is thrown, it's not a valid Base64 string
            }
        }
    }
}
