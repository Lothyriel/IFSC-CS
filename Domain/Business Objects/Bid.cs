namespace Domain.Business
{
    public class Bid
    {
        public Bid(BuyerData buyer, double value, bool isFromServer = true)
        {
            Buyer = buyer;
            Value = value;
            IsFromServer = isFromServer;
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

        public bool IsFromServer { get; }

        public const string InvalidMessage = "Bid failed, the new bid needs to cover the current one...";
        public const string ValidMessage = "Bid Successfully placed!";
    }
}