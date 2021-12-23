using Domain.Business;
using Domain.Business.Exceptions;
using Domain.Business_Objects;
using Domain.Criptography;

namespace Domain
{
    public class ClientConnection
    {
        public BuyerData BuyerData { get; }

        private AsymmetricKey AsymmetricKey { get; } = new();

        public AuctionConnection? AuctionConnection { get; set; } = null;

        private static HttpClient HttpClient { get; } = new();

        public ClientConnection(string name)
        {
            BuyerData = new(name, AsymmetricKey.SerializePublicKey());
        }

        public async Task RequestJoin()
        {
            var values = new Dictionary<string, string>
            {
                { "name", BuyerData.Name },
                { "publicKey",BuyerData.PublicKey  }
            };
            var data = new FormUrlEncodedContent(values);
            var response = await HttpClient.PostAsync("http://localhost:5009/join", data);

            TreatResponse(await response.Content.ReadAsStringAsync());
        }
        private void TreatResponse(string responseData)
        {
            if (responseData.Contains("Auction"))
                throw new AuctionNotStarted();

            var connectionData = AsymmetricKey.Decrypt(responseData);
            JoinAuctionConnection(connectionData);
        }

        public void JoinAuctionConnection(ConnectionData connectionData)
        {
            AuctionConnection = new(connectionData);
            BeginReceiveLoop();
        }

        public void Send(Bid newBid)
        {
            AuctionConnection?.Send(newBid);
        }
        public void BeginReceiveLoop()
        {
            Task.Run(() => AuctionConnection?.Receive());
        }
    }
}