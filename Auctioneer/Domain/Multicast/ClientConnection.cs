using Domain.Business;
using Domain.Business.Exceptions;
using Domain.Criptography;
using Newtonsoft.Json;
using System.Text;

namespace Domain
{
    public class ClientConnection
    {
        public BuyerData BuyerData { get; }

        private AsymmetricKey AsymmetricKey { get; } = new();

        public AuctionConnection? AuctionConnection { get; set; } = null;

        private static HttpClient HttpClient { get; } = new();
        public bool AuctionEnded { get; set; }

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

            var data = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync("http://localhost:5009/join", data);

            TreatResponse(await response.Content.ReadAsStringAsync());
        }
        public void TreatResponse(string responseData)
        {
            if (responseData.Contains("Not Started"))
                throw new AuctionNotStarted();

            var connectionData = AsymmetricKey.Decrypt(responseData);
            JoinAuctionConnection(connectionData);
        }

        public void JoinAuctionConnection(ConnectionData connectionData)
        {
            AuctionConnection = new(connectionData);
        }

        public void Bid(Bid newBid)
        {
            AuctionConnection?.Send(newBid);
        }
        public async Task BeginReceiveLoop(Action<Bid> handler)
        {
            await Task.Run(ReceiveLoop);

            void ReceiveLoop()
            {
                while (!AuctionEnded) 
                {
                    handler(AuctionConnection!.Receive());
                    //Thread.Sleep(3000);
                }
            }
        }
    }
}