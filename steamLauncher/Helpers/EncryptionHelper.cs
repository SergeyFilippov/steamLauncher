namespace SteamLauncher.Helpers
{
    using System;
    using System.Buffers.Text;
    using System.IO;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;

    public static class EncryptionHelper
    {
        /// <summary>
        /// The salt.
        /// </summary>
        private static string salt = Convert.ToBase64String(Encoding.Default.GetBytes("ababagalamaga"));

        /// <summary>
        /// The decrypt.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="encrypted_value">
        /// The encrypted_value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Decrypt(string password, string encrypted_value)
        {
            string decrypted;

            using (var aes = Aes.Create())
            {
                var keys = GetAesKeyAndIV(password, aes);
                aes.Key = keys.Item1;
                aes.IV = keys.Item2;

                // create a decryptor to perform the stream transform.
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                // create the streams used for encryption.
                var encrypted_bytes = ToByteArray(encrypted_value);
                using (var memory_stream = new MemoryStream(encrypted_bytes))
                {
                    using (var crypto_stream = new CryptoStream(memory_stream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(crypto_stream))
                        {
                            decrypted = reader.ReadToEnd();
                        }
                    }
                }
            }

            return decrypted;
        }

        /// <summary>
        /// The encrypt.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="plain_text">
        /// The plain_text.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Encrypt(string password, string plain_text)
        {
            string encrypted;

            using (var aes = Aes.Create())
            {
                var keys = GetAesKeyAndIV(password, aes);
                aes.Key = keys.Item1;
                aes.IV = keys.Item2;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var memory_stream = new MemoryStream())
                {
                    using (var crypto_stream = new CryptoStream(memory_stream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(crypto_stream))
                        {
                            writer.Write(plain_text);
                        }

                        var encrypted_bytes = memory_stream.ToArray();
                        encrypted = ToString(encrypted_bytes);
                    }
                }
            }

            return encrypted;
        }

        /// <summary>
        /// The random string.
        /// </summary>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <param name="lowerCase">
        /// The lower case.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        /// <summary>
        /// The get usable string.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetUsableString(SecureString input)
        {
            var result = new System.Net.NetworkCredential(string.Empty, input).Password;
            return result;
        }

        private static byte[] ToByteArray(string input)
        {
            return Convert.FromBase64String(input);
        }

        private static string ToString(byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        private static Tuple<byte[], byte[]> GetAesKeyAndIV(string password, SymmetricAlgorithm symmetricAlgorithm)
        {
            const int bits = 8;
            var key = new byte[16];
            var iv = new byte[16];

            var derive_bytes = new Rfc2898DeriveBytes(password, ToByteArray(salt));
            key = derive_bytes.GetBytes(symmetricAlgorithm.KeySize / bits);
            iv = derive_bytes.GetBytes(symmetricAlgorithm.BlockSize / bits);

            return new Tuple<byte[], byte[]>(key, iv);
        }
    }
}