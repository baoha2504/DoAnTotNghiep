using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MTA_Mobile_Forensic.Support
{
    internal class hash
    {
        public string ComputeMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return ConvertToHexString(hashBytes);
            }
        }

        public string ComputeSHA128(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                return ConvertToHexString(hashBytes);
            }
        }

        public string ComputeSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return ConvertToHexString(hashBytes);
            }
        }

        public string ComputeSHA512(string input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));
                return ConvertToHexString(hashBytes);
            }
        }

        public string ComputeFileMD5(string filePath)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = md5.ComputeHash(stream);
                    return ConvertToHexString(hashBytes);
                }
            }
        }

        public string ComputeFileSHA128(string filePath)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = sha1.ComputeHash(stream);
                    return ConvertToHexString(hashBytes);
                }
            }
        }

        public string ComputeFileSHA256(string filePath)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = sha256.ComputeHash(stream);
                    return ConvertToHexString(hashBytes);
                }
            }
        }

        public string ComputeFileSHA512(string filePath)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = sha512.ComputeHash(stream);
                    return ConvertToHexString(hashBytes);
                }
            }
        }

        private string ConvertToHexString(byte[] hashBytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
