using Domain.Business;
using Domain.Criptography;
using Microsoft.AspNetCore.Http;
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
            IPEP = new IPEndPoint(ipAddress, port);

            ConnectionData = new ConnectionData(IPEP.Address.ToString(), IPEP.Port, SymmetricKey);
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ipAddress));
            Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);

            Socket.Connect(IPEP);
        }
        public void Notify(Bid newBid)
        {
            var bytes = SymmetricKey.Encrypt(newBid);
            Socket.Send(bytes);
        }
        public Bid ReceiveBid() 
        {
            var buffer = new byte[4096];
            var bytes = Socket.Receive(buffer);

            var data = SymmetricKey.Decrypt(buffer);

            return (Bid)data;
        }
    }
}