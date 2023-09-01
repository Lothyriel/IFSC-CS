using Domain;

namespace Client
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            bt_Connect.Enabled = false;
        }
        private ClientConnection? Client { get; set; }

        #region Events

        private void bt_Connect_Click(object sender, EventArgs e)
        {
            Client = new(tb_Name.Text);
            MainScreen.Instance?.OpenFormPanel(new AuctionView(Client));
        }

        private void tb_Name_TextChanged(object sender, EventArgs e)
        {
            bt_Connect.Enabled = tb_Name.Text != string.Empty;
        }

        #endregion
    }
}