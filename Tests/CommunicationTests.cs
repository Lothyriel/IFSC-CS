using Domain;
using Domain.Business;
using Domain.Business.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
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
            var clientInfo = Client.AuctionConnection?.Receive();

            auction.CurrentBid.Value.Should().Be(clientInfo?.Value);
            auction.CurrentBid.Buyer.Name.Should().Be(clientInfo?.Buyer.Name);
            auction.CurrentBid.Buyer.PublicKey.Should().Be(clientInfo?.Buyer.PublicKey);
        }
        [Test]
        public void ShouldUpdateBidReceivingKey()
        {
            //arrange 
            var auction = new Auction("TV 50 Polegadas", 999.99, 10, 5);

            //act
            var connectionData = auction.Connection.Data;
            Client.JoinAuctionConnection(connectionData);

            //assert
            var newBid = new Bid(Client.BuyerData, 1050, false);
            Client.Send(newBid);

            Thread.Sleep(1000);

            auction.CurrentBid.Value.Should().Be(newBid.Value);
            auction.CurrentBid.Buyer.Name.Should().Be(newBid.Buyer.Name);
            auction.CurrentBid.Buyer.PublicKey.Should().Be(newBid.Buyer.PublicKey);
        }
        [Test]
        public async Task ShouldUpdateBidThroughAPI()
        {
            //arrange 
            Controller.CreateAuction("TV 50 Polegadas", 999.99, 10, 5);
            var auction = Auction.CurrentAuction;

            //act
            await Client.RequestJoin();

            //assert
            var newBid = new Bid(Client.BuyerData, 1050, false);
            Client.Send(newBid);

            await Task.Delay(1000);

            auction?.CurrentBid.Value.Should().Be(newBid.Value);
            auction?.CurrentBid.Buyer.Name.Should().Be(newBid.Buyer.Name);
            auction?.CurrentBid.Buyer.PublicKey.Should().Be(newBid.Buyer.PublicKey);
        }
    }
}