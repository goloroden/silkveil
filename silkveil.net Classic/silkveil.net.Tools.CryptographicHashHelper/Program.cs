using cherryflavored.net.ExtensionMethods.System.Security.Cryptography;

using System;

namespace silkveil.net.Tools.CryptographicHashHelper
{
    /// <summary>
    /// Represents the cryptographic hash helper application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Executes the application.
        /// </summary>
        public static void Main()
        {
            string text;

            while (true)
            {
                Console.Write("Select (h)ash or (q)uit: ");
                string action = Console.ReadLine();

                switch (action)
                {
                    case "h":
                        Console.Write("Text to hash: ");
                        text = Console.ReadLine();

                        Console.WriteLine("Hashed text: {0}", text.Hash());
                        break;
                    case "q":
                        return;
                }
            }
        }
    }
}