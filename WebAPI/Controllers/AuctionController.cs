using Domain.Business;
using Domain.Business.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    public class AuctionController : ControllerBase
    {
        public static string? BaseUrl { get; set; } = null;

        public static Auction? CurrentAuction { get; set; } = null;
        [HttpPost]
        [Route("join")]
        public string GetConnectionData(ClientData data)
        {
            return CurrentAuction?.GetConnectionData(data.Name, data.PublicKey) ?? throw new AuctionNotStarted();
        }
        [HttpPost()]
        [Route("start")]
        public void CreateAuction(AuctionData data)
        {
            CurrentAuction = new Auction(data.ProdutctDescription, data.StartingValue, data.MinBid, data.MinutesDueTime);
        }
        public record AuctionData(string ProdutctDescription, double StartingValue, double MinBid, double MinutesDueTime)
        {
        }
        public record ClientData(string Name, string PublicKey)
        {
        }
    }
}