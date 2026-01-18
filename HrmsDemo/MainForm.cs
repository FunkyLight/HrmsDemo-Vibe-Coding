using HrmsDemo.Models;
using HrmsDemo.Repositories;
using HrmsDemo.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace HrmsDemo
{
    public partial class MainForm : Form
    {
        private EmployeeRepository _empRepo;
        private MetaRepository _metaRepo;
        private List<Employee> _allEmployees;
        private BindingSource _bindingSource;
        private bool _isEditMode = false;
        private bool _isAddNew = false;
        private Employee _originalData;
        private int _currentEmployeeId = 0;

        // Virtual Mode Fields
        private int _totalRecords = 0;
        private int _pageSize = 20;
        private Dictionary<int, Employee> _dataCache = new Dictionary<int, Employee>();
        private HashSet<int> _loadingPages = new HashSet<int>(); // Track which pages are being fetched
        private int? _filterDept = null;
        private int? _filterRank = null;
        private string _filterSearch = "";

        public MainForm()
        {
            InitializeComponent();
            _empRepo = new EmployeeRepository();
            _metaRepo = new MetaRepository();
            _bindingSource = new BindingSource();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetupVirtualColumns();
            LoadMetaData();
            LoadEmployees();
            ApplyPermissions();

            btnCancel.Enabled = false; // Initial State

            // Wire up Change Detection Events
            var controls = new Control[] { txtAccount, txtPassword, txtName, txtMobile, txtEmail, txtNote };
            foreach (var c in controls) c.TextChanged += OnInputChanged;

            var combos = new ComboBox[] { cbGender, cbDept, cbRank };
            foreach (var c in combos) c.SelectedIndexChanged += OnInputChanged;

            dtpBirthday.ValueChanged += OnInputChanged;
            dtpJoinDate.ValueChanged += OnInputChanged;
            dtpLeaveDate.ValueChanged += OnInputChanged;
            cbIsResigned.CheckedChanged += OnInputChanged;
        }

        private void LoadMetaData()
        {
            var depts = _metaRepo.GetDepartments().ToList();
            var ranks = _metaRepo.GetRanks().ToList();

            // Setup ComboBoxes
            cbDept.DisplayMember = "Name";
            cbDept.ValueMember = "ID";
            cbDept.DataSource = new List<Department>(depts);

            cbRank.DisplayMember = "Name";
            cbRank.ValueMember = "ID";
            cbRank.DataSource = new List<Rank>(ranks);

            cbGender.SelectedIndex = 0; // Default Male

            // Search ComboBoxes
            var searchDepts = new List<Department>(depts);
            searchDepts.Insert(0, new Department { ID = 0, Name = "全部門" });

            cbSearchDept.DisplayMember = "Name";
            cbSearchDept.ValueMember = "ID";
            cbSearchDept.DataSource = searchDepts;

            var searchRanks = new List<Rank>(ranks);
            searchRanks.Insert(0, new Rank { ID = 0, Name = "全職位" });

            cbSearchRank.DisplayMember = "Name";
            cbSearchRank.ValueMember = "ID";
            cbSearchRank.DataSource = searchRanks;
        }



        private void LoadEmployees()
        {
            // Configure Virtual Mode
            dgvEmployees.VirtualMode = true;
            dgvEmployees.CellValueNeeded -= DgvEmployees_CellValueNeeded; // Prevent double subscription
            dgvEmployees.CellValueNeeded += DgvEmployees_CellValueNeeded;

            _filterSearch = txtSearch.Text.Trim();
            _filterDept = null; _filterRank = null;
            if (cbSearchDept.SelectedIndex > 0) _filterDept = (int)cbSearchDept.SelectedValue;
            if (cbSearchRank.SelectedIndex > 0) _filterRank = (int)cbSearchRank.SelectedValue;

            bool viewAll = AuthService.HasPermission("VIEW_ALL");
            bool viewDept = AuthService.HasPermission("VIEW_DEPT");
            bool viewSelf = AuthService.HasPermission("VIEW_SELF");

            if (!viewAll)
            {
                if (viewDept)
                {
                    // Restrict to Own Dept
                    _filterDept = AuthService.CurrentUser.DeptID;
                    // Disable Search Dept Combo if locked
                    cbSearchDept.Enabled = false;
                    cbSearchDept.SelectedValue = AuthService.CurrentUser.DeptID;
                }
            }

            // 1. Get Count
            if (!viewAll && !viewDept && viewSelf)
            {
                // Special Case: View Only Self
                _totalRecords = 1;
                // Pre-load self into cache index 0
                _dataCache.Clear();
                var self = _empRepo.GetAll("", null, null).FirstOrDefault(x => x.ID == AuthService.CurrentUser.ID);
                if (self != null) _dataCache[0] = self;
            }
            else
            {
                _totalRecords = _empRepo.GetTotalCount(_filterSearch, _filterDept, _filterRank);
                _dataCache.Clear();
                _loadingPages.Clear();
            }

            dgvEmployees.RowCount = 0; // Reset to trigger refresh
            dgvEmployees.RowCount = _totalRecords;
            dgvEmployees.Invalidate();
        }

        private void DgvEmployees_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= _totalRecords) return;

            // Check Cache
            if (_dataCache.ContainsKey(e.RowIndex))
            {
                var emp = _dataCache[e.RowIndex];
                if (emp == null) return;

                switch (dgvEmployees.Columns[e.ColumnIndex].Name)
                {
                    case "ID": e.Value = emp.ID; break;
                    case "Name": e.Value = emp.Name; break;
                    case "Account": e.Value = emp.Account; break;
                    case "Gender": e.Value = emp.Gender; break;
                    case "Mobile": e.Value = emp.Mobile; break;
                    case "Email": e.Value = emp.Email; break;
                        // Add other columns as needed for display
                }
            }
            else
            {
                // Not in cache, trigger load
                if (!_loadingPages.Contains(GetPage(e.RowIndex)))
                {
                    LoadPageAsync(GetPage(e.RowIndex));
                }
                e.Value = "..."; // Placeholder
            }
        }

        private int GetPage(int rowIndex) => rowIndex / _pageSize;

        private async void LoadPageAsync(int pageIndex)
        {
            if (_loadingPages.Contains(pageIndex)) return;
            _loadingPages.Add(pageIndex);

            int offset = pageIndex * _pageSize;

            try
            {
                // ASYNC Fetch
                var data = await System.Threading.Tasks.Task.Run(() =>
                {
                    return _empRepo.GetPaged(_filterSearch, _filterDept, _filterRank, offset, _pageSize).ToList();
                });

                // Update Cache (UI Thread)
                for (int i = 0; i < data.Count; i++)
                {
                    int rowIndex = offset + i;
                    if (!_dataCache.ContainsKey(rowIndex))
                        _dataCache.Add(rowIndex, data[i]);
                }

                // Cleanup Old Cache (Simple Window: Keep current, prev, next)
                // Remove pages distant from current (heuristic)
                var keysToRemove = _dataCache.Keys.Where(k => Math.Abs(GetPage(k) - pageIndex) > 2).ToList();
                foreach (var k in keysToRemove) _dataCache.Remove(k);

                // Refresh Grid
                if (!dgvEmployees.IsDisposed)
                    dgvEmployees.Invalidate();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                _loadingPages.Remove(pageIndex);
            }
        }

        private void SetupVirtualColumns()
        {
            dgvEmployees.Columns.Clear();
            dgvEmployees.Columns.Add("ID", "ID");
            dgvEmployees.Columns.Add("Account", "帳號");
            dgvEmployees.Columns.Add("Name", "姓名");
            dgvEmployees.Columns.Add("Gender", "性別");
            dgvEmployees.Columns.Add("Mobile", "電話");
            dgvEmployees.Columns.Add("Email", "信箱");
        }

        private void ApplyPermissions()
        {
            // Set Button Visibility
            btnAdd.Visible = AuthService.HasPermission("ADD_USER");
            btnDelete.Visible = AuthService.HasPermission("DELETE_USER");
            // Edit button is tricky, it toggles. Initially visible if EDIT_USER or WRITE_NOTE (partial edit) or SET_RESIGNED?
            // Requirement says button is "修改" (Edit).
            // We'll show it if any edit perm exists.
            bool canEdit = AuthService.HasPermission("EDIT_USER") || AuthService.HasPermission("WRITE_NOTE") || AuthService.HasPermission("SET_RESIGNED");
            btnEdit.Visible = canEdit;

            SetDetailReadOnly(true);
        }

        private void SetDetailReadOnly(bool readOnly)
        {
            // Always ReadOnly: Account (Requirement 8: 帳號(ReadOnly)) -> But for ADD it might be editable? 
            // Req 3: "新增 ... 與 Transaction 方式 INSERT". Normally Account is input. 
            // "右半邊詳細資料欄位變為空白，填入資料後點擊儲存". So for Add, Account is Editable. For Edit, Account is ReadOnly?
            // Usually Account shouldn't change. Let's assume Account is Editable ONLY when Adding.

            txtAccount.ReadOnly = _isAddNew ? false : true;

            txtPassword.ReadOnly = readOnly;
            txtName.ReadOnly = readOnly;
            txtMobile.ReadOnly = readOnly;
            txtEmail.ReadOnly = readOnly;
            txtNote.ReadOnly = readOnly;

            cbGender.Enabled = !readOnly;
            cbDept.Enabled = !readOnly;
            cbRank.Enabled = !readOnly;
            dtpBirthday.Enabled = !readOnly;
            dtpJoinDate.Enabled = !readOnly;
            dtpLeaveDate.Enabled = !readOnly;
            cbIsResigned.Enabled = !readOnly;

            // Special permission: WRITE_NOTE only
            if (!readOnly && !AuthService.HasPermission("EDIT_USER") && AuthService.HasPermission("WRITE_NOTE"))
            {
                // Lock everything except Note
                txtPassword.ReadOnly = true;
                txtName.ReadOnly = true;
                txtMobile.ReadOnly = true;
                txtEmail.ReadOnly = true;
                cbGender.Enabled = false;
                cbDept.Enabled = false;
                cbRank.Enabled = false;
                dtpBirthday.Enabled = false;
                dtpJoinDate.Enabled = false;
                dtpLeaveDate.Enabled = false;
                cbIsResigned.Enabled = false;

                txtNote.ReadOnly = false;
            }

            // Lock Grid during Edit to prevent losing changes
            dgvEmployees.Enabled = readOnly;
        }

        private void dgvEmployees_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                int index = dgvEmployees.SelectedRows[0].Index;
                if (_dataCache.ContainsKey(index))
                {
                    BindDetail(_dataCache[index]);
                }
            }
        }

        private void BindDetail(Employee emp)
        {
            _currentEmployeeId = emp.ID;
            txtAccount.Text = emp.Account;
            txtPassword.Text = emp.Password;
            txtName.Text = emp.Name;
            cbGender.SelectedItem = emp.Gender;
            txtMobile.Text = emp.Mobile;
            txtEmail.Text = emp.Email;
            txtNote.Text = emp.Note;

            if (emp.Birthday.HasValue) dtpBirthday.Value = emp.Birthday.Value;
            if (emp.JoinDate.HasValue) dtpJoinDate.Value = emp.JoinDate.Value;
            if (emp.LeaveDate.HasValue)
            {
                dtpLeaveDate.Value = emp.LeaveDate.Value;
                dtpLeaveDate.Checked = true;
            }
            else
            {
                dtpLeaveDate.Checked = false;
            }

            cbIsResigned.Checked = !emp.IsEmployed;

            cbDept.SelectedValue = emp.DeptID;
            cbRank.SelectedValue = emp.RankID;
        }

        // ================= MENUS =================
        private void menuItemDept_Click(object sender, EventArgs e)
        {
            if (AuthService.HasPermission("ADD_DEPT") || AuthService.HasPermission("EDIT_DEPT") || AuthService.HasPermission("DELETE_DEPT"))
            {
                new DeptForm().ShowDialog();
                LoadMetaData();
            }
            else
            {
                MessageBox.Show("權限不足");
            }
        }

        private void menuItemRank_Click(object sender, EventArgs e)
        {
            if (AuthService.HasPermission("ADD_RANK") || AuthService.HasPermission("EDIT_RANK") || AuthService.HasPermission("DELETE_RANK"))
            {
                new RankForm().ShowDialog();
                LoadMetaData();
            }
            else
            {
                MessageBox.Show("權限不足");
            }
        }

        private void menuItemPerm_Click(object sender, EventArgs e)
        {
            MessageBox.Show("權限代碼為系統固定，僅可修改描述 (本 Demo 省略)");
        }

        private void menuItemRolePerm_Click(object sender, EventArgs e)
        {
            if (AuthService.HasPermission("ADD_ROLE_PERM") || AuthService.HasPermission("EDIT_ROLE_PERM"))
            {
                new RolePermForm().ShowDialog();
            }
            else
            {
                MessageBox.Show("權限不足");
            }
        }

        // ================= BUTTONS =================

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _isAddNew = true;
            _isEditMode = true;

            // Clear Fields
            _currentEmployeeId = 0;
            txtAccount.Text = "";
            txtPassword.Text = "";
            txtName.Text = "";
            txtMobile.Text = "";
            txtEmail.Text = "";
            txtNote.Text = "";
            cbIsResigned.Checked = false;
            dtpLeaveDate.Checked = false;

            SetDetailReadOnly(false);

            // Capture State
            _originalData = GetEmployeeFromUI();

            btnAdd.Enabled = false;
            btnEdit.Text = "儲存";
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnCancel.Enabled = true; // Enable Cancel
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // If in View mode, switch to Edit mode
            if (!_isEditMode)
            {
                _isEditMode = true;
                _isAddNew = false;

                SetDetailReadOnly(false);

                // Capture State
                _originalData = GetEmployeeFromUI();

                btnEdit.Text = "儲存";
                btnEdit.Enabled = false;
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = true; // Enable Cancel
            }
            else
            {
                // SAVE Action
                SaveEmployee();
            }
        }

        private void SaveEmployee()
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtAccount.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("帳號與姓名為必填", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Construct Object
            var emp = GetEmployeeFromUI();

            try
            {
                if (_isAddNew)
                {
                    _empRepo.Add(emp);
                }
                else
                {
                    _empRepo.Update(emp);
                }

                MessageBox.Show("儲存成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reset State
                CancelEditState();
                LoadEmployees(); // Refresh list
            }
            catch (Exception ex)
            {
                MessageBox.Show("儲存失敗: " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_isEditMode)
            {
                // If no changes, cancel immediately without prompt
                if (!HasChanges())
                {
                    CancelEditState();
                    // Re-bind original selection
                    // Re-bind original selection
                    if (dgvEmployees.SelectedRows.Count > 0)
                    {
                        int index = dgvEmployees.SelectedRows[0].Index;
                        if (_dataCache.ContainsKey(index)) BindDetail(_dataCache[index]);
                    }
                    return;
                }

                // Confirm
                string action = _isAddNew ? "新增" : "修改";
                if (MessageBox.Show($"是否確定取消{action}", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CancelEditState();
                    // Re-bind original selection
                    if (dgvEmployees.SelectedRows.Count > 0)
                    {
                        int index = dgvEmployees.SelectedRows[0].Index;
                        if (_dataCache.ContainsKey(index)) BindDetail(_dataCache[index]);
                    }
                }
            }
        }

        private void CancelEditState()
        {
            _isEditMode = false;
            _isAddNew = false;

            SetDetailReadOnly(true);

            btnEdit.Text = "修改";
            btnEdit.Enabled = true;
            btnAdd.Enabled = AuthService.HasPermission("ADD_USER");
            btnDelete.Enabled = AuthService.HasPermission("DELETE_USER");
            btnCancel.Enabled = false; // Disable Cancel
        }

        private void OnInputChanged(object sender, EventArgs e)
        {
            if (_isEditMode)
            {
                btnEdit.Enabled = HasChanges();
            }
        }

        private bool HasChanges()
        {
            if (_originalData == null) return true;
            var current = GetEmployeeFromUI();

            // Simple comparison of relevant fields
            if (current.Account != _originalData.Account) return true;
            if (current.Password != _originalData.Password) return true;
            if (current.Name != _originalData.Name) return true;
            if (current.Gender != _originalData.Gender) return true;
            if (current.Birthday != _originalData.Birthday) return true;
            if (current.Mobile != _originalData.Mobile) return true;
            if (current.Email != _originalData.Email) return true;
            if (current.Note != _originalData.Note) return true;
            if (current.DeptID != _originalData.DeptID) return true;
            if (current.RankID != _originalData.RankID) return true;
            if (current.JoinDate != _originalData.JoinDate) return true;
            if (current.LeaveDate.HasValue != _originalData.LeaveDate.HasValue) return true;
            if (current.LeaveDate.HasValue && current.LeaveDate.Value != _originalData.LeaveDate.Value) return true;
            if (current.IsEmployed != _originalData.IsEmployed) return true;

            return false;
        }

        private Employee GetEmployeeFromUI()
        {
            int id = _currentEmployeeId;

            var emp = new Employee
            {
                ID = id,
                Account = txtAccount.Text,
                Password = txtPassword.Text,
                Name = txtName.Text,
                Gender = cbGender.SelectedItem?.ToString() ?? "男",
                Birthday = dtpBirthday.Value,
                Mobile = txtMobile.Text,
                Email = txtEmail.Text,
                Note = txtNote.Text,
                DeptID = cbDept.SelectedValue != null ? (int)cbDept.SelectedValue : 0,
                RankID = cbRank.SelectedValue != null ? (int)cbRank.SelectedValue : 0,
                JoinDate = dtpJoinDate.Value,
                IsEmployed = !cbIsResigned.Checked
            };

            if (dtpLeaveDate.Checked)
                emp.LeaveDate = dtpLeaveDate.Value;
            else
                emp.LeaveDate = null;

            return emp;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否確定刪除", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int id = _currentEmployeeId;
                try
                {
                    _empRepo.Delete(id);
                    LoadEmployees();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("刪除失敗: " + ex.Message);
                }
            }
        }

        private void menuItemLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否確定登出", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK; // Signal to Program/Login that we logged out
                this.Close();
            }
        }

        private void panelDetail_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
