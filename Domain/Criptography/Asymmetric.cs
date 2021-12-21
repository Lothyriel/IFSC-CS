using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Criptography
{
    public class Asymmetric
    {
        public Asymmetric()
        {
            var rsa = new RSACryptoServiceProvider(2048);
            PrivateKey = rsa.ExportParameters(true);
            PublicKey = rsa.ExportParameters(false);
        }
        private RSAParameters PrivateKey { get; }
        public RSAParameters PublicKey { get; }
        public string SerializePublicKey()
        {
            return JsonConvert.SerializeObject(PublicKey);
        }
        public static string Encrypt(string publicKey, ConnectionData auctionData)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(DesserializePublicKey(publicKey));

            var json = JsonConvert.SerializeObject(auctionData);

            var bytes = Encoding.Unicode.GetBytes(json);

            var encryptedBytes = rsa.Encrypt(bytes, false);

            return Convert.ToBase64String(encryptedBytes);
        }
        public ConnectionData Decrypt(string encryptedData)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(PrivateKey);

            var encryptedBytes = Convert.FromBase64String(encryptedData);

            var bytes = rsa.Decrypt(encryptedBytes, false);
            var json = Encoding.Unicode.GetString(bytes);

            return JsonConvert.DeserializeObject<ConnectionData>(json) ?? throw new NullReferenceException("Unable to deserialize");
        }
        public static RSAParameters DesserializePublicKey(string publicKey)
        {
            return JsonConvert.DeserializeObject<RSAParameters>(publicKey);
        }
    }
}