using AppX.Client.Infrastructure.Services.Validator;
using System.Security.Cryptography;

namespace AppX.Client.Infrastructure.Services.TOTP
{
    public class TOTPGenerator
    {
        private const int DigitLength = 6; // Number of digits in the generated code

        // Generate a 6-digit TOTP code based on the provided secret key and current time
        public static string GenerateCode(string secretKey)
        {
            long counter = GetCurrentCounter(); // Get the current time-based counter

            //byte[] keyBytes = Base32Decode(secretKey); // Decode the secret key from base32

            var IsValidBase64 = Base64Validator.IsValidBase64(secretKey);

            if (!IsValidBase64)
            {
                throw new ArgumentException("GenerateCode: the parameter is an invalid base64.");
            }

            string code = "";

            byte[] keyBytes = Base64Decode(secretKey); // Decode the secret key from base32

            // Convert the counter to a byte array (8 bytes, big-endian)
            byte[] counterBytes = BitConverter.GetBytes(counter);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(counterBytes);
            }

            // Create an HMAC-SHA1 instance with the key
            //using (var hmac = new HMACSHA1(keyBytes))
            using (var hmac = new HMACSHA512(keyBytes))
            {
                // Compute the HMAC hash of the counter bytes
                byte[] hash = hmac.ComputeHash(counterBytes);

                // Take the last 4 bits of the hash to determine the start offset
                int offset = hash[hash.Length - 1] & 0x0F;

                // Extract 4 bytes starting from the offset to generate a 32-bit integer
                int binaryCode = hash[offset] << 24 | hash[offset + 1] << 16 | hash[offset + 2] << 8 | hash[offset + 3];

                // Take the last 6 digits of the binary code and convert it to a string
                code = (binaryCode % (int)Math.Pow(10, DigitLength)).ToString("D6");
            }

            return code;
        }

        // Get the current time-based counter (seconds since Unix epoch divided by time step)
        private static long GetCurrentCounter()
        {
            long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            const long TimeStep = 30; // Time step (30 seconds)
            return unixTimestamp / TimeStep;
        }

        // Decode the secret key from base64 encoding
        private static byte[] Base64Decode(string input)
        {
            return Convert.FromBase64String(input);
        }

        // Decode the secret key from base32 encoding
        private static byte[] Base32Decode(string input)
        {
            const string Base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

            input = input.Trim().Replace(" ", "").ToUpper();
            byte[] buffer = new byte[input.Length * 5 / 8];

            int bytes = 0, bits = 0, index = 0;

            foreach (char c in input)
            {
                int value = Base32Chars.IndexOf(c);
                if (value < 0)
                {
                    throw new ArgumentException("Invalid base32 character");
                }

                buffer[index] |= (byte)(value << bits);
                bits += 5;

                if (bits >= 8)
                {
                    index++;
                    bits -= 8;
                }
            }

            return buffer;
        }
    }
}
