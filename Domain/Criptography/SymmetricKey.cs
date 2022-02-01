using Domain.Business;
using Domain.Business.Exceptions;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Criptography
{
    public class SymmetricKey
    {
        public SymmetricKey()
        {
            Key = Convert.ToBase64String(Aes.Key);
            IV = Convert.ToBase64String(Aes.IV);
        }
        [JsonConstructor]
        public SymmetricKey(string key, string iv)
        {
            Aes.Key = Convert.FromBase64String(key);
            Aes.IV = Convert.FromBase64String(iv);
        }
        [JsonIgnore]
        public Aes Aes { get; } = Aes.Create();
        public string? Key { get; }
        public string? IV { get; }

        public byte[] Encrypt(Bid newBid)
        {
            var encryptor = Aes.CreateEncryptor(Aes.Key, Aes.IV);

            var json = JsonConvert.SerializeObject(newBid);

            var bytes = Encoding.Unicode.GetBytes(json);

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();

            return memoryStream.ToArray();
        }
        public Bid Decrypt(byte[] encryptedBytes)
        {
            var decryptor = Aes.CreateDecryptor(Aes.Key, Aes.IV);

            using var memoryStream = new MemoryStream(encryptedBytes);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(cryptoStream, Encoding.Unicode);

            var decryptedBytes = Encoding.Unicode.GetBytes(srDecrypt.ReadToEnd());

            var json = Encoding.Unicode.GetString(decryptedBytes);

            return JsonConvert.DeserializeObject<Bid>(json) ?? throw new InvalidData($"Error desserializing {json}");
        }
    }
}