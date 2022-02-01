using Domain.Business;
using Domain.Criptography;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Tests
{
    public class SymmetricTests
    {
        private SymmetricKey SymmetricKey { get; } = new();
        private SymmetricKey? SymmetricKeyCopy;

        [SetUp]
        public void Setup()
        {
            SymmetricKeyCopy = new SymmetricKey(SymmetricKey.Aes.Key.GetToString(), SymmetricKey.Aes.IV.GetToString());
        }
        [Test]
        public void ShouldLoadSameKey()
        {
            //assert            
            SymmetricKeyCopy?.Aes.Key.Should().Equal(SymmetricKey.Aes.Key);
            SymmetricKeyCopy?.Aes.IV.Should().Equal(SymmetricKey.Aes.IV);
        }
        [Test]
        public void ShouldEncryptAndDecryptData()
        {
            //arrange
            var buyer = new BuyerData("João Xavier", "");
            var bid = new Bid(buyer, 200, "TV", 20, DateTime.MaxValue);

            //act
            var encryptedBid = SymmetricKey.Encrypt(bid);
            var decryptedBid = SymmetricKeyCopy?.Decrypt(encryptedBid);

            //assert            
            decryptedBid?.Value.Should().Be(bid.Value);
            decryptedBid?.Buyer.Name.Should().Be(bid.Buyer.Name);
            decryptedBid?.Buyer.PublicKey.Should().Be(bid.Buyer.PublicKey);
        }
    }
}