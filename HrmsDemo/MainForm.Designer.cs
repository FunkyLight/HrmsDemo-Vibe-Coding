namespace HrmsDemo
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            menuSystem = new ToolStripMenuItem();
            menuItemLogout = new ToolStripMenuItem();
            menuManage = new ToolStripMenuItem();
            menuItemDept = new ToolStripMenuItem();
            menuItemRank = new ToolStripMenuItem();
            menuItemPerm = new ToolStripMenuItem();
            menuItemRolePerm = new ToolStripMenuItem();
            splitContainer1 = new SplitContainer();
            splitContainerLeft = new SplitContainer();
            btnSearch = new Button();
            cbSearchRank = new ComboBox();
            cbSearchDept = new ComboBox();
            txtSearch = new TextBox();
            dgvEmployees = new DataGridView();
            btnCancel = new Button();
            btnDelete = new Button();
            btnEdit = new Button();
            btnAdd = new Button();
            cbIsResigned = new CheckBox();
            dtpLeaveDate = new DateTimePicker();
            label10 = new Label();
            dtpJoinDate = new DateTimePicker();
            label9 = new Label();
            txtNote = new TextBox();
            label8 = new Label();
            cbRank = new ComboBox();
            label7 = new Label();
            cbDept = new ComboBox();
            label6 = new Label();
            txtEmail = new TextBox();
            label5 = new Label();
            txtMobile = new TextBox();
            label4 = new Label();
            dtpBirthday = new DateTimePicker();
            label3 = new Label();
            cbGender = new ComboBox();
            labelGender = new Label();
            txtName = new TextBox();
            label2 = new Label();
            txtPassword = new TextBox();
            labelPass = new Label();
            txtAccount = new TextBox();
            label1 = new Label();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerLeft).BeginInit();
            splitContainerLeft.Panel1.SuspendLayout();
            splitContainerLeft.Panel2.SuspendLayout();
            splitContainerLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { menuSystem, menuManage });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1201, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // menuSystem
            // 
            menuSystem.DropDownItems.AddRange(new ToolStripItem[] { menuItemLogout });
            menuSystem.Font = new Font("Microsoft JhengHei UI", 12F);
            menuSystem.Name = "menuSystem";
            menuSystem.Size = new Size(53, 24);
            menuSystem.Text = "系統";
            // 
            // menuItemLogout
            // 
            menuItemLogout.Name = "menuItemLogout";
            menuItemLogout.Size = new Size(110, 24);
            menuItemLogout.Text = "登出";
            menuItemLogout.Click += menuItemLogout_Click;
            // 
            // menuManage
            // 
            menuManage.DropDownItems.AddRange(new ToolStripItem[] { menuItemDept, menuItemRank, menuItemPerm, menuItemRolePerm });
            menuManage.Font = new Font("Microsoft JhengHei UI", 12F);
            menuManage.Name = "menuManage";
            menuManage.Size = new Size(53, 24);
            menuManage.Text = "管理";
            // 
            // menuItemDept
            // 
            menuItemDept.Name = "menuItemDept";
            menuItemDept.Size = new Size(174, 24);
            menuItemDept.Text = "部門資料管理";
            menuItemDept.Click += menuItemDept_Click;
            // 
            // menuItemRank
            // 
            menuItemRank.Name = "menuItemRank";
            menuItemRank.Size = new Size(174, 24);
            menuItemRank.Text = "職位資料管理";
            menuItemRank.Click += menuItemRank_Click;
            // 
            // menuItemPerm
            // 
            menuItemPerm.Name = "menuItemPerm";
            menuItemPerm.Size = new Size(174, 24);
            menuItemPerm.Text = "權限資料管理";
            menuItemPerm.Click += menuItemPerm_Click;
            // 
            // menuItemRolePerm
            // 
            menuItemRolePerm.Name = "menuItemRolePerm";
            menuItemRolePerm.Size = new Size(174, 24);
            menuItemRolePerm.Text = "角色權限管理";
            menuItemRolePerm.Click += menuItemRolePerm_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Location = new Point(0, 28);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainerLeft);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(btnCancel);
            splitContainer1.Panel2.Controls.Add(btnDelete);
            splitContainer1.Panel2.Controls.Add(btnEdit);
            splitContainer1.Panel2.Controls.Add(btnAdd);
            splitContainer1.Panel2.Controls.Add(cbIsResigned);
            splitContainer1.Panel2.Controls.Add(dtpLeaveDate);
            splitContainer1.Panel2.Controls.Add(label10);
            splitContainer1.Panel2.Controls.Add(dtpJoinDate);
            splitContainer1.Panel2.Controls.Add(label9);
            splitContainer1.Panel2.Controls.Add(txtNote);
            splitContainer1.Panel2.Controls.Add(label8);
            splitContainer1.Panel2.Controls.Add(cbRank);
            splitContainer1.Panel2.Controls.Add(label7);
            splitContainer1.Panel2.Controls.Add(cbDept);
            splitContainer1.Panel2.Controls.Add(label6);
            splitContainer1.Panel2.Controls.Add(txtEmail);
            splitContainer1.Panel2.Controls.Add(label5);
            splitContainer1.Panel2.Controls.Add(txtMobile);
            splitContainer1.Panel2.Controls.Add(label4);
            splitContainer1.Panel2.Controls.Add(dtpBirthday);
            splitContainer1.Panel2.Controls.Add(label3);
            splitContainer1.Panel2.Controls.Add(cbGender);
            splitContainer1.Panel2.Controls.Add(labelGender);
            splitContainer1.Panel2.Controls.Add(txtName);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Panel2.Controls.Add(txtPassword);
            splitContainer1.Panel2.Controls.Add(labelPass);
            splitContainer1.Panel2.Controls.Add(txtAccount);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Size = new Size(1201, 772);
            splitContainer1.SplitterDistance = 580;
            splitContainer1.TabIndex = 1;
            // 
            // splitContainerLeft
            // 
            splitContainerLeft.Dock = DockStyle.Fill;
            splitContainerLeft.FixedPanel = FixedPanel.Panel1;
            splitContainerLeft.Location = new Point(0, 0);
            splitContainerLeft.Name = "splitContainerLeft";
            splitContainerLeft.Orientation = Orientation.Horizontal;
            // 
            // splitContainerLeft.Panel1
            // 
            splitContainerLeft.Panel1.BackgroundImageLayout = ImageLayout.None;
            splitContainerLeft.Panel1.Controls.Add(btnSearch);
            splitContainerLeft.Panel1.Controls.Add(cbSearchRank);
            splitContainerLeft.Panel1.Controls.Add(cbSearchDept);
            splitContainerLeft.Panel1.Controls.Add(txtSearch);
            // 
            // splitContainerLeft.Panel2
            // 
            splitContainerLeft.Panel2.Controls.Add(dgvEmployees);
            splitContainerLeft.Size = new Size(580, 772);
            splitContainerLeft.SplitterDistance = 80;
            splitContainerLeft.TabIndex = 0;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(136, 15);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 28);
            btnSearch.TabIndex = 0;
            btnSearch.Text = "搜尋";
            btnSearch.Click += btnSearch_Click;
            // 
            // cbSearchRank
            // 
            cbSearchRank.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSearchRank.Location = new Point(377, 15);
            cbSearchRank.Name = "cbSearchRank";
            cbSearchRank.Size = new Size(80, 28);
            cbSearchRank.TabIndex = 1;
            cbSearchRank.SelectedIndexChanged += btnSearch_Click;
            // 
            // cbSearchDept
            // 
            cbSearchDept.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSearchDept.Location = new Point(287, 15);
            cbSearchDept.Name = "cbSearchDept";
            cbSearchDept.Size = new Size(80, 28);
            cbSearchDept.TabIndex = 2;
            cbSearchDept.SelectedIndexChanged += btnSearch_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(10, 15);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "搜尋姓名/帳號";
            txtSearch.Size = new Size(120, 28);
            txtSearch.TabIndex = 3;
            // 
            // dgvEmployees
            // 
            dgvEmployees.AllowUserToAddRows = false;
            dgvEmployees.Dock = DockStyle.Fill;
            dgvEmployees.Location = new Point(0, 0);
            dgvEmployees.MultiSelect = false;
            dgvEmployees.Name = "dgvEmployees";
            dgvEmployees.ReadOnly = true;
            dgvEmployees.RowHeadersVisible = false;
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployees.Size = new Size(580, 688);
            dgvEmployees.TabIndex = 0;
            dgvEmployees.CellContentClick += dgvEmployees_CellContentClick;
            dgvEmployees.SelectionChanged += dgvEmployees_SelectionChanged;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(489, 714);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 28);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "取消";
            btnCancel.Click += btnCancel_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(327, 714);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 28);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "刪除";
            btnDelete.Click += btnDelete_Click;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(408, 714);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(75, 28);
            btnEdit.TabIndex = 3;
            btnEdit.Text = "修改";
            btnEdit.Click += btnEdit_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(246, 714);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 28);
            btnAdd.TabIndex = 4;
            btnAdd.Text = "新增";
            btnAdd.Click += btnAdd_Click;
            // 
            // cbIsResigned
            // 
            cbIsResigned.Location = new Point(338, 382);
            cbIsResigned.Name = "cbIsResigned";
            cbIsResigned.Size = new Size(104, 24);
            cbIsResigned.TabIndex = 5;
            cbIsResigned.Text = "是否離職";
            // 
            // dtpLeaveDate
            // 
            dtpLeaveDate.Location = new Point(152, 381);
            dtpLeaveDate.Name = "dtpLeaveDate";
            dtpLeaveDate.Size = new Size(166, 28);
            dtpLeaveDate.TabIndex = 6;
            // 
            // label10
            // 
            label10.Location = new Point(60, 382);
            label10.Name = "label10";
            label10.Size = new Size(75, 23);
            label10.TabIndex = 7;
            label10.Text = "離職日期";
            // 
            // dtpJoinDate
            // 
            dtpJoinDate.Location = new Point(152, 319);
            dtpJoinDate.Name = "dtpJoinDate";
            dtpJoinDate.Size = new Size(166, 28);
            dtpJoinDate.TabIndex = 8;
            // 
            // label9
            // 
            label9.Location = new Point(60, 324);
            label9.Name = "label9";
            label9.Size = new Size(75, 23);
            label9.TabIndex = 9;
            label9.Text = "加入日期";
            // 
            // txtNote
            // 
            txtNote.Location = new Point(152, 441);
            txtNote.Multiline = true;
            txtNote.Name = "txtNote";
            txtNote.Size = new Size(408, 248);
            txtNote.TabIndex = 10;
            // 
            // label8
            // 
            label8.Location = new Point(92, 441);
            label8.Name = "label8";
            label8.Size = new Size(100, 23);
            label8.TabIndex = 11;
            label8.Text = "備註";
            // 
            // cbRank
            // 
            cbRank.Location = new Point(439, 200);
            cbRank.Name = "cbRank";
            cbRank.Size = new Size(121, 28);
            cbRank.TabIndex = 12;
            // 
            // label7
            // 
            label7.Location = new Point(379, 203);
            label7.Name = "label7";
            label7.Size = new Size(100, 23);
            label7.TabIndex = 13;
            label7.Text = "職位";
            // 
            // cbDept
            // 
            cbDept.Location = new Point(439, 147);
            cbDept.Name = "cbDept";
            cbDept.Size = new Size(121, 28);
            cbDept.TabIndex = 14;
            // 
            // label6
            // 
            label6.Location = new Point(379, 150);
            label6.Name = "label6";
            label6.Size = new Size(100, 23);
            label6.TabIndex = 15;
            label6.Text = "部門";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(152, 260);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(200, 28);
            txtEmail.TabIndex = 16;
            // 
            // label5
            // 
            label5.Location = new Point(92, 263);
            label5.Name = "label5";
            label5.Size = new Size(100, 23);
            label5.TabIndex = 17;
            label5.Text = "信箱";
            // 
            // txtMobile
            // 
            txtMobile.Location = new Point(152, 202);
            txtMobile.Name = "txtMobile";
            txtMobile.Size = new Size(121, 28);
            txtMobile.TabIndex = 18;
            // 
            // label4
            // 
            label4.Location = new Point(92, 205);
            label4.Name = "label4";
            label4.Size = new Size(100, 23);
            label4.TabIndex = 19;
            label4.Text = "電話";
            // 
            // dtpBirthday
            // 
            dtpBirthday.Location = new Point(152, 149);
            dtpBirthday.Name = "dtpBirthday";
            dtpBirthday.Size = new Size(166, 28);
            dtpBirthday.TabIndex = 20;
            // 
            // label3
            // 
            label3.Location = new Point(92, 152);
            label3.Name = "label3";
            label3.Size = new Size(100, 23);
            label3.TabIndex = 21;
            label3.Text = "生日";
            // 
            // cbGender
            // 
            cbGender.Items.AddRange(new object[] { "男", "女" });
            cbGender.Location = new Point(439, 91);
            cbGender.Name = "cbGender";
            cbGender.Size = new Size(65, 28);
            cbGender.TabIndex = 22;
            // 
            // labelGender
            // 
            labelGender.Location = new Point(379, 94);
            labelGender.Name = "labelGender";
            labelGender.Size = new Size(100, 23);
            labelGender.TabIndex = 23;
            labelGender.Text = "性別";
            // 
            // txtName
            // 
            txtName.Location = new Point(152, 93);
            txtName.Name = "txtName";
            txtName.Size = new Size(121, 28);
            txtName.TabIndex = 24;
            // 
            // label2
            // 
            label2.Location = new Point(92, 96);
            label2.Name = "label2";
            label2.Size = new Size(100, 23);
            label2.TabIndex = 25;
            label2.Text = "姓名";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(439, 38);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(121, 28);
            txtPassword.TabIndex = 26;
            // 
            // labelPass
            // 
            labelPass.Location = new Point(379, 41);
            labelPass.Name = "labelPass";
            labelPass.Size = new Size(100, 23);
            labelPass.TabIndex = 27;
            labelPass.Text = "密碼";
            // 
            // txtAccount
            // 
            txtAccount.Location = new Point(152, 40);
            txtAccount.Name = "txtAccount";
            txtAccount.ReadOnly = true;
            txtAccount.Size = new Size(121, 28);
            txtAccount.TabIndex = 28;
            // 
            // label1
            // 
            label1.Location = new Point(92, 43);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 29;
            label1.Text = "帳號";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1201, 800);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            Font = new Font("Microsoft JhengHei UI", 12F);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "人事管理系統 - 主畫面";
            Load += MainForm_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainerLeft.Panel1.ResumeLayout(false);
            splitContainerLeft.Panel1.PerformLayout();
            splitContainerLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerLeft).EndInit();
            splitContainerLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuSystem;
        private System.Windows.Forms.ToolStripMenuItem menuItemLogout;
        private System.Windows.Forms.ToolStripMenuItem menuManage;
        private System.Windows.Forms.ToolStripMenuItem menuItemDept;
        private System.Windows.Forms.ToolStripMenuItem menuItemRank;
        private System.Windows.Forms.ToolStripMenuItem menuItemPerm;
        private System.Windows.Forms.ToolStripMenuItem menuItemRolePerm;
        private System.Windows.Forms.SplitterPanel splitContainer1_Panel1; // Not real, just cleanup
        // Note: Designer file structure usually ends with variables.
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainerLeft;
        private System.Windows.Forms.ComboBox cbSearchRank;
        private System.Windows.Forms.ComboBox cbSearchDept;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvEmployees;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.CheckBox cbIsResigned;
        private System.Windows.Forms.DateTimePicker dtpLeaveDate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dtpJoinDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbRank;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbDept;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMobile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpBirthday;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbGender;
        private System.Windows.Forms.Label labelGender;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label labelPass;
        private System.Windows.Forms.TextBox txtAccount;
        private System.Windows.Forms.Label label1;
    }
}
