
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
            this.tableLayoutPanelLunchMode = new System.Windows.Forms.TableLayoutPanel();
            this.tbSkipPath = new System.Windows.Forms.TextBox();
            this.bSkipPath = new System.Windows.Forms.Button();
            this.rbSkip = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanelLunchMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelLunchMode
            // 
            this.tableLayoutPanelLunchMode.AutoSize = true;
            this.tableLayoutPanelLunchMode.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelLunchMode.ColumnCount = 3;
            this.tableLayoutPanelLunchMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLunchMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLunchMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLunchMode.Controls.Add(this.rbSkip, 0, 0);
            this.tableLayoutPanelLunchMode.Controls.Add(this.tbSkipPath, 1, 0);
            this.tableLayoutPanelLunchMode.Controls.Add(this.bSkipPath, 2, 0);
            this.tableLayoutPanelLunchMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelLunchMode.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelLunchMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 22);
            this.tableLayoutPanelLunchMode.Name = "tableLayoutPanelLunchMode";
            this.tableLayoutPanelLunchMode.RowCount = 1;
            this.tableLayoutPanelLunchMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLunchMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLunchMode.Size = new System.Drawing.Size(1403, 45);
            this.tableLayoutPanelLunchMode.TabIndex = 3;
            // 
            // tbSkipPath
            // 
            this.tbSkipPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbSkipPath.Location = new System.Drawing.Point(83, 5);
            this.tbSkipPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSkipPath.Name = "tbSkipPath";
            this.tbSkipPath.Size = new System.Drawing.Size(1273, 31);
            this.tbSkipPath.TabIndex = 0;
            // 
            // bSkipPath
            // 
            this.bSkipPath.AutoSize = true;
            this.bSkipPath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bSkipPath.Location = new System.Drawing.Point(1364, 5);
            this.bSkipPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bSkipPath.Name = "bSkipPath";
            this.bSkipPath.Size = new System.Drawing.Size(35, 35);
            this.bSkipPath.TabIndex = 3;
            this.bSkipPath.Text = "…";
            this.bSkipPath.UseVisualStyleBackColor = true;
            // 
            // rbSkip
            // 
            this.rbSkip.AutoSize = true;
            this.rbSkip.Location = new System.Drawing.Point(4, 5);
            this.rbSkip.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbSkip.Name = "rbSkip";
            this.rbSkip.Size = new System.Drawing.Size(71, 29);
            this.rbSkip.TabIndex = 5;
            this.rbSkip.TabStop = true;
            this.rbSkip.Text = "Skip";
            this.rbSkip.UseVisualStyleBackColor = true;
            // 
            // LSMControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelLunchMode);
            this.Name = "LSMControl";
            this.Size = new System.Drawing.Size(1403, 679);
            this.tableLayoutPanelLunchMode.ResumeLayout(false);
            this.tableLayoutPanelLunchMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLunchMode;
        private System.Windows.Forms.RadioButton rbSkip;
        private System.Windows.Forms.TextBox tbSkipPath;
        private System.Windows.Forms.Button bSkipPath;
    }
}
