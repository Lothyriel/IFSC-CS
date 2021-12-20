using Newtonsoft.Json;

namespace Domain.Business
{
    public abstract class Data
    {
        public static NotStarted NotStarted { get; } = new NotStarted();

        public static Data JsonDeserialize(string json) 
        {
            try
            {
                return JsonConvert.DeserializeObject<Bid>(json) ?? throw new NullReferenceException("Unable to deserialize");
            }
            catch (Exception)
            {
                return JsonConvert.DeserializeObject<Auction>(json) ?? throw new NullReferenceException("Unable to deserialize");
            }
        }
    }
    public class NotStarted : Data
    {
        public static string Information => "Auction Not Started Yet ...";
    }
}
