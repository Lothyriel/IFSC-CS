namespace Domain.Business
{
    public class Bid
    {
        public Bid(BuyerData buyer, double value, string productName, double minBid, DateTime dueTime, bool isFromServer = false)
        {
            Buyer = buyer;
            Value = value;
            ProductName = productName;
            MinBid = minBid;
            DueTime = dueTime;
            IsFromServer = isFromServer;
        }

        public BuyerData Buyer { get; }
        public double Value { get; }
        public string ProductName { get; }
        public double MinBid { get; }
        public DateTime DueTime { get; }
        public bool IsFromServer { get; set; }

        public const string InvalidMessage = "Bid failed, the new bid needs to cover the current one...";
        public const string ValidMessage = "Bid Successfully placed!";

        public bool IsValid(double newValue)
        {
            return newValue > Value + MinBid;
        }
        public override string ToString()
        {
            return $"Buyer: {Buyer} | Value: {Value}";
        }

        public bool AuctionExpired()
        {
            return DueTime < DateTime.Now;
        }
    }
}