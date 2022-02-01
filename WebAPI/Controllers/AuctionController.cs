using Domain.Business;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [ApiController]
    public class AuctionController : ControllerBase
    {
        [HttpPost]
        [Route("join")]
        public string GetConnectionData(ConnectionDataViewModel cd)
        {
            try
            {
                return Auction.CurrentAuction?.GetConnectionData(cd.Name, cd.PublicKey) ?? "Auction Not Started!";
            }
            catch (JsonReaderException)
            {
                return "Invalid public key";
            }
        }
        [HttpPost()]
        [Route("start")]
        public string CreateAuction(AuctionStartViewModel auction)
        {
            if (Auction.CurrentAuction != null)
                return "Auction is already running";

            new Auction(auction.ProdutctDescription, auction.StartingValue, auction.MinBid, auction.MinutesDueTime).Start();
            return "Auction Started!";
        }
    }
}