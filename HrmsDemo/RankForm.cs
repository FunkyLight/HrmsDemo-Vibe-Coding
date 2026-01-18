using HrmsDemo.Models;
using HrmsDemo.Repositories;
using HrmsDemo.Helpers;
using System;
using System.Windows.Forms;
using Dapper;

namespace HrmsDemo
{
    public partial class RankForm : Form
    {
        private TextBox txtName;
        private TextBox txtLevel;
        private Button btnAdd;
        private Button btnDelete;
        private ListBox lstRanks;
        private Label label1;
        private Label label2;
        
        public RankForm()
        {
            InitializeComponent();
            LoadRanks();
        }

        private void InitializeComponent()
        {
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtLevel = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lstRanks = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            
            this.lstRanks.FormattingEnabled = true;
            this.lstRanks.ItemHeight = 15;
            this.lstRanks.Location = new System.Drawing.Point(12, 12);
            this.lstRanks.Size = new System.Drawing.Size(200, 229);
            this.lstRanks.DisplayMember = "Name";
            this.lstRanks.ValueMember = "ID";
            
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(230, 20);
            this.label1.Text = "職位名稱";
            
            this.txtName.Location = new System.Drawing.Point(230, 40);
            this.txtName.Size = new System.Drawing.Size(150, 23);
            
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(230, 70);
            this.label2.Text = "職等 (數字)";
            
            this.txtLevel.Location = new System.Drawing.Point(230, 90);
            this.txtLevel.Size = new System.Drawing.Size(150, 23);
            
            this.btnAdd.Location = new System.Drawing.Point(230, 130);
            this.btnAdd.Size = new System.Drawing.Size(70, 25);
            this.btnAdd.Text = "新增";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            
            this.btnDelete.Location = new System.Drawing.Point(310, 130);
            this.btnDelete.Size = new System.Drawing.Size(70, 25);
            this.btnDelete.Text = "刪除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            
            this.ClientSize = new System.Drawing.Size(400, 260);
            this.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtLevel);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstRanks);
            this.Name = "RankForm";
            this.Text = "職位管理";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadRanks()
        {
            var repo = new MetaRepository();
            lstRanks.DataSource = repo.GetRanks();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name)) return;
            if (!int.TryParse(txtLevel.Text, out int level)) { MessageBox.Show("職等請輸入數字"); return; }
            
            try 
            {
                using(var conn = DbHelper.GetConnection())
                {
                    string sql = "INSERT INTO Ranks (Name, RankLevel) VALUES (@Name, @RankLevel)";
                    conn.Execute(sql, new { Name = name, RankLevel = level });
                    LoadRanks();
                    txtName.Text = "";
                    txtLevel.Text = "";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
             if (lstRanks.SelectedItem != null)
             {
                 try 
                 {
                     int id = ((Rank)lstRanks.SelectedItem).ID;
                     using(var conn = DbHelper.GetConnection())
                     {
                         conn.Execute("DELETE FROM Ranks WHERE ID = @ID", new { ID = id });
                     }
                     LoadRanks();
                 }
                 catch
                 {
                     MessageBox.Show("刪除失敗，該職位可能還有員工");
                 }
             }
        }
    }
}
