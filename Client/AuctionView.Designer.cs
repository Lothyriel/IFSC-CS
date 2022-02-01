namespace Client
{
    partial class AuctionView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bt_Bid = new System.Windows.Forms.Button();
            this.tb_BidValue = new System.Windows.Forms.TextBox();
            this.lb_Product = new System.Windows.Forms.Label();
            this.lb_BidValue = new System.Windows.Forms.Label();
            this.lb_ProductName = new System.Windows.Forms.Label();
            this.lb_Bid = new System.Windows.Forms.Label();
            this.lb_BuyerName = new System.Windows.Forms.Label();
            this.lb_BuyerNameValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bt_Bid
            // 
            this.bt_Bid.Location = new System.Drawing.Point(76, 367);
            this.bt_Bid.Name = "bt_Bid";
            this.bt_Bid.Size = new System.Drawing.Size(94, 29);
            this.bt_Bid.TabIndex = 0;
            this.bt_Bid.Text = "Bid";
            this.bt_Bid.UseVisualStyleBackColor = true;
            this.bt_Bid.Click += new System.EventHandler(this.bt_Bid_Click);
            // 
            // tb_BidValue
            // 
            this.tb_BidValue.Location = new System.Drawing.Point(22, 314);
            this.tb_BidValue.Name = "tb_BidValue";
            this.tb_BidValue.Size = new System.Drawing.Size(207, 27);
            this.tb_BidValue.TabIndex = 1;
            this.tb_BidValue.TextChanged += new System.EventHandler(this.tb_BidValue_TextChanged);
            // 
            // lb_Product
            // 
            this.lb_Product.AutoSize = true;
            this.lb_Product.Location = new System.Drawing.Point(22, 31);
            this.lb_Product.Name = "lb_Product";
            this.lb_Product.Size = new System.Drawing.Size(63, 20);
            this.lb_Product.TabIndex = 3;
            this.lb_Product.Text = "Product:";
            // 
            // lb_BidValue
            // 
            this.lb_BidValue.AutoSize = true;
            this.lb_BidValue.Location = new System.Drawing.Point(124, 81);
            this.lb_BidValue.Name = "lb_BidValue";
            this.lb_BidValue.Size = new System.Drawing.Size(71, 20);
            this.lb_BidValue.TabIndex = 4;
            this.lb_BidValue.Text = "Bid Value";
            // 
            // lb_ProductName
            // 
            this.lb_ProductName.AutoSize = true;
            this.lb_ProductName.Location = new System.Drawing.Point(124, 31);
            this.lb_ProductName.Name = "lb_ProductName";
            this.lb_ProductName.Size = new System.Drawing.Size(100, 20);
            this.lb_ProductName.TabIndex = 5;
            this.lb_ProductName.Text = "Product Value";
            // 
            // lb_Bid
            // 
            this.lb_Bid.AutoSize = true;
            this.lb_Bid.Location = new System.Drawing.Point(22, 81);
            this.lb_Bid.Name = "lb_Bid";
            this.lb_Bid.Size = new System.Drawing.Size(34, 20);
            this.lb_Bid.TabIndex = 6;
            this.lb_Bid.Text = "Bid:";
            // 
            // lb_BuyerName
            // 
            this.lb_BuyerName.AutoSize = true;
            this.lb_BuyerName.Location = new System.Drawing.Point(22, 128);
            this.lb_BuyerName.Name = "lb_BuyerName";
            this.lb_BuyerName.Size = new System.Drawing.Size(93, 20);
            this.lb_BuyerName.TabIndex = 8;
            this.lb_BuyerName.Text = "Buyer Name:";
            // 
            // lb_BuyerNameValue
            // 
            this.lb_BuyerNameValue.AutoSize = true;
            this.lb_BuyerNameValue.Location = new System.Drawing.Point(124, 128);
            this.lb_BuyerNameValue.Name = "lb_BuyerNameValue";
            this.lb_BuyerNameValue.Size = new System.Drawing.Size(130, 20);
            this.lb_BuyerNameValue.TabIndex = 7;
            this.lb_BuyerNameValue.Text = "Buyer Name Value";
            // 
            // AuctionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 428);
            this.Controls.Add(this.lb_BuyerName);
            this.Controls.Add(this.lb_BuyerNameValue);
            this.Controls.Add(this.lb_Bid);
            this.Controls.Add(this.lb_ProductName);
            this.Controls.Add(this.lb_BidValue);
            this.Controls.Add(this.lb_Product);
            this.Controls.Add(this.tb_BidValue);
            this.Controls.Add(this.bt_Bid);
            this.Name = "AuctionView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AuctionView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button bt_Bid;
        private TextBox tb_BidValue;
        private Label lb_Bid;
        private Label lb_BidValue;
        private Label lb_Product;
        private Label lb_ProductName;
        private Label lb_BuyerName;
        private Label lb_BuyerNameValue;
    }
}