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
            CurrentBid = new Bid(new Buyer("Starting Value", "None"), StartingValue);
        }

        public string ProdutctDescription { get; }
        public double StartingValue { get; }
        public double MinBid { get; }
        public DateTime DueTime { get; }
        public Bid CurrentBid { get; set; }

        private List<Bid> Bids { get; } = new();
        private Dictionary<string, Buyer> Buyers { get; } = new();
        private MultiCastGroup Group { get; } = new("224.5.6.7");

        public string Join(string name, string publicKey)
        {
            var data = Group.Join(publicKey);
            Buyers.Add(publicKey, new Buyer(name, publicKey));
            return data;
        }
        public string MakeBid(Bid newBid)
        {
            if (!newBid.IsValid(this))
                return "Bid failed, the new bid needs to cover the current one...";

            Bids.Add(newBid);
            Group.Notify(newBid);
            return "Bid Successfully placed!";
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