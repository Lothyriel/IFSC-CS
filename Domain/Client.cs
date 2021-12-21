using Domain.Business;
using Domain.Business.Exceptions;
using Domain.Criptography;
using System.Net;
using System.Net.Sockets;

namespace Domain
{
    public class Client
    {
        public BuyerData BuyerData { get; }

        public UdpClient UDPClient { get; } = new UdpClient();
        private Asymmetric AsymmetricKey { get; } = new Asymmetric();
        public Symmetric? SymmetricKey { get; set; } = null;

        private static HttpClient HttpClient { get; } = new();
        public Auction Auction => UpdateAuctionData();

        public Client(string name)
        {
            BuyerData = new BuyerData(name, AsymmetricKey.SerializePublicKey());
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

        public void MakeBid(Bid newBid)
        {
            var encryptedBytes = SymmetricKey.Encrypt(newBid);
            UDPClient.Send(encryptedBytes);
        }

        public Bid GetCurrentBid()
        {
            return Auction.CurrentBid;
        }

        private Auction UpdateAuctionData()
        {
            var ipep = new IPEndPoint(IPAddress.Any, 0);
            var encryptedBytes = UDPClient.Receive(ref ipep);

            var data = SymmetricKey?.Decrypt(encryptedBytes) ?? throw new NotConnected();
            return (Auction)data;
        }

        private void TreatResponse(string responseData)
        {
            if (responseData.Contains("Auction"))
                throw new AuctionNotStarted();

            var connectionData = AsymmetricKey.Decrypt(responseData);
            SymmetricKey = connectionData.SymmetricKey;
            ConnectToMulticastGroup(connectionData.MultiCastAddress, connectionData.Port);
        }

        public void ConnectToMulticastGroup(string address, int port)
        {
            UDPClient.Connect(address, port);
            UDPClient.JoinMulticastGroup(IPAddress.Parse(address), 2);
        }
    }
}