namespace LoadOrderTool.UI {

    partial class SelectPathsDialog {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxGamePath = new System.Windows.Forms.TextBox();
            this.textBoxSteamPath = new System.Windows.Forms.TextBox();
            this.buttonGamePath = new System.Windows.Forms.Button();
            this.buttonSteamPath = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.OKButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel1);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel2);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(621, 124);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please select path to the following:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxGamePath, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxSteamPath, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonGamePath, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonSteamPath, 2, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 20);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(615, 66);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Cities.exe:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Steam.exe:";
            // 
            // textBoxGamePath
            // 
            this.textBoxGamePath.Location = new System.Drawing.Point(79, 3);
            this.textBoxGamePath.Name = "textBoxGamePath";
            this.textBoxGamePath.Size = new System.Drawing.Size(500, 25);
            this.textBoxGamePath.TabIndex = 2;
            // 
            // textBoxSteamPath
            // 
            this.textBoxSteamPath.Location = new System.Drawing.Point(79, 36);
            this.textBoxSteamPath.Name = "textBoxSteamPath";
            this.textBoxSteamPath.Size = new System.Drawing.Size(500, 25);
            this.textBoxSteamPath.TabIndex = 3;
            // 
            // buttonGamePath
            // 
            this.buttonGamePath.AutoSize = true;
            this.buttonGamePath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonGamePath.Location = new System.Drawing.Point(585, 3);
            this.buttonGamePath.Name = "buttonGamePath";
            this.buttonGamePath.Size = new System.Drawing.Size(27, 27);
            this.buttonGamePath.TabIndex = 4;
            this.buttonGamePath.Text = "…";
            this.buttonGamePath.UseVisualStyleBackColor = true;
            // 
            // buttonSteamPath
            // 
            this.buttonSteamPath.AutoSize = true;
            this.buttonSteamPath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonSteamPath.Location = new System.Drawing.Point(585, 36);
            this.buttonSteamPath.Name = "buttonSteamPath";
            this.buttonSteamPath.Size = new System.Drawing.Size(27, 27);
            this.buttonSteamPath.TabIndex = 5;
            this.buttonSteamPath.Text = "…";
            this.buttonSteamPath.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.OKButton);
            this.flowLayoutPanel2.Controls.Add(this.ExitButton);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(456, 92);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(162, 29);
            this.flowLayoutPanel2.TabIndex = 2;
            this.flowLayoutPanel2.WrapContents = false;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(3, 3);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "&OK";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.ExitButton.Location = new System.Drawing.Point(84, 3);
            this.ExitButton.Name = "button2";
            this.ExitButton.Size = new System.Drawing.Size(75, 23);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "&Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            // 
            // SelectPathsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(632, 137);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "SelectPathsDialog";
            this.Text = "Select Paths";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxGamePath;
        private System.Windows.Forms.TextBox textBoxSteamPath;
        private System.Windows.Forms.Button buttonGamePath;
        private System.Windows.Forms.Button buttonSteamPath;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button ExitButton;
    }
}

