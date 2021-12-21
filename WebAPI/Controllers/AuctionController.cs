using Domain.Business;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    public class AuctionController : ControllerBase
    {
        public Auction? CurrentAuction = null;

        [HttpPost]
        [Route("join")]
        public string GetConnectionData(string name, string publicKey)
        {
            return CurrentAuction?.GetConnectionData(name, publicKey) ?? NotStarted.Information;
        }
        [HttpPost()]
        [Route("start")]
        public void CreateAuction(string produtctDescription, double startingValue, double minBid, double minutesDueTime)
        {
            CurrentAuction = new Auction(produtctDescription, startingValue, minBid, minutesDueTime);
        }
    }
}