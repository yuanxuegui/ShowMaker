using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;

namespace ShowMaker.Desktop.Models.Util
{
    public static class ShowFileEncryptDecrypt
    {
        //  Call this function to remove the key from memory after use for security
        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        static extern bool ZeroMemory(IntPtr Destination, int Length);

        // Function to Generate a 64 bits Key.
        static string GenerateKey()
        {
            // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

            // Use the Automatically generated key for Encryption. 
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }

        static byte[] GenerateKeyBytes()
        {
            // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

            // Use the Automatically generated key for Encryption. 
            return desCrypto.Key;
        }

        static void EncryptFile(string sInputFilename,
           string sOutputFilename,
           string sKey)
        {
            FileStream fsInput = new FileStream(sInputFilename,
               FileMode.Open,
               FileAccess.Read);

            FileStream fsEncrypted = new FileStream(sOutputFilename,
               FileMode.Create,
               FileAccess.Write);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(fsEncrypted,
               desencrypt,
               CryptoStreamMode.Write);

            byte[] bytearrayinput = new byte[fsInput.Length];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Close();

            fsInput.Close();
            fsEncrypted.Close();
        }

        static void DecryptFile(string sInputFilename,
           string sOutputFilename,
           string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 64 bit key and IV is required for this provider.
            //Set secret key For DES algorithm.
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            //Set initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            //Create a file stream to read the encrypted file back.
            FileStream fsread = new FileStream(sInputFilename,
               FileMode.Open,
               FileAccess.Read);
            //Create a DES decryptor from the DES instance.
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            //Create crypto stream set to read and do a 
            //DES decryption transform on incoming bytes.
            CryptoStream cryptostreamDecr = new CryptoStream(fsread,
               desdecrypt,
               CryptoStreamMode.Read);
            //Print the contents of the decrypted file.
            StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
            fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
            fsDecrypted.Flush();

            fsread.Close();
            fsDecrypted.Close();
        } 

        public static void SaveShowFile(string sourceFile)
        {
            // Must be 64 bits, 8 bytes.
            // Distribute this key to the user who will decrypt this file.
            string sSecretKey;
            byte[] secretKeyBytes;
            string tempFile = sourceFile + ".temp";

            // Get the Key for the file to Encrypt.
            secretKeyBytes = ShowFileEncryptDecrypt.GenerateKeyBytes();
            sSecretKey = ASCIIEncoding.ASCII.GetString(secretKeyBytes);

            // For additional security Pin the key.
            GCHandle gch = GCHandle.Alloc(sSecretKey, GCHandleType.Pinned);

            ShowFileEncryptDecrypt.EncryptFile(sourceFile, tempFile, sSecretKey);

            // Remove the Key from memory. 
            ShowFileEncryptDecrypt.ZeroMemory(gch.AddrOfPinnedObject(), sSecretKey.Length * 2);
            gch.Free();

            File.Delete(sourceFile);
            using (FileStream fs = File.Create(sourceFile))
            {
                fs.Write(secretKeyBytes, 0, 8);
                using (FileStream efs = File.OpenRead(tempFile))
                {
                    byte[] buffer = new byte[1024];
                    int count;
                    while((count = efs.Read(buffer, 0, 1024)) != 0)
                    {
                        fs.Write(buffer, 0, count);
                    }
                }
            }
            File.Delete(tempFile);
        }

        public static string LoadShowFile(string sourceFile)
        {
            string keyString;
            byte[] key = new byte[8];
            string tempFile = sourceFile + ".temp";
            string decryptFile = sourceFile + ".p";

            using (FileStream fs = File.Open(sourceFile, FileMode.Open, FileAccess.Read))
            {
                fs.Read(key, 0, 8);
                keyString = ASCIIEncoding.ASCII.GetString(key);
                using (FileStream tfs = File.Create(tempFile))
                {
                    byte[] buffer = new byte[1024];
                    int count;
                    while ((count = fs.Read(buffer, 0, 1024)) != 0)
                    {
                        tfs.Write(buffer, 0, count);
                    }
                }
                ShowFileEncryptDecrypt.DecryptFile(tempFile, decryptFile, keyString);
                File.Delete(tempFile);
            }
            return decryptFile;
        }
    }
}
