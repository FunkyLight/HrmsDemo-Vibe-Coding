using Dapper;
using HrmsDemo.Helpers;
using HrmsDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HrmsDemo.Repositories
{
    public class MetaRepository
    {
        public IEnumerable<Department> GetDepartments()
        {
            using (var conn = DbHelper.GetConnection())
            {
                return conn.Query<Department>("SELECT * FROM Departments");
            }
        }

        public IEnumerable<Rank> GetRanks()
        {
            using (var conn = DbHelper.GetConnection())
            {
                return conn.Query<Rank>("SELECT * FROM Ranks");
            }
        }

        public IEnumerable<Permission> GetPermissions()
        {
            using (var conn = DbHelper.GetConnection())
            {
                return conn.Query<Permission>("SELECT * FROM Permissions");
            }
        }
    }

    public class AuthRepository
    {
        public Employee Login(string account, string password)
        {
            using (var conn = DbHelper.GetConnection())
            {
                string sql = @"
                    SELECT e.*, d.Name as DeptName, r.Name as RankName 
                    FROM Employees e
                    LEFT JOIN Departments d ON e.DeptID = d.ID
                    LEFT JOIN Ranks r ON e.RankID = r.ID
                    WHERE Account = @Account AND Password = @Password AND IsEmployed = 1";
                return conn.QueryFirstOrDefault<Employee>(sql, new { Account = account, Password = password });
            }
        }

        public List<string> GetUserPermissions(int deptId, int rankId)
        {
            using (var conn = DbHelper.GetConnection())
            {
                string sql = "SELECT PermCode FROM RolePermissions WHERE DeptID = @DeptID AND RankID = @RankID";
                return conn.Query<string>(sql, new { DeptID = deptId, RankID = rankId }).ToList();
            }
        }
    }

    public class EmployeeRepository
    {
        public IEnumerable<Employee> GetAll(string searchTerm = null, int? deptId = null, int? rankId = null)
        {
            using (var conn = DbHelper.GetConnection())
            {
                var sql = "SELECT e.*, d.Name as DeptName, r.Name as RankName FROM Employees e JOIN Departments d ON e.DeptID = d.ID JOIN Ranks r ON e.RankID = r.ID WHERE 1=1";
                var parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    sql += " AND (e.Name LIKE @Search OR e.Account LIKE @Search OR e.Mobile LIKE @Search)";
                    parameters.Add("Search", "%" + searchTerm + "%");
                }
                if (deptId.HasValue)
                {
                    sql += " AND e.DeptID = @DeptID";
                    parameters.Add("DeptID", deptId.Value);
                }
                if (rankId.HasValue)
                {
                    sql += " AND e.RankID = @RankID";
                    parameters.Add("RankID", rankId.Value);
                }

                sql += " ORDER BY e.ID DESC"; // Default sort
                return conn.Query<Employee>(sql, parameters);
            }
        }

        public Employee GetById(int id)
        {
            using (var conn = DbHelper.GetConnection())
            {
                string sql = "SELECT * FROM Employees WHERE ID = @ID";
                return conn.QueryFirstOrDefault<Employee>(sql, new { ID = id });
            }
        }

        public void Add(Employee emp)
        {
            using (var conn = DbHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = @"INSERT INTO Employees (Account, Password, Name, Gender, Birthday, Email, Mobile, JoinDate, LeaveDate, IsEmployed, Note, DeptID, RankID, CreateDate) 
                                       VALUES (@Account, @Password, @Name, @Gender, @Birthday, @Email, @Mobile, @JoinDate, @LeaveDate, @IsEmployed, @Note, @DeptID, @RankID, NOW())";
                        conn.Execute(sql, emp, trans);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(Employee emp)
        {
            using (var conn = DbHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = @"UPDATE Employees SET 
                                        Password = @Password,
                                        Name = @Name,
                                        Gender = @Gender,
                                        Birthday = @Birthday,
                                        Email = @Email,
                                        Mobile = @Mobile,
                                        JoinDate = @JoinDate,
                                        LeaveDate = @LeaveDate,
                                        IsEmployed = @IsEmployed,
                                        Note = @Note,
                                        DeptID = @DeptID,
                                        RankID = @RankID
                                       WHERE ID = @ID";
                        conn.Execute(sql, emp, trans);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = DbHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = "DELETE FROM Employees WHERE ID = @ID";
                        conn.Execute(sql, new { ID = id }, trans);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        public int GetTotalCount(string search, int? deptId, int? rankId)
        {
            using (var conn = DbHelper.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Employees WHERE 1=1";
                
                if (!string.IsNullOrEmpty(search))
                {
                    sql += " AND (Name LIKE @Search OR Account LIKE @Search)";
                }
                if (deptId.HasValue)
                {
                    sql += " AND DeptID = @DeptID";
                }
                if (rankId.HasValue)
                {
                    sql += " AND RankID = @RankID";
                }

                return conn.ExecuteScalar<int>(sql, new { Search = "%" + search + "%", DeptID = deptId, RankID = rankId });
            }
        }

        public IEnumerable<Employee> GetPaged(string search, int? deptId, int? rankId, int offset, int limit)
        {
            using (var conn = DbHelper.GetConnection())
            {
                string sql = @"
                    SELECT e.*, d.Name as DeptName, r.Name as RankName 
                    FROM Employees e
                    LEFT JOIN Departments d ON e.DeptID = d.ID
                    LEFT JOIN Ranks r ON e.RankID = r.ID
                    WHERE 1=1";

                if (!string.IsNullOrEmpty(search))
                {
                    sql += " AND (e.Name LIKE @Search OR e.Account LIKE @Search)";
                }
                if (deptId.HasValue)
                {
                    sql += " AND e.DeptID = @DeptID";
                }
                if (rankId.HasValue)
                {
                    sql += " AND e.RankID = @RankID";
                }

                sql += " ORDER BY e.ID DESC LIMIT @Limit OFFSET @Offset";

                return conn.Query<Employee>(sql, new { 
                    Search = "%" + search + "%", 
                    DeptID = deptId, 
                    RankID = rankId,
                    Limit = limit,
                    Offset = offset
                });
            }
        }
    }
}
