using Domain.Business;
using Domain.Business_Objects;
using Domain.Criptography;

namespace Domain
{
    public class AuctionConnection
    {
        public ConnectionData Data { get; }
        public UdpConnection UdpConnection { get; init; }
        public SymmetricKey SymmetricKey { get; }

        public AuctionConnection(ConnectionData data)
        {
            UdpConnection = new UdpConnection(data.MultiCastAddress, data.Port);
            SymmetricKey = data.SymmetricKey;
            Data = data;
        }

        public void Send(Bid newBid)
        {
            var encryptedBytes = SymmetricKey.Encrypt(newBid);
            UdpConnection.Send(encryptedBytes); 
        }
        public Bid Receive()
        {
            var encryptedBytes = UdpConnection.Receive();
            return SymmetricKey.Decrypt(encryptedBytes);
        }
    }
}