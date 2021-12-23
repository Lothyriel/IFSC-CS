using Domain.Business_Objects;
using Domain.Criptography;

namespace Domain.Business
{
    public class Auction
    {
        public Auction(string produtctDescription, double startingValue, double minBid, double dueTime)
        {
            ProdutctDescription = produtctDescription;
            StartingValue = startingValue;
            MinBid = minBid;
            DueTime = DateTime.Now.Add(TimeSpan.FromMinutes(dueTime));
            CurrentBid = new Bid(new BuyerData("Starting Value", "None"), StartingValue);
            Connection = new(Data);

            BeginReceivingLoop();
            BeginBroadcast();
        }

        public string ProdutctDescription { get; }
        public double StartingValue { get; }
        public double MinBid { get; }
        public DateTime DueTime { get; }
        public Bid CurrentBid { get; set; }
        public AuctionConnection Connection { get; }

        public ConnectionData Data = new("224.168.55.25", 50000, new());

        private List<Bid> Bids { get; } = new();
        private Dictionary<string, BuyerData> Buyers { get; } = new();
        public static Auction? CurrentAuction { get; set; } = null;

        public string GetConnectionData(string name, string publicKey)
        {
            var encryptedData = AsymmetricKey.Encrypt(publicKey, Connection.Data);
            Buyers.Add(publicKey, new BuyerData(name, publicKey));

            return encryptedData;
        }

        private void BeginBroadcast()
        {
            Task.Run(Broadcast);

            void Broadcast()
            {
                while (true)
                {
                    Console.WriteLine($"Current Bid is: {CurrentBid}");
                    Connection.Send(CurrentBid);
                    Thread.Sleep(500);
                }
            }
        }
        private void BeginReceivingLoop()
        {
            Task.Run(ReceiveLoop);

            void ReceiveLoop()
            {
                while (true)
                {
                    var bid = Connection.Receive();
                    if (!bid.IsValid(this) && bid.IsFromServer)
                        continue;

                    Bids.Add(bid);
                    CurrentBid = bid;
                }
            }
        }

        public override string ToString()
        {
            return @$"
                    Auction Details:
                        Product: {ProdutctDescription}
                        Current Bid: {CurrentBid}
                        Due Time: {DueTime:G}
                        Starting Value: {StartingValue}
                        Mininum Bid Value: {MinBid}";
        }
    }
}