using Domain.Business;
using Domain.Criptography;
using System.Net;
using System.Net.Sockets;

namespace Domain
{
    public class MultiCastGroup
    {
        public IPAddress IPAddress { get; }
        public Socket Socket { get; }
        public Symmetric SymmetricKey { get; } = new Symmetric();
        public ConnectionData AuctionData { get; }

        public MultiCastGroup(string ipAddress)
        {
            IPAddress = IPAddress.Parse(ipAddress);

            AuctionData = new ConnectionData(IPAddress.ToString(), SymmetricKey);
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress));
            Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);

            var ipep = new IPEndPoint(IPAddress, 4567);
            Socket.Connect(ipep);
        }

        public string Join(string publicKey)
        {
            var encryptedData = Asymmetric.Encrypt(publicKey, AuctionData);

            var cliente = new UdpClient();
            cliente.JoinMulticastGroup(IPAddress);

            return encryptedData;
        }

        public void Notify(Bid newBid)
        {
            var bytes = SymmetricKey.Encrypt(newBid);
            Task.Run(() => Socket.Send(bytes));
        }
    }
}