using Domain.Criptography;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    public class AsymmetricTests
    {
        private AsymmetricKey AsymmetricKeys { get; } = new();

        [Test]
        public void ShouldLoadSameKey()
        {
            //act         
            var strPublicKey = AsymmetricKeys.SerializePublicKey();
            var publicKey = AsymmetricKey.DesserializePublicKey(strPublicKey);

            //assert
            publicKey.Modulus.Should().Equal(AsymmetricKeys.PublicKey.Modulus);
            publicKey.Exponent.Should().Equal(AsymmetricKeys.PublicKey.Exponent);
        }
        [Test]
        public void ShouldEncryptAndDecryptData()
        {
            //arrange
            var strPublicKey = AsymmetricKeys.SerializePublicKey();
            var connectionData = new ConnectionData("IP MULTICAST", 42069, new SymmetricKey());

            //act
            var encryptedConnectionData = AsymmetricKey.Encrypt(strPublicKey, connectionData);
            var decryptedConnectionData = AsymmetricKeys.Decrypt(encryptedConnectionData);

            //assert            
            decryptedConnectionData.SymmetricKey.Aes.Key.Should().Equal(connectionData.SymmetricKey.Aes.Key);
            decryptedConnectionData.SymmetricKey.Aes.IV.Should().Equal(connectionData.SymmetricKey.Aes.IV);
            decryptedConnectionData.MultiCastAddress.Should().Be(connectionData.MultiCastAddress);
            decryptedConnectionData.Port.Should().Be(connectionData.Port);
        }
    }
}