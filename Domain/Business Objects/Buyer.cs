namespace Domain.Business
{
    public class Buyer
    {
        public Buyer(string name, string publicKey)
        {
            Name = name;
            PublicKey = publicKey;
        }

        public string Name { get; }
        public string PublicKey { get; }

        public override string ToString()
        {
            return $"{Name} | Public Key: {PublicKey}";
        }
    }
}