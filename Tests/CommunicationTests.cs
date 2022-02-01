using Domain;
using Domain.Business;
using Domain.Business.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using System.Threading;
using WebAPI.Controllers;

namespace Tests
{
    public class CommunicationTests
    {
        private ClientConnection Client { get; set; } = new("João Xavier");
        private AuctionController Controller { get; set; } = new();

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
            var connectionData = auction.Connection.Data;
            Client.JoinAuctionConnection(connectionData);
        }
        [Test]
        public void ShouldGetAuctionData()
        {
            //arrange 
            var auction = new Auction("TV 50 Polegadas", 999.99, 10, 5);

            //act
            var connectionData = auction.Connection.Data;
            Client.JoinAuctionConnection(connectionData);

            //assert
            auction.Start();
            Thread.Sleep(1000);
            var clientInfo = Client.AuctionConnection?.Receive();

            auction.CurrentBid.Value.Should().Be(clientInfo?.Value);
            auction.CurrentBid.Buyer.Name.Should().Be(clientInfo?.Buyer.Name);
            auction.CurrentBid.Buyer.PublicKey.Should().Be(clientInfo?.Buyer.PublicKey);
        }
        [Test]
        public void ShouldUpdateBidReceivingKey()
        {
            var productDescripton = "TV 50 Inches";
            //arrange 
            var auction = new Auction(productDescripton, 999.99, 10, 5);
            auction.Start();

            //act
            var connectionData = auction.Connection.Data;
            Client.JoinAuctionConnection(connectionData);

            //assert
            var newBid = new Bid(Client.BuyerData, 1050, productDescripton, auction.MinBid, auction.DueTime);
            Client.Bid(newBid);

            Thread.Sleep(3000);

            auction.CurrentBid.Value.Should().Be(newBid.Value);
            auction.CurrentBid.Buyer.Name.Should().Be(newBid.Buyer.Name);
            auction.CurrentBid.Buyer.PublicKey.Should().Be(newBid.Buyer.PublicKey);
        }
        [Test]
        public void ShouldUpdateBidThroughAPIController()
        {
            //arrange 
            var productDescription = "TV 50 Inches";
            var auction = new Auction(productDescription, 999.99, 10, 5);
            auction.Start();

            //act
            var connectionData = Controller.GetConnectionData(new (Client.BuyerData.Name, Client.BuyerData.PublicKey));
            Client.TreatResponse(connectionData);

            //assert
            var newBid = new Bid(Client.BuyerData, 1050, productDescription, auction.MinBid, auction.DueTime);
            Client.Bid(newBid);

            Thread.Sleep(5000);

            auction?.CurrentBid.Value.Should().Be(newBid.Value);
            auction?.CurrentBid.Buyer.Name.Should().Be(newBid.Buyer.Name);
            auction?.CurrentBid.Buyer.PublicKey.Should().Be(newBid.Buyer.PublicKey);
        }
    }
}