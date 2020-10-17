using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AppMailManager.Managers
{
    public class HashMD5
    {
        public void StartMD5(string[] args=null)
        {
            bool exit = false;

            do
            {

                try
                {

                    if (args?.Length > 0 == false)
                    {
                        args = new string[] { Guid.NewGuid().ToString().Substring(0, 8) };

                    }

                    using (MD5 md5Hash = MD5.Create())
                    {
                        foreach (var source in args)
                        {

                            string hash = GetMd5Hash(md5Hash, source);

                            Console.WriteLine($"The MD5 hash of '{source}' is: ");
                            Console.WriteLine(hash);

                            Console.WriteLine("Verifying the hash...");

                            if (VerifyMd5Hash(md5Hash, source, hash))
                            {
                                Console.WriteLine("OK.");
                            }
                            else
                            {
                                Console.WriteLine("KO.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("type q if you want to exit.");

                string input = Console.ReadLine().Trim();

                if (input.Trim().ToUpper() == "Q")
                {

                    exit = true;

                }
                else if (!string.IsNullOrEmpty(input))
                {
                    args = input.Split('\n');

                }
                else
                {
                    args = null;
                }

            }
            while (exit == false);
        }

        public string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
