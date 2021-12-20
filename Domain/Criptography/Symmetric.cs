using Domain.Business;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Criptography
{
    public class Symmetric
    {
        public Symmetric()
        {
            Key = Encoding.Unicode.GetString(Aes.Key);
            IV = Encoding.Unicode.GetString(Aes.IV);
        }
        [JsonConstructor]
        public Symmetric(string key, string iv)
        {
            Aes.Key = Encoding.Unicode.GetBytes(key);
            Aes.IV = Encoding.Unicode.GetBytes(iv);
        }
        [JsonIgnore]
        public Aes Aes { get; } = Aes.Create();
        public string? Key { get; }
        public string? IV { get; }

        public byte[] Encrypt(Data data)
        {
            var encryptor = Aes.CreateEncryptor(Aes.Key, Aes.IV);

            var json = JsonConvert.SerializeObject(data);
            var bytes = Encoding.Unicode.GetBytes(json);

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
        }
        public Data Decrypt(byte[] encryptedBytes)
        {
            var decryptor = Aes.CreateDecryptor(Aes.Key, Aes.IV);

            using var memoryStream = new MemoryStream(encryptedBytes);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(cryptoStream, Encoding.Unicode);
            var decryptedBytes = Encoding.Unicode.GetBytes(srDecrypt.ReadToEnd());
            var json = Encoding.Unicode.GetString(decryptedBytes);
            return Data.JsonDeserialize(json);
        }
    }
}