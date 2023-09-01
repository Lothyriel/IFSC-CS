namespace Domain.Business.Exceptions
{
    public class InvalidData : Exception
    {
        public InvalidData(string? message) : base(message)
        {
        }
    }
    public class AuctionNotStarted : Exception
    {
    }
    public class NotConnected : Exception
    {
        public NotConnected() : base("You don't have a connection to the multicast group")
        {

        }
    }
}
