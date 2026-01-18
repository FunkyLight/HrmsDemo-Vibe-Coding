using HrmsDemo.Services;
using System;
using System.Windows.Forms;

namespace HrmsDemo
{
    public partial class LoginForm : Form
    {
        private AuthService _authService;

        public LoginForm()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Load last used account from Settings if available
            // Since we didn't setup Settings.settings, we can use a local file or just skip for now,
            // BUT req says "登入畫面輸入框保留上一個登入者的account"
            // Let's use a simple static way or file for demo. 
            // For simple demo, using Properties.Settings is best but need to ensure it's generated.
            // I'll assume Properties.Settings exists or use a simple file.
            // Let's retry a simple file approach to be safe and dependent-less.
            try
            {
                if (System.IO.File.Exists("lastuser.txt"))
                {
                    txtAccount.Text = System.IO.File.ReadAllText("lastuser.txt");
                    this.ActiveControl = txtPassword;
                }
            }
            catch { }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string account = txtAccount.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("請輸入帳號與密碼", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string message;
                bool success = _authService.Login(account, password, out message);

                if (success)
                {
                    // Save Last User
                    System.IO.File.WriteAllText("lastuser.txt", account);

                    // Open Main Form
                    this.Hide();
                    MainForm mainForm = new MainForm();
                    // mainForm.FormClosed += (s, args) => this.Close(); // Remove this line 
                    // Note: If Main form logs out, it should probably just show this form again.
                    // The requirements say: "登入後進入主畫面，登出後返回登入畫面"
                    // So we should handle that in Program.cs or here.
                    // Better approach: Hide Login, Show Main. When Main closes, check if it was a logout or exit.
                    
                    var result = mainForm.ShowDialog();
                    if (result == DialogResult.OK) // OK means Logout
                    {
                        this.Show();
                        txtPassword.Text = "";
                        txtAccount.Focus();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show(message, "登入失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("資料庫連線失敗: " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
