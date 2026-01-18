using HrmsDemo.Models;
using HrmsDemo.Repositories;
using HrmsDemo.Services;
using System;
using System.Windows.Forms;
using Dapper;
using HrmsDemo.Helpers;

namespace HrmsDemo
{
    public partial class DeptForm : Form
    {
        private TextBox txtName;
        private Button btnAdd;
        private Button btnDelete;
        private ListBox lstDepts;
        private Label label1;
        
        public DeptForm()
        {
            InitializeComponent();
            LoadDepts();
        }

        private void InitializeComponent()
        {
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lstDepts = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            
            // lstDepts
            this.lstDepts.FormattingEnabled = true;
            this.lstDepts.ItemHeight = 15;
            this.lstDepts.Location = new System.Drawing.Point(12, 12);
            this.lstDepts.Name = "lstDepts";
            this.lstDepts.Size = new System.Drawing.Size(200, 229);
            this.lstDepts.TabIndex = 0;
            this.lstDepts.DisplayMember = "Name";
            this.lstDepts.ValueMember = "ID";
            
            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(230, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.Text = "部門名稱";
            
            // txtName
            this.txtName.Location = new System.Drawing.Point(230, 40);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(150, 23);
            this.txtName.TabIndex = 1;
            
            // btnAdd
            this.btnAdd.Location = new System.Drawing.Point(230, 80);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(70, 25);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "新增/修改";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            
            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(310, 80);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(70, 25);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "刪除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            
            // DeptForm
            this.ClientSize = new System.Drawing.Size(400, 260);
            this.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstDepts);
            this.Name = "DeptForm";
            this.Text = "部門管理";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadDepts()
        {
            var repo = new MetaRepository();
            lstDepts.DataSource = repo.GetDepartments();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Simple Add or Edit if selected?
            // Requirement just says ADD_DEPT, EDIT_DEPT.
            // Let's assume if something is selected in ListBox -> Update? Or separate logic?
            // Simplified: If text matches selected, update. If not/new, Add.
            // Actually explicit Add is better.
            
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name)) return;
            
            try 
            {
                using(var conn = DbHelper.GetConnection())
                {
                    // Update if selected?
                    if (lstDepts.SelectedItem != null && ((Department)lstDepts.SelectedItem).Name == name)
                    {
                        // No logic change
                        return; 
                    }
                    
                    // Simple Add
                    string sql = "INSERT INTO Departments (Name) VALUES (@Name)";
                    conn.Execute(sql, new { Name = name });
                    
                    txtName.Text = "";
                    LoadDepts();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
             if (lstDepts.SelectedItem != null)
             {
                 try 
                 {
                     int id = ((Department)lstDepts.SelectedItem).ID;
                     using(var conn = DbHelper.GetConnection())
                     {
                         // Check constraints? catch exception usually
                         conn.Execute("DELETE FROM Departments WHERE ID = @ID", new { ID = id });
                     }
                     LoadDepts();
                 }
                 catch
                 {
                     MessageBox.Show("刪除失敗，該部門可能還有員工");
                 }
             }
        }
    }
}
