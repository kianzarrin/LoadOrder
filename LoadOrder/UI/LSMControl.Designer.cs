
namespace LoadOrderTool.UI {
    partial class LSMControl {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanelLunchMode = new System.Windows.Forms.TableLayoutPanel();
            this.cbSkip = new System.Windows.Forms.CheckBox();
            this.tbSkipPath = new System.Windows.Forms.TextBox();
            this.bSkipPath = new System.Windows.Forms.Button();
            this.cbLoadEnabled = new System.Windows.Forms.CheckBox();
            this.cbLoadUsed = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanelLunchMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanelLunchMode);
            this.flowLayoutPanel1.Controls.Add(this.cbLoadEnabled);
            this.flowLayoutPanel1.Controls.Add(this.cbLoadUsed);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1403, 679);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanelLunchMode
            // 
            this.tableLayoutPanelLunchMode.AutoSize = true;
            this.tableLayoutPanelLunchMode.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelLunchMode.ColumnCount = 3;
            this.tableLayoutPanelLunchMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLunchMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLunchMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLunchMode.Controls.Add(this.cbSkip, 0, 0);
            this.tableLayoutPanelLunchMode.Controls.Add(this.tbSkipPath, 1, 0);
            this.tableLayoutPanelLunchMode.Controls.Add(this.bSkipPath, 2, 0);
            this.tableLayoutPanelLunchMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelLunchMode.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanelLunchMode.Margin = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanelLunchMode.Name = "tableLayoutPanelLunchMode";
            this.tableLayoutPanelLunchMode.RowCount = 1;
            this.tableLayoutPanelLunchMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLunchMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanelLunchMode.Size = new System.Drawing.Size(1393, 45);
            this.tableLayoutPanelLunchMode.TabIndex = 4;
            // 
            // cbSkip
            // 
            this.cbSkip.AutoSize = true;
            this.cbSkip.Location = new System.Drawing.Point(4, 5);
            this.cbSkip.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbSkip.Name = "cbSkip";
            this.cbSkip.Size = new System.Drawing.Size(72, 29);
            this.cbSkip.TabIndex = 5;
            this.cbSkip.Text = "Skip";
            this.cbSkip.UseVisualStyleBackColor = true;
            // 
            // tbSkipPath
            // 
            this.tbSkipPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbSkipPath.Location = new System.Drawing.Point(84, 5);
            this.tbSkipPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSkipPath.Name = "tbSkipPath";
            this.tbSkipPath.Size = new System.Drawing.Size(1262, 31);
            this.tbSkipPath.TabIndex = 0;
            // 
            // bSkipPath
            // 
            this.bSkipPath.AutoSize = true;
            this.bSkipPath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bSkipPath.Location = new System.Drawing.Point(1354, 5);
            this.bSkipPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bSkipPath.Name = "bSkipPath";
            this.bSkipPath.Size = new System.Drawing.Size(35, 35);
            this.bSkipPath.TabIndex = 3;
            this.bSkipPath.Text = "…";
            this.bSkipPath.UseVisualStyleBackColor = true;
            // 
            // cbLoadEnabled
            // 
            this.cbLoadEnabled.AutoSize = true;
            this.cbLoadEnabled.Location = new System.Drawing.Point(3, 58);
            this.cbLoadEnabled.Name = "cbLoadEnabled";
            this.cbLoadEnabled.Size = new System.Drawing.Size(145, 29);
            this.cbLoadEnabled.TabIndex = 5;
            this.cbLoadEnabled.Text = "Load Enabled";
            this.cbLoadEnabled.UseVisualStyleBackColor = true;
            // 
            // LoadUsed
            // 
            this.cbLoadUsed.AutoSize = true;
            this.cbLoadUsed.Location = new System.Drawing.Point(3, 93);
            this.cbLoadUsed.Name = "LoadUsed";
            this.cbLoadUsed.Size = new System.Drawing.Size(122, 29);
            this.cbLoadUsed.TabIndex = 6;
            this.cbLoadUsed.Text = "Load Used";
            this.cbLoadUsed.UseVisualStyleBackColor = true;
            // 
            // LSMControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "LSMControl";
            this.Size = new System.Drawing.Size(1403, 679);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanelLunchMode.ResumeLayout(false);
            this.tableLayoutPanelLunchMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLunchMode;
        private System.Windows.Forms.CheckBox cbSkip;
        private System.Windows.Forms.TextBox tbSkipPath;
        private System.Windows.Forms.Button bSkipPath;
        private System.Windows.Forms.CheckBox cbLoadEnabled;
        private System.Windows.Forms.CheckBox cbLoadUsed;
    }
}
