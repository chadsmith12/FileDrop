using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace FileDrop.Web.Helpers
{
    public static class FileHelpers
    {
        public static void EncryptFileToDisk(byte[] file, string outputPath, string key)
        {
            var salt = new byte[] { 0x27, 0xbc, 0xf0, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x06, 0xaf, 0x4d, 0x08, 0x22, 0x3d };
            var derivedBytes = new Rfc2898DeriveBytes(key, salt);

            var test = Encrypt(file, key);

            var cryptoFileStream = new FileStream(outputPath, FileMode.Create);
            var rmCrypt = new RijndaelManaged();
            var cs = new CryptoStream(cryptoFileStream, rmCrypt.CreateEncryptor(derivedBytes.GetBytes(32), derivedBytes.GetBytes(16)), CryptoStreamMode.Write);

            foreach (var data in file)
                cs.WriteByte(data);

            cs.Close();
            cryptoFileStream.Close();
        }

        public static byte[] Encrypt(byte[] data, string key)
        {
            var salt = new byte[] { 0x27, 0xbc, 0xf0, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x06, 0xaf, 0x4d, 0x08, 0x22, 0x3d };
            var derivedBytes = new Rfc2898DeriveBytes(key, salt);
            var algorithm = new RijndaelManaged();

            using (var encryptor = algorithm.CreateEncryptor(derivedBytes.GetBytes(32), derivedBytes.GetBytes(16)))
            {
                return DoCrypto(encryptor, data);
            }
        }

        public static byte[] Decrypt(byte[] data, string key)
        {
            var salt = new byte[] { 0x27, 0xbc, 0xf0, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x06, 0xaf, 0x4d, 0x08, 0x22, 0x3d };
            var derivedBytes = new Rfc2898DeriveBytes(key, salt);
            var algorithm = new RijndaelManaged();

            using (var decryptor = algorithm.CreateDecryptor(derivedBytes.GetBytes(32), derivedBytes.GetBytes(16)))
            {
                return DoCrypto(decryptor, data);
            }
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="mapPath">The map path.</param>
        /// <returns></returns>
        public static string GetPath(string filename, string mapPath)
        {
            var orginalDirectory =
                        new DirectoryInfo(string.Format("{0}Uploads", mapPath));
            var path = Path.Combine(orginalDirectory.ToString(), "uploadpath");
            var exists = Directory.Exists(path);

            if (!exists)
            {
                Directory.CreateDirectory(path);
            }

            var fullPath = string.Format("{0}\\{1}", path, filename);

            return fullPath;
        }

        /// <summary>
        /// Byteses to mega bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static double BytesToMegaBytes(long bytes)
        {
            return (bytes/1024f)/1024f;
        }

        /// <summary>
        /// Reads all bytes.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(string fullPath)
        {
            return File.ReadAllBytes(fullPath);
        }

        /// <summary>
        /// To the data URL.
        /// </summary>
        /// <param name="base64">The base64.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string ToDataUrl(string base64, string type)
        {
            return string.Format("data:{0};base64,{1}", type, base64);
        }

        public static bool IsImage(string type)
        {
            var fileType = type.Split('/')[0];

            return fileType == "image";
        }

        private static byte[] DoCrypto(ICryptoTransform cryptoTransform, byte[] data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }
    }
}