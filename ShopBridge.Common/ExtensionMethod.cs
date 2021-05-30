using Shop_Bridge.DTO;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace Shop_Bridge.Common
{
    public static class ExtensionMethod
    {
        public static bool IsNull(this object value)
        {
            return (value == null);
        }
        public static bool IsNotNull(this object value)
        {
            return (value != null);
        }
        public static bool HasRecords(this object value)
        {
            bool response = true;
            response = value != null;
            if (value is ICollection)
            {
                response = response && (value as ICollection).Count > 0;
            }
            return response;
        }

        public static string SerializeObject(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }
        public static T DeserializeObject<T>(this string value)
        {
            T response = default;
            try
            {
                response = JsonConvert.DeserializeObject<T>(value);
            }
            catch
            {
            }
            return response;
        }

        public static Expression<Func<T, bool>> ExpressionAnd<T>(this Expression<Func<T, bool>> expr, Expression<Func<T, bool>> exprToAnd)
        {
            Expression<Func<T, bool>> response = null;
            if (expr.IsNull())
                response = exprToAnd;
            else if (exprToAnd.IsNull())
                response = expr;
            else
                response = expr.And(exprToAnd);
            return response;
        }
        public static Expression<Func<T, bool>> ExpressionOr<T>(this Expression<Func<T, bool>> expr, Expression<Func<T, bool>> exprToOr)
        {
            Expression<Func<T, bool>> response = null;
            if (expr.IsNull())
                response = exprToOr;
            else if (exprToOr.IsNull())
                response = expr;
            else
                response = expr.Or(exprToOr);
            return response;
        }

        public static string Encrypt(this string PlainData)
        {
            return ByteArrayToString(EncryptStringToBytes(PlainData));
        }

        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private static byte[] EncryptStringToBytes(string plainText)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");

            byte[] encrypted;
            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Encoding.ASCII.GetBytes(ConfigManager.Instance.EncryptionKey.Substring(0, 32));
                rijAlg.IV = Encoding.ASCII.GetBytes(ConfigManager.Instance.EncryptionKey.Substring(32));

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }

        public static Dictionary<string, string> DecryptAccessTokenAES(this string accessToken)
        {
            try
            {
                var CipherText = StringToByteArray(accessToken);
                string PlainText = DecryptStringFromBytes(CipherText, Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(ConfigManager.Instance.EncryptionKey.Substring(0, 32))), Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(ConfigManager.Instance.EncryptionKey.Substring(32))));
                return DeserializeObject<Dictionary<string, string>>(PlainText);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
            return null;
        }

        private static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, string Key, string IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Encoding.ASCII.GetBytes(Key);
                rijAlg.IV = Encoding.ASCII.GetBytes(IV);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    //Write all data to the stream.
                    plaintext = srDecrypt.ReadToEnd();
                }
            }

            return plaintext;
        }
    }
}
