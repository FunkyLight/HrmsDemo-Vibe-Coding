using HrmsDemo.Models;
using HrmsDemo.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace HrmsDemo.Services
{
    public class AuthService
    {
        private readonly AuthRepository _authRepo;
        public static Employee CurrentUser { get; private set; }
        public static List<string> CurrentPermissions { get; private set; } = new List<string>();

        public AuthService()
        {
            _authRepo = new AuthRepository();
        }

        public bool Login(string account, string password, out string message)
        {
            var user = _authRepo.Login(account, password);
            if (user == null)
            {
                message = "帳號或密碼錯誤";
                return false;
            }

            if (!user.IsEmployed)
            {
                message = "該帳號已停用 (離職)";
                return false;
            }

            CurrentUser = user;
            CurrentPermissions = _authRepo.GetUserPermissions(user.DeptID, user.RankID);
            
            message = "登入成功";
            return true;
        }

        public void Logout()
        {
            CurrentUser = null;
            CurrentPermissions.Clear();
        }

        public static bool HasPermission(string permCode)
        {
            return CurrentPermissions.Contains(permCode);
        }
    }
}
