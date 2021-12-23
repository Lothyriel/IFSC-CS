using Domain.Business;
using Domain.Business.Exceptions;
using Domain.Criptography;
using Newtonsoft.Json;
using System.Text;

namespace Domain
{
    public class Client
    {
        public BuyerData BuyerData { get; }

        private AsymmetricKey AsymmetricKey { get; } = new();

        public AuctionConnection? AuctionConnection { get; set; } = null;

        public static HttpClient HttpClient { get; set; } = new();
        public Bid? CurrentBid { get; set; } = null;

        public Client(string name)
        {
            BuyerData = new(name, AsymmetricKey.SerializePublicKey());
        }

        public async Task RequestJoin()
        {
            await PostJoin("http://localhost:5009/join");
        }

        public async Task PostJoin(string uri)
        {
            var values = new Dictionary<string, string>
            {
                { "name", BuyerData.Name },
                { "publicKey",BuyerData.PublicKey  }
            };
            var data = new StringContent(JsonConvert.SerializeObject(values), Encoding.Unicode, "application/json");
            var response = await HttpClient.PostAsync(uri, data);
            TreatResponse(await response.Content.ReadAsStringAsync());
        }

        public void TreatResponse(string responseData)
        {
            if (responseData.Contains("Auction"))
                throw new AuctionNotStarted();
            if (responseData.Contains("validation"))
                throw new InvalidData("Parameters publicKey and name are mandatory!");
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
            Task.Run(ReceiveLoop);

            void ReceiveLoop()
            {
                while (true)
                {
                    var bid = AuctionConnection?.Receive() ?? throw new NotConnected();

                    if (bid.IsFromServer)
                        CurrentBid = bid;
                }
            }
        }
    }
}