
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SubscribeAll = new System.Windows.Forms.Button();
            this.tbIDs = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Controls.Add(this.SubscribeAll);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 419);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(800, 31);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(722, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SubscribeAll
            // 
            this.SubscribeAll.AutoSize = true;
            this.SubscribeAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SubscribeAll.Location = new System.Drawing.Point(587, 3);
            this.SubscribeAll.Name = "SubscribeAll";
            this.SubscribeAll.Size = new System.Drawing.Size(129, 25);
            this.SubscribeAll.TabIndex = 1;
            this.SubscribeAll.Text = "Subscribe to All in CS";
            this.SubscribeAll.UseVisualStyleBackColor = true;
            this.SubscribeAll.Click += new System.EventHandler(this.SubscribeAll_Click);
            // 
            // tbAssets
            // 
            this.tbIDs.AcceptsReturn = true;
            this.tbIDs.AllowDrop = true;
            this.tbIDs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbIDs.Location = new System.Drawing.Point(0, 0);
            this.tbIDs.Multiline = true;
            this.tbIDs.Name = "tbAssets";
            this.tbIDs.Size = new System.Drawing.Size(800, 419);
            this.tbIDs.TabIndex = 2;
            this.tbIDs.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbAssets_DragDrop);
            this.tbIDs.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbAssets_DragEnter);
            this.tbIDs.DragLeave += new System.EventHandler(this.tbAssets_DragLeave);
            // 
            // SubscribeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tbIDs);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "SubscribeDialog";
            this.Text = "SubscribeDialog";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button SubscribeAll;
        private System.Windows.Forms.TextBox tbIDs;
    }
}