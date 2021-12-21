namespace Domain.Business
{
    public class Bid : Data
    {
        public Bid(BuyerData buyer, double value)
        {
            Buyer = buyer;
            Value = value;
        }
        public bool IsValid(Auction auction)
        {
            return (auction.CurrentBid.Value + auction.MinBid) <= Value;
        }
        public override string ToString()
        {
            return $"Buyer: {Buyer} | Value: {Value}";
        }
        public BuyerData Buyer { get; }
        public double Value { get; }

        public const string InvalidMessage = "Bid failed, the new bid needs to cover the current one...";
        public const string ValidMessage = "Bid Successfully placed!";
    }
}