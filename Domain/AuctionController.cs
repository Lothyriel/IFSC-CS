using Domain.Business;

namespace Domain
{
    public class AuctionController
    {
        public Auction? CurrentAuction = null;
        public Data CurrentStatus => CurrentAuction is null ? Data.NotStarted : CurrentAuction;
        public string MakeBid(Bid newBid)
        {
            return CurrentAuction?.MakeBid(newBid) ?? NotStarted.Information;
        }
        public string Join(string name, string publicKey)
        {
            return CurrentAuction?.Join(name, publicKey) ?? NotStarted.Information;
        }
        public void CreateAuction(string produtctDescription, double startingValue, double minBid, double minutesDueTime)
        {
            CurrentAuction = new Auction(produtctDescription, startingValue, minBid, minutesDueTime);
        }
    }
}