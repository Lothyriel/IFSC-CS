using Domain.Business;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    public class AuctionController : ControllerBase
    {
        [HttpPost]
        [Route("join")]
        public string GetConnectionData(string name, string publicKey)
        {
            return Auction.CurrentAuction?.GetConnectionData(name, publicKey) ?? NotStarted.Information;
        }
        [HttpPost()]
        [Route("start")]
        public void CreateAuction(string produtctDescription, double startingValue, double minBid, double minutesDueTime)
        {
            Auction.CurrentAuction = new Auction(produtctDescription, startingValue, minBid, minutesDueTime);
        }
    }
}