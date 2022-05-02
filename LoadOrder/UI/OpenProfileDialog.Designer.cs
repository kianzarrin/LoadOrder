using PresentationControls;

namespace LoadOrderTool.UI {

    partial class OpenProfileDialog {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblMissingItems = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cID = new System.Windows.Forms.DataGridViewLinkColumn();
            this.cType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAppend = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.cbItemType = new CheckBoxComboBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSubscribeAll = new System.Windows.Forms.Button();
            this.btnSubscribeMissing = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMissingItems
            // 
            this.lblMissingItems.AutoSize = true;
            this.lblMissingItems.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMissingItems.Location = new System.Drawing.Point(0, 0);
            this.lblMissingItems.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMissingItems.Name = "lblMissingItems";
            this.lblMissingItems.Size = new System.Drawing.Size(125, 25);
            this.lblMissingItems.TabIndex = 0;
            this.lblMissingItems.Text = "Missing items:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Beige;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cID,
            this.cType,
            this.cName});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 25);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(1017, 873);
            this.dataGridView1.TabIndex = 1;
            // 
            // cID
            // 
            this.cID.HeaderText = "ID";
            this.cID.MinimumWidth = 8;
            this.cID.Name = "cID";
            this.cID.ReadOnly = true;
            this.cID.Width = 150;
            // 
            // cType
            // 
            this.cType.HeaderText = "Type";
            this.cType.MinimumWidth = 8;
            this.cType.Name = "cType";
            this.cType.ReadOnly = true;
            this.cType.Width = 150;
            // 
            // cName
            // 
            this.cName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cName.HeaderText = "Name";
            this.cName.MinimumWidth = 8;
            this.cName.Name = "cName";
            this.cName.ReadOnly = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Controls.Add(this.btnAppend);
            this.flowLayoutPanel1.Controls.Add(this.btnReplace);
            this.flowLayoutPanel1.Controls.Add(this.btnReload);
            this.flowLayoutPanel1.Controls.Add(this.cbItemType);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel2);
            this.flowLayoutPanel1.Controls.Add(this.btnSubscribeAll);
            this.flowLayoutPanel1.Controls.Add(this.btnSubscribeMissing);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 853);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1017, 45);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCancel.Location = new System.Drawing.Point(940, 5);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 35);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAppend
            // 
            this.btnAppend.AutoSize = true;
            this.btnAppend.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAppend.Location = new System.Drawing.Point(846, 5);
            this.btnAppend.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(86, 35);
            this.btnAppend.TabIndex = 2;
            this.btnAppend.Text = "Append";
            this.btnAppend.UseVisualStyleBackColor = true;
            // 
            // btnReplace
            // 
            this.btnReplace.AutoSize = true;
            this.btnReplace.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnReplace.Location = new System.Drawing.Point(756, 5);
            this.btnReplace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(82, 35);
            this.btnReplace.TabIndex = 3;
            this.btnReplace.Text = "Replace";
            this.btnReplace.UseVisualStyleBackColor = true;
            // 
            // btnReload
            // 
            this.btnReload.AutoSize = true;
            this.btnReload.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnReload.Location = new System.Drawing.Point(672, 5);
            this.btnReload.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(76, 35);
            this.btnReload.TabIndex = 4;
            this.btnReload.Text = "Reload";
            this.btnReload.UseVisualStyleBackColor = true;
            // 
            // cbItemType
            // 
            this.cbItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbItemType.FormattingEnabled = true;
            this.cbItemType.Location = new System.Drawing.Point(453, 5);
            this.cbItemType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbItemType.Name = "cbItemType";
            this.cbItemType.Size = new System.Drawing.Size(211, 33);
            this.cbItemType.TabIndex = 1;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(449, 0);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(0, 0);
            this.flowLayoutPanel2.TabIndex = 5;
            // 
            // btnSubscribeAll
            // 
            this.btnSubscribeAll.AutoSize = true;
            this.btnSubscribeAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSubscribeAll.Location = new System.Drawing.Point(231, 5);
            this.btnSubscribeAll.Margin = new System.Windows.Forms.Padding(4, 5, 29, 5);
            this.btnSubscribeAll.Name = "btnSubscribeAll";
            this.btnSubscribeAll.Size = new System.Drawing.Size(189, 35);
            this.btnSubscribeAll.TabIndex = 6;
            this.btnSubscribeAll.Text = "Subscribe To all in CS";
            this.btnSubscribeAll.UseVisualStyleBackColor = true;
            this.btnSubscribeAll.Click += new System.EventHandler(this.SubscribeAll_Click);
            // 
            // btnSubscribeMissing
            // 
            this.btnSubscribeMissing.AutoSize = true;
            this.btnSubscribeMissing.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSubscribeMissing.Location = new System.Drawing.Point(55, 5);
            this.btnSubscribeMissing.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSubscribeMissing.Name = "btnSubscribeMissing";
            this.btnSubscribeMissing.Size = new System.Drawing.Size(233, 35);
            this.btnSubscribeMissing.TabIndex = 7;
            this.btnSubscribeMissing.Text = "Subscribe To Missing in CS";
            this.btnSubscribeMissing.UseVisualStyleBackColor = true;
            this.btnSubscribeMissing.Click += new System.EventHandler(this.SubscribeMissing_Click);
            // 
            // OpenProfileDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 898);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblMissingItems);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "OpenProfileDialog";
            this.Text = "Open Profile";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMissingItems;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnCancel;
        private CheckBoxComboBox cbItemType;
        private System.Windows.Forms.Button btnAppend;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.DataGridViewLinkColumn cID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnSubscribeAll;
        private System.Windows.Forms.Button btnSubscribeMissing;
    }
}