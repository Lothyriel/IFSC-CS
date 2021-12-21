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

        public Socket Socket { get; }
        public Symmetric? SymmetricKey { get; set; } = null;
        public IPEndPoint MultiCastEP { get; private set; }
        private Asymmetric AsymmetricKey { get; } = new();

        private static HttpClient HttpClient { get; } = new();

        public Client(string name)
        {
            Socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
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
            SymmetricKey = connectionData.SymmetricKey;
            ConnectToMulticastGroup(connectionData.MultiCastAddress, connectionData.Port);
        }

        public void ConnectToMulticastGroup(string address, int port)
        {
            var ipep = new IPEndPoint(IPAddress.Any, port);
            var multiCastIP = IPAddress.Parse(address);
            MultiCastEP = new IPEndPoint(multiCastIP, port);
            Socket.Bind(ipep);
            Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multiCastIP, IPAddress.Any));
        }

        public void MakeBid(Bid newBid)
        {
            throw new Exception("udpclient???");
            var encryptedBytes = SymmetricKey.Encrypt(newBid);
            Socket.SendTo(encryptedBytes, MultiCastEP);
        }
        public Bid GetCurrentBid()
        {
            var buffer = new byte[4096];
            var qtdBytes = Socket.Receive(buffer);

            var data = SymmetricKey?.Decrypt(buffer[0..qtdBytes]) ?? throw new NotConnected();
            return (Bid)data;
        }
    }
}