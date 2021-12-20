namespace Domain.Business
{
    public class Bid : Data
    {
        public Bid(Buyer buyer, double value)
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
        public Buyer Buyer { get; }
        public double Value { get; }
    }
}