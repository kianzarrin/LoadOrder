namespace LoadOrderTool.UI {

    partial class ResubscribeDialog {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tbIDs = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cbMissing = new System.Windows.Forms.CheckBox();
            this.btnUnsubscribeAll = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnSubscribeAll = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnFinish = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // tbIDs
            // 
            this.tbIDs.AcceptsReturn = true;
            this.tbIDs.AllowDrop = true;
            this.tbIDs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbIDs.Location = new System.Drawing.Point(0, 0);
            this.tbIDs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbIDs.Multiline = true;
            this.tbIDs.Name = "tbIDs";
            this.tbIDs.Size = new System.Drawing.Size(929, 591);
            this.tbIDs.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.cbMissing);
            this.flowLayoutPanel1.Controls.Add(this.btnUnsubscribeAll);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox1);
            this.flowLayoutPanel1.Controls.Add(this.btnSubscribeAll);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox2);
            this.flowLayoutPanel1.Controls.Add(this.btnFinish);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 591);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(929, 48);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // cbMissing
            // 
            this.cbMissing.AutoSize = true;
            this.cbMissing.Checked = true;
            this.cbMissing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMissing.Location = new System.Drawing.Point(4, 7);
            this.cbMissing.Margin = new System.Windows.Forms.Padding(4, 7, 4, 5);
            this.cbMissing.Name = "cbMissing";
            this.cbMissing.Size = new System.Drawing.Size(161, 29);
            this.cbMissing.TabIndex = 3;
            this.cbMissing.Text = "Include missing";
            this.cbMissing.UseVisualStyleBackColor = true;
            // 
            // btnUnsubscribeAll
            // 
            this.btnUnsubscribeAll.AutoSize = true;
            this.btnUnsubscribeAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnUnsubscribeAll.Location = new System.Drawing.Point(173, 5);
            this.btnUnsubscribeAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnUnsubscribeAll.Name = "btnUnsubscribeAll";
            this.btnUnsubscribeAll.Size = new System.Drawing.Size(233, 35);
            this.btnUnsubscribeAll.TabIndex = 2;
            this.btnUnsubscribeAll.Text = "Unsubscribe from All in CS";
            this.btnUnsubscribeAll.UseVisualStyleBackColor = true;
            this.btnUnsubscribeAll.Click += new System.EventHandler(this.btnUnsubscribeAll_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(413, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 25);
            this.label1.TabIndex = 4;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(419, 5);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 35);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // btnSubscribeAll
            // 
            this.btnSubscribeAll.AutoSize = true;
            this.btnSubscribeAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSubscribeAll.Location = new System.Drawing.Point(476, 5);
            this.btnSubscribeAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSubscribeAll.Name = "btnSubscribeAll";
            this.btnSubscribeAll.Size = new System.Drawing.Size(191, 35);
            this.btnSubscribeAll.TabIndex = 1;
            this.btnSubscribeAll.Text = "Subscribe to All in CS";
            this.btnSubscribeAll.UseVisualStyleBackColor = true;
            this.btnSubscribeAll.Click += new System.EventHandler(this.btnSubscribeAll_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.ErrorImage = null;
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(674, 5);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(50, 35);
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // btnFinish
            // 
            this.btnFinish.Location = new System.Drawing.Point(731, 5);
            this.btnFinish.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(107, 38);
            this.btnFinish.TabIndex = 7;
            this.btnFinish.Text = "Finish";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // ResubscribeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 639);
            this.Controls.Add(this.tbIDs);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "ResubscribeDialog";
            this.Text = "Resubscribe broken downloads";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbIDs;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox cbMissing;
        private System.Windows.Forms.Button btnUnsubscribeAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnSubscribeAll;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnFinish;
    }
}