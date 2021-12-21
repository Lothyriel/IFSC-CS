using Domain;
using Domain.Business;
using Domain.Business.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace Tests
{
    public class CommunicationTests
    {
        private Client Client { get; set; } = new("João Xavier");
        AuctionController Controller { get; set; } = new();

        [Test]
        public void ShouldThrowAuctionNotStarted()
        {
            //act
            var act = async () => await Client.RequestJoin();       //nao ta funfando

            //assert
            act.Should().ThrowAsync<AuctionNotStarted>();
        }
        [Test]
        public void ShouldJoinMultiCastGroup()
        {
            //arrange 
            var auction = new Auction("TV 50 Polegadas", 999.99, 10, 5);

            //act
            var connectionData = auction.MultiCastGroup.ConnectionData;
            Client.ConnectToMulticastGroup(connectionData.MultiCastAddress, connectionData.Port);
        }
        [Test]
        public void ShouldGetAuctionData()
        {
            //arrange 
            var auction = new Auction("TV 50 Polegadas", 999.99, 10, 5);

            //act
            var connectionData = auction.MultiCastGroup.ConnectionData;
            Client.SymmetricKey = connectionData.SymmetricKey;
            Client.ConnectToMulticastGroup(connectionData.MultiCastAddress, connectionData.Port);

            //assert
            var clientInfo = Client.GetCurrentBid();

            auction.CurrentBid.Value.Should().Be(clientInfo.Value);
            auction.CurrentBid.Buyer.Name.Should().Be(clientInfo.Buyer.Name);
            auction.CurrentBid.Buyer.PublicKey.Should().Be(clientInfo.Buyer.PublicKey);
        }
        [Test]
        public void ShouldUpdateBid()
        {
            //arrange 
            var auction = new Auction("TV 50 Polegadas", 999.99, 10, 5);

            //act
            var connectionData = auction.MultiCastGroup.ConnectionData;
            Client.SymmetricKey = connectionData.SymmetricKey;
            Client.ConnectToMulticastGroup(connectionData.MultiCastAddress, connectionData.Port);

            //assert
            var newBid = new Bid(Client.BuyerData, 1050);
            Client.MakeBid(newBid);

            auction.CurrentBid.Value.Should().Be(newBid.Value);
            auction.CurrentBid.Buyer.Name.Should().Be(newBid.Buyer.Name);
            auction.CurrentBid.Buyer.PublicKey.Should().Be(newBid.Buyer.PublicKey);
        }
    }
}
