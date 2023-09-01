namespace Client
{
    public partial class MainScreen : Form
    {
        public static MainScreen? Instance { get; set; }

        public MainScreen()
        {
            InitializeComponent();
            Instance = this;
            OpenFormPanel(new Login());
        }
        public void OpenFormPanel(Form panelForm)
        {
            panelForm.TopLevel = false;
            panelForm.FormBorderStyle = FormBorderStyle.None;
            panelForm.Dock = DockStyle.Fill;

            //Panel.Controls.Clear();
            Panel.Controls.Add(panelForm);

            panelForm.BringToFront();
            panelForm.Show();
        }
    }
}
