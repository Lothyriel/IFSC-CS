using Domain.Criptography;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    public class AsymmetricTests
    {
        private Asymmetric AsymmetricKeys { get; } = new();

        [Test]
        public void ShouldLoadSameKey()
        {
            //act         
            var strPublicKey = AsymmetricKeys.SerializePublicKey();
            var publicKey = Asymmetric.DesserializePublicKey(strPublicKey);

            //assert
            publicKey.Modulus.Should().Equal(AsymmetricKeys.PublicKey.Modulus);
            publicKey.Exponent.Should().Equal(AsymmetricKeys.PublicKey.Exponent);
        }
        [Test]
        public void ShouldEncryptAndDecryptData()
        {
            //arrange
            var strPublicKey = AsymmetricKeys.SerializePublicKey();
            var connectionData = new ConnectionData("IP MULTICAST", 42069, new Symmetric());

            //act
            var encryptedConnectionData = Asymmetric.Encrypt(strPublicKey, connectionData);
            var decryptedConnectionData = AsymmetricKeys.Decrypt(encryptedConnectionData);

            //assert            
            decryptedConnectionData.SymmetricKey.Aes.Key.Should().Equal(connectionData.SymmetricKey.Aes.Key);
            decryptedConnectionData.SymmetricKey.Aes.IV.Should().Equal(connectionData.SymmetricKey.Aes.IV);
            decryptedConnectionData.MultiCastAddress.Should().Be(connectionData.MultiCastAddress);
            decryptedConnectionData.Port.Should().Be(connectionData.Port);
        }
    }
}