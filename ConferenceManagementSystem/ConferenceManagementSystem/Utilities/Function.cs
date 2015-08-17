using ConferenceManagementSystem.DataAccessLayer;
using ConferenceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ConferenceManagementSystem.Utilities
{
    public class Function
    {
        public static ConferenceManagementContext db = new ConferenceManagementContext();

        public static string Encrypt(string text)
        {
            string EncryptionKey = "9mare";
            byte[] dataBytes = Encoding.Unicode.GetBytes(text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(dataBytes, 0, dataBytes.Length);
                        cs.Close();
                    }
                    text = Convert.ToBase64String(ms.ToArray());
                }
            }
            return text;
        }

        //Decrypt password
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "9mare";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new
                    Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static bool LoginAsUser(string username, string password)
        {
            bool Success = false;
            {
                User user = db.Users.FirstOrDefault(u => u.Username == username);

                if (user != null)
                {
                    if (Decrypt(user.encryptedPassword) == password)
                    {
                        Success = true;
                    }
                }
                return Success;
            }
        }

        public static bool LoginAsAdmin(string username, string password)
        {
            bool Success = false;
            {
                Admin admin = db.Admins.FirstOrDefault(u => u.Username == username);

                if (admin != null)
                {
                    if (Decrypt(admin.Password) == password)
                    {
                        Success = true;
                    }
                }
                return Success;
            }
        }

        public static bool LoginAsOrganizer(string username, string password)
        {
            bool Success = false;
            {
                Conference organizer = db.Conferences.FirstOrDefault(u => u.Username == username);

                if (organizer != null)
                {
                    if (Decrypt(organizer.encryptedPassword) == password)
                    {
                        Success = true;
                    }
                }
                return Success;
            }
        }
    }
}