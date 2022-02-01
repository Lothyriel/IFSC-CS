using Domain;

namespace ClientWindowsForms
{
    public partial class BidScreen : Form
    {
        public BidScreen(Client client)
        {
            InitializeComponent();
            Client = client;
            Join();
        }
        private Client Client { get; }

        private async void Join()
        {
            await Client.RequestJoin();
        }

        private void BeginReceiveLoop() 
        {
            while (true)
            {
                Client.Receive();
            }
            var oi = Task.(() => Math.Max(100, 1);
            oi
        }
    }
}
