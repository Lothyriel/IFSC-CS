using Domain;
using Domain.Business;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPI.Controllers;
using Xunit;

namespace Tests
{
    public class APITests
    {
        private HttpClient HttpClient { get; }
        private Client Client { get; } = new("Juca Jr");
        private Client Client2 { get; } = new("Juca Senior");

        public APITests()
        {
            var factory = new WebApplicationFactory<AuctionController>();
            HttpClient = factory.CreateClient();
        }
        [Fact]
        public async Task ShouldUpdateBidThroughAPI()
        {
            //act
            AuctionController.CurrentAuction = new Auction("descricao", 100, 10, 5);
            Client.HttpClient = HttpClient;

            await Client.PostJoin("/join");
            var firstBid = new Bid(Client.BuyerData, 1050);
            Client.Send(firstBid);

            await Client2.PostJoin("/join");
            var secondBid = new Bid(Client2.BuyerData, 2000);
            Client2.Send(secondBid);

            await Task.Delay(1000);

            //assert
            Client.AuctionConnection.Should().NotBeNull();
            Client2.AuctionConnection.Should().NotBeNull();

            Client.CurrentBid.Should().NotBeNull();
            Client2.CurrentBid.Should().NotBeNull();

            Client2.CurrentBid?.Value.Should().Be(Client.CurrentBid?.Value);
            Client2.CurrentBid?.Buyer.Name.Should().Be(Client.CurrentBid?.Buyer.Name);
            Client2.CurrentBid?.Buyer.PublicKey.Should().Be(Client.CurrentBid?.Buyer.PublicKey);

            AuctionController.CurrentAuction.CurrentBid?.Value.Should().Be(secondBid.Value);
            AuctionController.CurrentAuction.CurrentBid?.Buyer.Name.Should().Be(secondBid.Buyer.Name);
            AuctionController.CurrentAuction.CurrentBid?.Buyer.PublicKey.Should().Be(secondBid.Buyer.PublicKey);
        }
    }
}
