using Domain.Business;
using Domain.Criptography;
using System.Net;
using System.Net.Sockets;

namespace Domain
{
    public class MultiCastGroup
    {
        public Socket Socket { get; }
        public Symmetric SymmetricKey { get; } = new Symmetric();
        public ConnectionData ConnectionData { get; }
        public IPEndPoint IPEP { get; }

        public MultiCastGroup(string strIpAddress, int port)
        {
            var ipAddress = IPAddress.Parse(strIpAddress);
            IPEP = new(ipAddress, port);

            ConnectionData = new(IPEP.Address.ToString(), IPEP.Port, SymmetricKey);
            Socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ipAddress));
            Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 1);

            Socket.Connect(IPEP);
        }
        public void Broadcast(Bid newBid)
        {
            var encryptedBytes = SymmetricKey.Encrypt(newBid);
            Socket.Send(encryptedBytes);
        }
        public Bid ReceiveBid()
        {
            var buffer = new byte[4096];
            var ep = (EndPoint)IPEP;
            var qntBytes = Socket.ReceiveFrom(buffer, ref ep);

            var data = SymmetricKey.Decrypt(buffer[0..qntBytes]);

            return (Bid)data;
        }
    }
}