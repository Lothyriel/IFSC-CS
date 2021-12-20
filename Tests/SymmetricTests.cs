using Domain.Business;
using Domain.Criptography;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    public class SymmetricTests
    {
        private Symmetric SymmetricKey { get; } = new();
        private Symmetric SymmetricKeyCopy;

        [SetUp]
        public void Setup()
        {
            SymmetricKeyCopy = new Symmetric(SymmetricKey.Aes.Key.GetToString(), SymmetricKey.Aes.IV.GetToString());
        }
        [Test]
        public void ShouldLoadSameKey()
        {
            //assert            
            SymmetricKeyCopy.Aes.Key.Should().Equal(SymmetricKey.Aes.Key);
            SymmetricKeyCopy.Aes.IV.Should().Equal(SymmetricKey.Aes.IV);
        }
        [Test]
        public void ShouldEncryptAndDecryptData()
        {
            //arrange
            var buyer = new Buyer("João Xavier", "");
            var bid = new Bid(buyer, 200);

            //act
            var encryptedBid = SymmetricKey.Encrypt(bid);
            var decryptedData = SymmetricKeyCopy.Decrypt(encryptedBid);

            //assert            
            var decryptedBid = (Bid)decryptedData;
            decryptedBid.Value.Should().Be(bid.Value);
            decryptedBid.Buyer.Name.Should().Be(bid.Buyer.Name);
            decryptedBid.Buyer.PublicKey.Should().Be(bid.Buyer.PublicKey);
        }
    }
}