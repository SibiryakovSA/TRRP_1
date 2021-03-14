using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TRRP_Lab1
{
    static class SecureManager
    {
        static byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
        static public bool Encrypt(string message)
        {
            try
            {
                //Create a file stream
                using FileStream myStream = new FileStream("Data.sym", FileMode.OpenOrCreate);

                //Create a new instance of the default Aes implementation class  
                // and configure encryption key.  
                using Aes aes = Aes.Create();
                aes.Key = key;

                //Stores IV at the beginning of the file.
                //This information will be used for decryption.
                byte[] iv = aes.IV;
                myStream.Write(iv, 0, iv.Length);

                //Create a CryptoStream, pass it the FileStream, and encrypt
                //it with the Aes class.  
                using CryptoStream cryptStream = new CryptoStream(
                    myStream,
                    aes.CreateEncryptor(),
                    CryptoStreamMode.Write);

                //Create a StreamWriter for easy writing to the
                //file stream.  
                using StreamWriter sWriter = new StreamWriter(cryptStream);

                //Write to the stream.  
                sWriter.WriteLine(message);
                return true;
            }
            catch
            {
                return false;
            }
        }

        static public string Decrypt()
        {
            try
            {
                //Create a file stream.
                using FileStream myStream = new FileStream("Data.sym", FileMode.Open);

                //Create a new instance of the default Aes implementation class
                using Aes aes = Aes.Create();

                //Reads IV value from beginning of the file.
                byte[] iv = new byte[aes.IV.Length];
                myStream.Read(iv, 0, iv.Length);

                //Create a CryptoStream, pass it the file stream, and decrypt
                //it with the Aes class using the key and IV.
                using CryptoStream cryptStream = new CryptoStream(
                   myStream,
                   aes.CreateDecryptor(key, iv),
                   CryptoStreamMode.Read);

                //Read the stream.
                using StreamReader sReader = new StreamReader(cryptStream);

                return sReader.ReadToEnd().Trim();
            }
            catch
            {
                return null;
            }
        }

    }
}
