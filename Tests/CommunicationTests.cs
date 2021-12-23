using Domain;
using Domain.Business;
using Domain.Business.Exceptions;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class CommunicationTests
    {
        private Client Client { get; set; } = new("João Xavier");

        [Fact]
        public void ShouldThrowAuctionNotStarted()
        {
            //act
            var act = async () => await Client.RequestJoin();       //nao ta funfando

            //assert
            act.Should().ThrowAsync<AuctionNotStarted>();
        }
        [Fact]
        public void ShouldJoinMultiCastGroup()
        {
            //arrange 
            var auction = new Auction("TV 50 Polegadas", 999.99, 10, 5);

            //act
            var connectionData = auction.Connection.Data;
            Client.JoinAuctionConnection(connectionData);
        }
        [Fact]
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
        [Fact]
        public void ShouldUpdateCurrentBidReceivingKey()
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
        [Fact]
        public void ShouldUpdateCurrentBidOfClient2ReceivingKey()
        {
            //arrange 
            var auction = new Auction("TV 50 Polegadas", 999.99, 10, 5);
            var client2 = new Client("José Gabas Juca Jr");

            //act
            var connectionData = auction.Connection.Data;
            Client.JoinAuctionConnection(connectionData);
            client2.JoinAuctionConnection(connectionData);

            var newBid = new Bid(Client.BuyerData, 1050, false);
            Client.Send(newBid);
            Thread.Sleep(1000);

            //assert
            client2.CurrentBid?.Value.Should().Be(newBid.Value);
            client2.CurrentBid?.Buyer.Name.Should().Be(newBid.Buyer.Name);
            client2.CurrentBid?.Buyer.PublicKey.Should().Be(newBid.Buyer.PublicKey);
        }
    }
}