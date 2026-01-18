using System;

namespace HrmsDemo.Models
{
    public class Department
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Rank
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int RankLevel { get; set; }
    }

    public class Permission
    {
        public string PermCode { get; set; }
        public string Description { get; set; }
    }

    public class RolePermission
    {
        public int DeptID { get; set; }
        public int RankID { get; set; }
        public string PermCode { get; set; }
    }

    public class Employee
    {
        public int ID { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public bool IsEmployed { get; set; }
        public string Note { get; set; }
        public int DeptID { get; set; }
        public int RankID { get; set; }
        public DateTime CreateDate { get; set; }

        // Joined Properties (Extra properties for display if needed)
        public string DeptName { get; set; }
        public string RankName { get; set; }
    }
}
