using HrmsDemo.Helpers;
using HrmsDemo.Models;
using HrmsDemo.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Dapper;

namespace HrmsDemo
{
    public partial class RolePermForm : Form
    {
        private ComboBox cbDept;
        private ComboBox cbRank;
        private CheckedListBox clbPerms;
        private Button btnSave;
        private MetaRepository _metaRepo;

        public RolePermForm()
        {
            InitializeComponent();
            _metaRepo = new MetaRepository();
            LoadMetaData();
        }

        private void InitializeComponent()
        {
            this.cbDept = new System.Windows.Forms.ComboBox();
            this.cbRank = new System.Windows.Forms.ComboBox();
            this.clbPerms = new System.Windows.Forms.CheckedListBox();
            this.btnSave = new System.Windows.Forms.Button();
            var lbl1 = new Label();
            var lbl2 = new Label();
            
            lbl1.Text = "部門"; lbl1.Location = new System.Drawing.Point(20, 20);
            cbDept.Location = new System.Drawing.Point(20, 40); cbDept.Size = new System.Drawing.Size(120, 23);
            cbDept.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDept.SelectedIndexChanged += new EventHandler(this.SelectionChanged);

            lbl2.Text = "職位"; lbl2.Location = new System.Drawing.Point(160, 20);
            cbRank.Location = new System.Drawing.Point(160, 40); cbRank.Size = new System.Drawing.Size(120, 23);
            cbRank.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRank.SelectedIndexChanged += new EventHandler(this.SelectionChanged);
            
            clbPerms.Location = new System.Drawing.Point(20, 80);
            clbPerms.Size = new System.Drawing.Size(300, 300);
            clbPerms.CheckOnClick = true;
            
            btnSave.Location = new System.Drawing.Point(20, 400);
            btnSave.Text = "儲存設定";
            btnSave.Click += new EventHandler(this.btnSave_Click);
            
            this.Controls.Add(lbl1);
            this.Controls.Add(cbDept);
            this.Controls.Add(lbl2);
            this.Controls.Add(cbRank);
            this.Controls.Add(clbPerms);
            this.Controls.Add(btnSave);
            
            this.ClientSize = new System.Drawing.Size(350, 450);
            this.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Text = "角色權限管理";
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void LoadMetaData()
        {
            cbDept.DisplayMember = "Name"; cbDept.ValueMember = "ID";
            cbDept.DataSource = _metaRepo.GetDepartments();
            
            cbRank.DisplayMember = "Name"; cbRank.ValueMember = "ID";
            cbRank.DataSource = _metaRepo.GetRanks();
            
            var perms = _metaRepo.GetPermissions();
            foreach(var p in perms)
            {
                clbPerms.Items.Add(p); // We'll need formatting
            }
            ((ListBox)clbPerms).DisplayMember = "Description"; // Assuming Model has Description
            
            // Check first role
            SelectionChanged(null, null);
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            if (cbDept.SelectedValue == null || cbRank.SelectedValue == null) return;
            if (!(cbDept.SelectedValue is int) || !(cbRank.SelectedValue is int)) return; // Safety check
            
            int deptId = (int)cbDept.SelectedValue;
            int rankId = (int)cbRank.SelectedValue;
            
            // Load current perms
            List<string> currentPerms = new AuthRepository().GetUserPermissions(deptId, rankId);
            
            for (int i = 0; i < clbPerms.Items.Count; i++)
            {
                var p = (Permission)clbPerms.Items[i];
                clbPerms.SetItemChecked(i, currentPerms.Contains(p.PermCode));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbDept.SelectedValue == null || cbRank.SelectedValue == null) return;
            int deptId = (int)cbDept.SelectedValue;
            int rankId = (int)cbRank.SelectedValue;
            
            try 
            {
                using(var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using(var trans = conn.BeginTransaction())
                    {
                        // Delete all old
                        conn.Execute("DELETE FROM RolePermissions WHERE DeptID=@D AND RankID=@R", 
                            new { D = deptId, R = rankId }, trans);
                            
                        // Insert new
                        foreach(Permission p in clbPerms.CheckedItems)
                        {
                            conn.Execute("INSERT INTO RolePermissions VALUES (@D, @R, @P)", 
                                new { D = deptId, R = rankId, P = p.PermCode }, trans);
                        }
                        trans.Commit();
                    }
                }
                MessageBox.Show("權限更新成功");
            }
            catch(Exception ex)
            {
                MessageBox.Show("錯誤: " + ex.Message);
            }
        }
    }
}
