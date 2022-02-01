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
            CurrentBid = new (new ("Starting Value", "None"), StartingValue, produtctDescription, minBid, DueTime, true);
            Connection = new (Data);
        }

        public string ProdutctDescription { get; }
        public double StartingValue { get; }
        public double MinBid { get; }
        public DateTime DueTime { get; }
        public Bid CurrentBid { get; set; }
        public AuctionConnection Connection { get; }
        public ConnectionData Data { get; } = new("224.168.55.25", 50000, new());

        private List<Bid> Bids { get; } = new();
        public static Auction? CurrentAuction { get; set; } = null;


        public string GetConnectionData(string name, string publicKey)
        {
            return AsymmetricKey.Encrypt(publicKey, Connection.Data);
        }
        public void Start()
        {
            CurrentAuction = this;
            BeginReceivingLoop();
            BeginBroadcast();
        }
        private void BeginBroadcast()
        {
            Task.Run(BroadcastLoop);

            void BroadcastLoop()
            {
                while (true)
                {
                    Console.WriteLine($"Current Bid is: {CurrentBid}");
                    Connection.Send(CurrentBid);
                    Thread.Sleep(3000);
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
                    if (!CurrentBid.IsValid(bid.Value) || bid.IsFromServer || bid.AuctionExpired())
                        continue;

                    Bids.Add(bid);
                    CurrentBid = bid;
                    CurrentBid.IsFromServer = true;
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