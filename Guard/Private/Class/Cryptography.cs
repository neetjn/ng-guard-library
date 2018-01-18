using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Guard.Private
{
    class Cryptography
    {

        private const string passPhrase = "LtOpxegDASwn5a5Nfxqqj76geJEwgPjhYbm";
        private const string saltValue = "tEiQc8dIdajpQlqbSORkRxz4dwFVMbLO3vb";
        private const string hashAlgorithm = "SHA1";
        private const int passwordIterations = 1337;
        private const string initVector = "~ZCS{a}-csE(;(Eyq";
        private const int keySize = 256;

        public string StringCleaner(string NormalString)
        {
            const string BannedString = ";,()-*&^%$#@!~`{}|?<>:-";
            string CleanedString = NormalString;
            foreach (char c in BannedString)
            {
                CleanedString = CleanedString.Replace(c.ToString(), "");
            }
            return CleanedString;
        }
        public string doHASH(string hType, string hString)
        {

            string hashed = hString;
            switch (hType)
            {

                case "ap":
                    return apHashString(hashed);
                case "md5":
                    return MD5Hash(hashed);
                case "rot13":
                    return ROT13(hashed);
                case "crc32":
                    return CRC32(hashed);
                case "ripemd160":
                    return RIPEMD160Hash(hashed);
                case "sha1":
                    return SHA1Hash(hashed);
                case "sha256":
                    return SHA256Hash(hashed);
                case "sha348":
                    return SHA348Hash(hashed);
                case "sha512":
                    return SHA512Hash(hashed);
            }

            return hashed;
        }
        public string qEncode(string data)
        {

            string encoded = null;
            encoded = BASE64_Encode(BASE64_Encode(data));
            return encoded;
        }
        public string qDecode(string data)
        {

            string decoded = null;
            decoded = BASE64_Decode(BASE64_Decode(data));
            return decoded;
        }
        public string SHAEncrypt(string data)
        {

            return SaltedShaEncrypt(data);
        }
        public string SHADecrypt(string data)
        {

            return SaltedShaDecrypt(data);
        }

        private UInt32 APHash(char[] str, uint len)
        {

            uint hash = 0xAAAAAAAA;
            uint i = 0;
            uint s = 0;

            for (i = 0; i < len; s++, i++)
            {
                hash ^= ((i & 1) == 0) ? ((hash << 7) ^ (str[s]) * (hash >> 5)) :
                               (~((hash << 17) + ((str[s]) ^ (hash >> 8))));
            }

            return hash;
        }
        private string apHashString(string sString)
        {

            return (APHash(sString.ToCharArray(), (uint)sString.Length)).ToString();
        }
        private string CRC32(string data)
        {
            
            return String.Format("{0:X}", data.GetHashCode());
        }
        private string SaltedShaEncrypt(string data)
        {

            byte[] bytes = Encoding.ASCII.GetBytes(initVector), rgbSalt = Encoding.ASCII.GetBytes(saltValue), buffer = Encoding.UTF8.GetBytes(data);
            byte[] rgbKey = new PasswordDeriveBytes(passPhrase, rgbSalt, hashAlgorithm, passwordIterations).GetBytes(keySize / 8);
            var managed = new RijndaelManaged() { Mode = CipherMode.CBC };
            var transform = managed.CreateEncryptor(rgbKey, bytes);
            var stream = new MemoryStream();
            var stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            byte[] inArray = stream.ToArray();
            stream.Close();
            stream2.Close();
            return Convert.ToBase64String(inArray);
        }
        private string SaltedShaDecrypt(string data)
        {

            byte[] bytes = Encoding.ASCII.GetBytes(initVector);
            byte[] rgbSalt = Encoding.ASCII.GetBytes(saltValue);
            byte[] buffer = Convert.FromBase64String(data);
            byte[] rgbKey = new PasswordDeriveBytes(passPhrase, rgbSalt, hashAlgorithm, passwordIterations).GetBytes(keySize / 8);
            var managed = new RijndaelManaged() { Mode = CipherMode.CBC };
            var transform = managed.CreateDecryptor(rgbKey, bytes);
            var stream = new MemoryStream(buffer);
            var stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            byte[] buffer5 = new byte[buffer.Length];
            int count = stream2.Read(buffer5, 0, buffer5.Length);
            stream.Close();
            stream2.Close();
            return Encoding.UTF8.GetString(buffer5, 0, count);
        }
        private string ROT13(string input)
        {

            char[] inputCharacters = input.ToCharArray();
            var output = new StringBuilder();
            for (int i = 0; i <= input.Length - 1; i++) output.Append((char)inputCharacters[i] ^ 13);
            return output.ToString();
        }
        private string ZARA128_Encode(string input)
        {

            var output = new StringBuilder();
            foreach (char c in input)
            {
                output.Append(((int)c + 312) + " ");
            }
            return output.ToString();
        }
        private string ZARA128_Decode(string input)
        {

            var output = new StringBuilder();
            string[] strings = input.Split(' ');
            Array.ForEach(strings, s => output.Append((char)Convert.ToInt32(s) - 312));
            return output.ToString();
        }
        private string BASE64_Decode(string input)
        {

            return ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(input));
        }
        private string BASE64_Encode(string input)
        {
            return Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(input));
        }
        private string String2Hex(string input)
        {

            var output = new StringBuilder();
            char[] chars = input.ToCharArray();
            Array.ForEach(chars, c => output.Append(String.Format("{0:X} ", (int)c)));
            return output.ToString().Substring(0, output.Length - 1);
        }
        private string Hex2String(string input)
        {

            var output = new StringBuilder();
            string[] strings = input.Split(' ');
            Array.ForEach(strings, s => output.Append((char)Convert.ToInt32(s, 16)));
            return output.ToString();
        }
        private string MD5Hash(string input)
        {

            var MD5CryptoService = new MD5CryptoServiceProvider();
            byte[] byteHash = MD5CryptoService.ComputeHash(ASCIIEncoding.ASCII.GetBytes(input));
            return BitConverter.ToString(byteHash).Replace("-", null);
        }
        private string MD5(string input)
        {

            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }
        private string RIPEMD160Hash(string input)
        {

            var RIPEMD160Manager = new RIPEMD160Managed();
            byte[] byteHash = RIPEMD160Manager.ComputeHash(ASCIIEncoding.ASCII.GetBytes(input));
            return BitConverter.ToString(byteHash).Replace("-", null);
        }
        private string SHA1Hash(string input)
        {

            var SHA1Manager = new SHA1Managed();
            byte[] byteHash = SHA1Manager.ComputeHash(ASCIIEncoding.ASCII.GetBytes(input));
            return BitConverter.ToString(byteHash).Replace("-", null);
        }
        private string SHA256Hash(string input)
        {

            var SHA256Manager = new SHA256Managed();
            byte[] byteHash = SHA256Manager.ComputeHash(ASCIIEncoding.ASCII.GetBytes(input));
            return BitConverter.ToString(byteHash).Replace("-", null);
        }
        private string SHA348Hash(string input)
        {

            var SHA348Manager = new SHA384Managed();
            byte[] byteHash = SHA348Manager.ComputeHash(ASCIIEncoding.ASCII.GetBytes(input));
            return BitConverter.ToString(byteHash).Replace("-", null);
        }
        private string SHA512Hash(string input)
        {

            var SHA512Manager = new SHA512Managed();
            byte[] byteHash = SHA512Manager.ComputeHash(ASCIIEncoding.ASCII.GetBytes(input));
            return BitConverter.ToString(byteHash).Replace("-", null);
        }
        private string DES_Encrypt(string input, string password)
        {

            var DESCryptoService = new DESCryptoServiceProvider();
            var MD5CryptoService = new MD5CryptoServiceProvider();
            string ouput = string.Empty;
            try
            {
                byte[] hash = new byte[8];
                byte[] temp = MD5CryptoService.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
                Array.Copy(temp, 0, hash, 0, 8);
                DESCryptoService.Key = hash;
                DESCryptoService.Mode = CipherMode.ECB;
                var DESEncryptor = DESCryptoService.CreateEncryptor();
                byte[] buffer = ASCIIEncoding.ASCII.GetBytes(input);
                ouput = Convert.ToBase64String(DESEncryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch { }
            return ouput;
        }
        private string DES_Decrypt(string input, string password)
        {

            var DESCryptoService = new DESCryptoServiceProvider();
            var MD5CryptoService = new MD5CryptoServiceProvider();
            string ouput = string.Empty;
            try
            {
                byte[] hash = new byte[8];
                byte[] temp = MD5CryptoService.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
                Array.Copy(temp, 0, hash, 0, 8);
                DESCryptoService.Key = hash;
                DESCryptoService.Mode = CipherMode.ECB;
                var DESEncryptor = DESCryptoService.CreateDecryptor();
                byte[] buffer = Convert.FromBase64String(input);
                ouput = ASCIIEncoding.ASCII.GetString(DESEncryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch { }
            return ouput;
        }
        private string AES_Encrypt(string input, string password)
        {

            var RijndaelManager = new RijndaelManaged();
            var MD5CryptoService = new MD5CryptoServiceProvider();
            string output = string.Empty;
            try
            {
                byte[] hash = new byte[32];
                byte[] temp = MD5CryptoService.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);
                RijndaelManager.Key = hash;
                RijndaelManager.Mode = CipherMode.ECB;
                var RijndaelEncryptor = RijndaelManager.CreateEncryptor();
                byte[] buffer = ASCIIEncoding.ASCII.GetBytes(input);
                output = Convert.ToBase64String(RijndaelEncryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch { }
            return output;
        }
        private string AES_Decrypt(string input, string password)
        {

            var RijndaelManager = new RijndaelManaged();
            var MD5CryptoService = new MD5CryptoServiceProvider();
            string output = string.Empty;
            try
            {
                byte[] hash = new byte[32];
                byte[] temp = MD5CryptoService.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);
                RijndaelManager.Key = hash;
                RijndaelManager.Mode = CipherMode.ECB;
                var RijndaelEncryptor = RijndaelManager.CreateDecryptor();
                byte[] buffer = Convert.FromBase64String(input);
                output = ASCIIEncoding.ASCII.GetString(RijndaelEncryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch { }
            return output;
        }
        private string TripleDES_Encrypt(string input, string password)
        {

            var TripleDESCryptoService = new TripleDESCryptoServiceProvider();
            var MD5CryptoService = new MD5CryptoServiceProvider();
            string output = string.Empty;
            try
            {
                byte[] hash = new byte[24];
                byte[] temp = MD5CryptoService.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 8);
                TripleDESCryptoService.Key = hash;
                TripleDESCryptoService.Mode = CipherMode.ECB;
                var TripleDESDecryptor = TripleDESCryptoService.CreateEncryptor();
                byte[] buffer = ASCIIEncoding.ASCII.GetBytes(input);
                output = Convert.ToBase64String(TripleDESDecryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch { }
            return output;
        }
        private string TripleDES_Decrypt(string input, string password)
        {

            var TripleDESCryptoService = new TripleDESCryptoServiceProvider();
            var MD5CryptoService = new MD5CryptoServiceProvider();
            string output = string.Empty;
            try
            {
                byte[] hash = new byte[24];
                byte[] temp = MD5CryptoService.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 8);
                TripleDESCryptoService.Key = hash;
                TripleDESCryptoService.Mode = CipherMode.ECB;
                var TripleDESDecryptor = TripleDESCryptoService.CreateDecryptor();
                byte[] buffer = Convert.FromBase64String(input);
                output = ASCIIEncoding.ASCII.GetString(TripleDESDecryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch { }
            return output;
        }
        private string RC2_Encrypt(string input, string password)
        {

            var RC2CryptoService = new RC2CryptoServiceProvider();
            var MD5CryptoService = new MD5CryptoServiceProvider();
            string output = string.Empty;
            try
            {
                byte[] hash = MD5CryptoService.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
                RC2CryptoService.Key = hash;
                RC2CryptoService.Mode = CipherMode.ECB;
                var RC2Encryptor = RC2CryptoService.CreateEncryptor();
                byte[] buffer = ASCIIEncoding.ASCII.GetBytes(input);
                output = Convert.ToBase64String(RC2Encryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch { }
            return output;
        }
        private string RC2_Decrypt(string input, string password)
        {

            var RC2CryptoService = new RC2CryptoServiceProvider();
            var MD5CryptoService = new MD5CryptoServiceProvider();
            string output = string.Empty;
            try
            {
                byte[] hash = MD5CryptoService.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
                RC2CryptoService.Key = hash;
                RC2CryptoService.Mode = CipherMode.ECB;
                var RC2Decryptor = RC2CryptoService.CreateDecryptor();
                byte[] buffer = Convert.FromBase64String(input);
                output = ASCIIEncoding.ASCII.GetString(RC2Decryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch { }
            return output;
        }
        private string RSA_Encrypt(string input)
        {

            var CspP = new CspParameters() { Flags = CspProviderFlags.UseMachineKeyStore, KeyContainerName = "~ZCS{a}-csE(;(Eyq" };
            var RSACryptoService = new RSACryptoServiceProvider(CspP);
            byte[] buffer = UTF8Encoding.UTF8.GetBytes(input);
            byte[] encrypted = RSACryptoService.Encrypt(buffer, true);
            return Convert.ToBase64String(encrypted);
        }
        private string RSA_Decrypt(string input)
        {

            var CspP = new CspParameters() { Flags = CspProviderFlags.UseMachineKeyStore, KeyContainerName = "~ZCS{a}-csE(;(Eyq" };
            var RSACryptoService = new RSACryptoServiceProvider(CspP);
            byte[] buffer = Convert.FromBase64String(input);
            byte[] decrypted = RSACryptoService.Decrypt(buffer, true);
            return UTF8Encoding.UTF8.GetString(decrypted);
        }
    }
}
