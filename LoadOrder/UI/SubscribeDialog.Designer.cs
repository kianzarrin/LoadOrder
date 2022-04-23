
namespace LoadOrderTool.UI
{
    partial class SubscribeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubscribeDialog));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnIncludeAll = new System.Windows.Forms.Button();
            this.SubscribeAll = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbIDs = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Controls.Add(this.btnIncludeAll);
            this.flowLayoutPanel1.Controls.Add(this.SubscribeAll);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 702);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1143, 48);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(1032, 5);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 38);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnIncludeAll
            // 
            this.btnIncludeAll.AutoSize = true;
            this.btnIncludeAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnIncludeAll.Location = new System.Drawing.Point(920, 5);
            this.btnIncludeAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnIncludeAll.Name = "btnIncludeAll";
            this.btnIncludeAll.Size = new System.Drawing.Size(104, 35);
            this.btnIncludeAll.TabIndex = 2;
            this.btnIncludeAll.Text = "Include All";
            this.btnIncludeAll.UseVisualStyleBackColor = true;
            this.btnIncludeAll.Click += new System.EventHandler(this.btnIncludeAll_Click);
            // 
            // SubscribeAll
            // 
            this.SubscribeAll.AutoSize = true;
            this.SubscribeAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SubscribeAll.Location = new System.Drawing.Point(721, 5);
            this.SubscribeAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SubscribeAll.Name = "SubscribeAll";
            this.SubscribeAll.Size = new System.Drawing.Size(191, 35);
            this.SubscribeAll.TabIndex = 1;
            this.SubscribeAll.Text = "Subscribe to All in CS";
            this.SubscribeAll.UseVisualStyleBackColor = true;
            this.SubscribeAll.Click += new System.EventHandler(this.SubscribeAll_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(997, 100);
            this.label1.TabIndex = 3;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // tbIDs
            // 
            this.tbIDs.AcceptsReturn = true;
            this.tbIDs.AllowDrop = true;
            this.tbIDs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbIDs.Location = new System.Drawing.Point(0, 100);
            this.tbIDs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbIDs.Multiline = true;
            this.tbIDs.Name = "tbIDs";
            this.tbIDs.Size = new System.Drawing.Size(1143, 602);
            this.tbIDs.TabIndex = 4;
            // 
            // SubscribeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 750);
            this.Controls.Add(this.tbIDs);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SubscribeDialog";
            this.Text = "Mass Subscribe";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button SubscribeAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbIDs;
        private System.Windows.Forms.Button btnIncludeAll;
    }
}