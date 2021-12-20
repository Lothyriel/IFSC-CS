using Domain.Business;
using Domain.Business.Exceptions;
using Domain.Criptography;
using System.Net;
using System.Net.Sockets;

namespace Domain
{
    public class Client
    {
        public string Name { get; }
        public Socket? Socket { get; set; } = null;
        private Asymmetric Asymmetric { get; } = new Asymmetric();
        private Symmetric? Symmetric { get; set; } = null;

        private static HttpClient HttpClient { get; } = new();

        public Client(string name)
        {
            Name = name;
        }

        public async void Join()
        {
            var values = new Dictionary<string, string>
            {
                { "name", "Name" },
                { "publicKey", Asymmetric.SerializePublicKey() }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await HttpClient.PostAsync("http://localhost:5212", content);

            TreatResponse(await response.Content.ReadAsStringAsync());
        }
        public string Bid(Bid newBid)
        {
            throw new NotImplementedException();
        }

        public Data TreatData(byte[] data)
        {
            return Symmetric?.Decrypt(data) ?? throw new NotConnected();
        }

        private void TreatResponse(string responseData)
        {
            if (responseData.Contains("Auction"))
                throw new AuctionNotStarted();

            var connectionData = Asymmetric.Decrypt(responseData);

            Symmetric = connectionData.Symmetric;
            ConnectToMulticastGroup(connectionData.MultiCastAddress);
        }

        private void ConnectToMulticastGroup(string multiCastAddress)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse(multiCastAddress)));
            Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);
        }
    }
}