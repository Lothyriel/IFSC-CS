using Domain;

namespace ClientWindowsForms
{
    public partial class MainWindow : Form
    {
        public static MainWindow? Instance { get; private set; }
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
        }

        private void BtJoin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TbNome.Text))
                MessageBox.Show("Insira um nome para participar do leilão...");
            
            Hide();
            new BidScreen(new Client(TbNome.Text)).Show();
        }
    }
}