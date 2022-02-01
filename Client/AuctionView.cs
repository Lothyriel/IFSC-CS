using Domain;
using Domain.Business;
using Domain.Business.Exceptions;
using System.Security.Cryptography;

namespace Client
{
    public partial class AuctionView : Form
    {
        public AuctionView(ClientConnection client)
        {
            InitializeComponent();
            Client = client;

            bt_Bid.Enabled = false;

            lb_ProductName.Text = "Loading...";
            lb_BidValue.Text = "Loading...";
            lb_BuyerNameValue.Text = "Loading...";
            MainScreen.Instance!.Text = $"Auctioner - {client.BuyerData.Name}";

            Task.Run(() => Connect(Client));
        }

        private ClientConnection Client { get; }

        private Dictionary<(double, string), Bid> Bids { get; } = new();

        private Bid? Bid { get; set; }

        private async Task Connect(ClientConnection client)
        {
            try
            {
                await Client.RequestJoin();
                await Client.BeginReceiveLoop(BidHandler);
            }
            //catch (CryptographicException)
            //{
            //    await Connect(new ClientConnection(client.BuyerData.Name));
            //}
            catch (AuctionNotStarted)
            {
                MessageBox.Show("Auction Not Started", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainScreen.Instance?.OpenFormPanel(new Login());
            }
        }

        private void BidHandler(Bid newBid)
        {
            if (Bids.TryGetValue((newBid.Value, newBid.Buyer.PublicKey), out _))
                return;

            Bids.Add((newBid.Value, newBid.Buyer.PublicKey), newBid);

            if (!Invoke(UpdateLabels))
                Client.AuctionEnded = true;

            bool UpdateLabels()
            {
                if (newBid.AuctionExpired())
                {
                    MessageBox.Show($"Auction Ended Winner: {newBid.Buyer.Name} ", "Expired", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                Bid = newBid;
                lb_ProductName.Text = $"{newBid?.ProductName}";
                lb_BidValue.Text = $"{newBid?.Value}";
                lb_BuyerNameValue.Text = $"{newBid?.Buyer.Name}";
                return true;
            }
        }

        private bool ValidBid(double newValue)
        {
            if (Bid!.IsValid(newValue))
            {
                Bid = new Bid(Client.BuyerData, newValue, Bid!.ProductName, Bid.MinBid, Bid.DueTime);
                return true;
            }
            return false;
        }

        #region Events
        private void bt_Bid_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(tb_BidValue.Text, out double newValue))
            {
                MessageBox.Show("Input was not a number!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!ValidBid(newValue))
            {
                MessageBox.Show(Bid.InvalidMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bt_Bid.Enabled = false;
            tb_BidValue.Clear();
            Task.Run(() => Client.Bid(Bid!));
            MessageBox.Show(Bid.ValidMessage, "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tb_BidValue_TextChanged(object sender, EventArgs e)
        {
            bt_Bid.Enabled = tb_BidValue.Text != string.Empty && Bid != null;
        }

        #endregion
    }
}