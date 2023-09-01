namespace WebAPI.ViewModels
{
    public record ConnectionDataViewModel(string Name, string PublicKey);

    public record AuctionStartViewModel(string ProdutctDescription, double StartingValue, double MinBid, double MinutesDueTime);
}
