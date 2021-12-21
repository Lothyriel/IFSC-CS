using Domain.Criptography;
using Microsoft.AspNetCore.Http;

namespace Domain.Business
{
    public class Auction : Data
    {
        public Auction(string produtctDescription, double startingValue, double minBid, double dueTime)
        {
            ProdutctDescription = produtctDescription;
            StartingValue = startingValue;
            MinBid = minBid;
            DueTime = DateTime.Now.Add(TimeSpan.FromMinutes(dueTime));
            CurrentBid = new Bid(new BuyerData("Starting Value", "None"), StartingValue);
            Task.Run(BeginBroadcast);
            Task.Run(BeginReceivingLoop);
        }

        public string ProdutctDescription { get; }
        public double StartingValue { get; }
        public double MinBid { get; }
        public DateTime DueTime { get; }
        public Bid CurrentBid { get; set; }

        private List<Bid> Bids { get; } = new();
        private Dictionary<string, BuyerData> Buyers { get; } = new();
        public MultiCastGroup MultiCastGroup { get; } = new("224.9.9.9", 4960);

        public string GetConnectionData(string name, string publicKey)
        {
            var encryptedData = Asymmetric.Encrypt(publicKey, MultiCastGroup.ConnectionData); ;
            Buyers.Add(publicKey, new BuyerData(name, publicKey));

            return encryptedData;
        }

        private void BeginBroadcast()
        {
            while (true)
            {
                Console.WriteLine($"Current Bid is: {CurrentBid}");
                MultiCastGroup.Notify(CurrentBid);
                Thread.Sleep(500);
            }
        }
        private void BeginReceivingLoop()
        {
            while (true)
            {
                var bid = MultiCastGroup.ReceiveBid();
                if (!bid.IsValid(this))
                    continue;

                CurrentBid = bid;
                MultiCastGroup.Notify(CurrentBid);
                Thread.Sleep(500);
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