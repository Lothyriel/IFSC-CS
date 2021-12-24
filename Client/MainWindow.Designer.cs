namespace ClientWindowsForms
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TbNome = new System.Windows.Forms.TextBox();
            this.LbName = new System.Windows.Forms.Label();
            this.BtJoin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TbNome
            // 
            this.TbNome.Location = new System.Drawing.Point(36, 39);
            this.TbNome.Name = "TbNome";
            this.TbNome.Size = new System.Drawing.Size(184, 23);
            this.TbNome.TabIndex = 0;
            // 
            // LbName
            // 
            this.LbName.AutoSize = true;
            this.LbName.Location = new System.Drawing.Point(36, 21);
            this.LbName.Name = "LbName";
            this.LbName.Size = new System.Drawing.Size(43, 15);
            this.LbName.TabIndex = 1;
            this.LbName.Text = "Nome:";
            // 
            // BtJoin
            // 
            this.BtJoin.Location = new System.Drawing.Point(85, 102);
            this.BtJoin.Name = "BtJoin";
            this.BtJoin.Size = new System.Drawing.Size(75, 23);
            this.BtJoin.TabIndex = 2;
            this.BtJoin.Text = "Conectar";
            this.BtJoin.UseVisualStyleBackColor = true;
            this.BtJoin.Click += new System.EventHandler(this.BtJoin_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 170);
            this.Controls.Add(this.BtJoin);
            this.Controls.Add(this.LbName);
            this.Controls.Add(this.TbNome);
            this.Name = "MainWindow";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox TbNome;
        private Label LbName;
        private Button BtJoin;
    }
}