using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Printlaser.Infra.Crosscutting.Seguranca
{
    public class Criptografia
    {
        private const string __SENHA_ENCRYPT_CMP = "cam!2_@()8878*&^%";
        private const string __SENHA_ENCRYPT = "c1Mj@!2P{t-it$G}Yb&;=lf2Kfao=^;y";
        public static string Criptografar(string conteudo)
        {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(conteudo);
            System.Security.Cryptography.PasswordDeriveBytes pdb = new System.Security.Cryptography.PasswordDeriveBytes(__SENHA_ENCRYPT_CMP,
                new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return Convert.ToBase64String(encryptedData);
        }
        public static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Security.Cryptography.Rijndael alg = System.Security.Cryptography.Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, alg.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write);
            cs.Write(clearData, 0, clearData.Length);
            cs.Close();
            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }
        public static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Security.Cryptography.Rijndael alg = System.Security.Cryptography.Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, alg.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);
            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            return decryptedData;

        }
        public static string Descriptografar(string conteudo)
        {
            byte[] cipherBytes = Convert.FromBase64String(conteudo);
            System.Security.Cryptography.PasswordDeriveBytes pdb = new System.Security.Cryptography.PasswordDeriveBytes(__SENHA_ENCRYPT_CMP,
                new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }

        /// <summary>
        /// Método para criptografar os dados da querystring
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns></returns>
        public static string EncryptQueryString(string param)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(param);

            var pdb = new Rfc2898DeriveBytes(__SENHA_ENCRYPT, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            var result = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            param = Convert.ToBase64String(result.ToArray());

            return param;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string DecryptQueryString(string param)
        {
            param = param.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(param);
            var pdb = new Rfc2898DeriveBytes(__SENHA_ENCRYPT, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            var result = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            Encoding.Unicode.GetString(result.ToArray());

            return Encoding.Unicode.GetString(result.ToArray());
        }

        public static string CriarChave()
        {
            RandomNumberGenerator cryptoRandomDataGenerator = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[32];
            cryptoRandomDataGenerator.GetBytes(buffer);
            string ubiq = Convert.ToBase64String(buffer);

            return ubiq.Replace("=", "");
        }
    }
}
