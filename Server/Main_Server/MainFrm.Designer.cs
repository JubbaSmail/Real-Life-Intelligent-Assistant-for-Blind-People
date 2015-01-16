namespace Main_Server
{
    partial class MainFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.txt_zoom = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Show_on_Google_Map = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.labelSW = new System.Windows.Forms.Label();
            this.labelW = new System.Windows.Forms.Label();
            this.labelNW = new System.Windows.Forms.Label();
            this.labelN = new System.Windows.Forms.Label();
            this.label_NE = new System.Windows.Forms.Label();
            this.label_E = new System.Windows.Forms.Label();
            this.label_SE = new System.Windows.Forms.Label();
            this.label_S = new System.Windows.Forms.Label();
            this.label_Counter = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_server_port = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_zoom
            // 
            this.txt_zoom.Location = new System.Drawing.Point(106, 266);
            this.txt_zoom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_zoom.Name = "txt_zoom";
            this.txt_zoom.Size = new System.Drawing.Size(40, 24);
            this.txt_zoom.TabIndex = 38;
            this.txt_zoom.Text = "15";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 268);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 17);
            this.label3.TabIndex = 39;
            this.label3.Text = "Zoom Value";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(152, 268);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 17);
            this.label1.TabIndex = 40;
            this.label1.Text = "X";
            // 
            // btn_Show_on_Google_Map
            // 
            this.btn_Show_on_Google_Map.Enabled = false;
            this.btn_Show_on_Google_Map.Location = new System.Drawing.Point(29, 313);
            this.btn_Show_on_Google_Map.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Show_on_Google_Map.Name = "btn_Show_on_Google_Map";
            this.btn_Show_on_Google_Map.Size = new System.Drawing.Size(148, 28);
            this.btn_Show_on_Google_Map.TabIndex = 42;
            this.btn_Show_on_Google_Map.Text = "Show on Google Map";
            this.btn_Show_on_Google_Map.UseVisualStyleBackColor = true;
            this.btn_Show_on_Google_Map.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Main_Server.Properties.Resources.Compass_3;
            this.pictureBox2.Location = new System.Drawing.Point(3, 2);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(231, 249);
            this.pictureBox2.TabIndex = 53;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1040, 711);
            this.pictureBox1.TabIndex = 41;
            this.pictureBox1.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.labelSW);
            this.splitContainer1.Panel2.Controls.Add(this.labelW);
            this.splitContainer1.Panel2.Controls.Add(this.labelNW);
            this.splitContainer1.Panel2.Controls.Add(this.labelN);
            this.splitContainer1.Panel2.Controls.Add(this.label_NE);
            this.splitContainer1.Panel2.Controls.Add(this.label_E);
            this.splitContainer1.Panel2.Controls.Add(this.label_SE);
            this.splitContainer1.Panel2.Controls.Add(this.label_S);
            this.splitContainer1.Panel2.Controls.Add(this.label_Counter);
            this.splitContainer1.Panel2.Controls.Add(this.button5);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox2);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.btn_Show_on_Google_Map);
            this.splitContainer1.Panel2.Controls.Add(this.txt_zoom);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.txt_server_port);
            this.splitContainer1.Size = new System.Drawing.Size(1274, 711);
            this.splitContainer1.SplitterDistance = 1040;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 54;
            // 
            // labelSW
            // 
            this.labelSW.AutoSize = true;
            this.labelSW.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.labelSW.Location = new System.Drawing.Point(39, 133);
            this.labelSW.Name = "labelSW";
            this.labelSW.Size = new System.Drawing.Size(50, 24);
            this.labelSW.TabIndex = 57;
            this.labelSW.Text = "S W";
            this.labelSW.Click += new System.EventHandler(this.labelSW_Click);
            // 
            // labelW
            // 
            this.labelW.AutoSize = true;
            this.labelW.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.labelW.Location = new System.Drawing.Point(25, 87);
            this.labelW.Name = "labelW";
            this.labelW.Size = new System.Drawing.Size(31, 24);
            this.labelW.TabIndex = 57;
            this.labelW.Text = "W";
            this.labelW.Click += new System.EventHandler(this.labelW_Click);
            // 
            // labelNW
            // 
            this.labelNW.AutoSize = true;
            this.labelNW.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.labelNW.Location = new System.Drawing.Point(40, 46);
            this.labelNW.Name = "labelNW";
            this.labelNW.Size = new System.Drawing.Size(52, 24);
            this.labelNW.TabIndex = 57;
            this.labelNW.Text = "N W";
            this.labelNW.Click += new System.EventHandler(this.labelNW_Click);
            // 
            // labelN
            // 
            this.labelN.AutoSize = true;
            this.labelN.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.labelN.Location = new System.Drawing.Point(92, 20);
            this.labelN.Name = "labelN";
            this.labelN.Size = new System.Drawing.Size(25, 24);
            this.labelN.TabIndex = 57;
            this.labelN.Text = "N";
            this.labelN.Click += new System.EventHandler(this.labelN_Click);
            // 
            // label_NE
            // 
            this.label_NE.AutoSize = true;
            this.label_NE.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.label_NE.Location = new System.Drawing.Point(124, 48);
            this.label_NE.Name = "label_NE";
            this.label_NE.Size = new System.Drawing.Size(43, 24);
            this.label_NE.TabIndex = 57;
            this.label_NE.Text = "N E";
            this.label_NE.Click += new System.EventHandler(this.label_NE_Click);
            // 
            // label_E
            // 
            this.label_E.AutoSize = true;
            this.label_E.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.label_E.Location = new System.Drawing.Point(157, 88);
            this.label_E.Name = "label_E";
            this.label_E.Size = new System.Drawing.Size(22, 24);
            this.label_E.TabIndex = 57;
            this.label_E.Text = "E";
            this.label_E.Click += new System.EventHandler(this.label_E_Click);
            // 
            // label_SE
            // 
            this.label_SE.AutoSize = true;
            this.label_SE.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.label_SE.Location = new System.Drawing.Point(124, 136);
            this.label_SE.Name = "label_SE";
            this.label_SE.Size = new System.Drawing.Size(41, 24);
            this.label_SE.TabIndex = 57;
            this.label_SE.Text = "S E";
            this.label_SE.Click += new System.EventHandler(this.label_SE_Click);
            // 
            // label_S
            // 
            this.label_S.AutoSize = true;
            this.label_S.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.label_S.Location = new System.Drawing.Point(92, 158);
            this.label_S.Name = "label_S";
            this.label_S.Size = new System.Drawing.Size(23, 24);
            this.label_S.TabIndex = 57;
            this.label_S.Text = "S";
            this.label_S.Click += new System.EventHandler(this.label_S_Click);
            // 
            // label_Counter
            // 
            this.label_Counter.AutoSize = true;
            this.label_Counter.Location = new System.Drawing.Point(27, 428);
            this.label_Counter.Name = "label_Counter";
            this.label_Counter.Size = new System.Drawing.Size(71, 17);
            this.label_Counter.TabIndex = 55;
            this.label_Counter.Text = "Counter : ";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(29, 369);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(148, 28);
            this.button5.TabIndex = 54;
            this.button5.Text = "Save Blind Path";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 491);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 17);
            this.label2.TabIndex = 39;
            this.label2.Text = "Server Port :";
            // 
            // txt_server_port
            // 
            this.txt_server_port.Location = new System.Drawing.Point(119, 487);
            this.txt_server_port.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_server_port.Name = "txt_server_port";
            this.txt_server_port.Size = new System.Drawing.Size(76, 24);
            this.txt_server_port.TabIndex = 38;
            this.txt_server_port.Text = "4444";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1274, 711);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "0";
            this.Text = "Complete Navigator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txt_zoom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_Show_on_Google_Map;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label_Counter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_server_port;
        private System.Windows.Forms.Label label_S;
        private System.Windows.Forms.Label labelSW;
        private System.Windows.Forms.Label labelW;
        private System.Windows.Forms.Label labelNW;
        private System.Windows.Forms.Label labelN;
        private System.Windows.Forms.Label label_NE;
        private System.Windows.Forms.Label label_E;
        private System.Windows.Forms.Label label_SE;
    }
}

