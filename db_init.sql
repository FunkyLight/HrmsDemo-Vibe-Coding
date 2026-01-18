-- Create Database
CREATE DATABASE IF NOT EXISTS HrmsDB;
USE HrmsDB;

-- Disable foreign key checks for bulk operations
SET FOREIGN_KEY_CHECKS = 0;

-- 1. Departments
DROP TABLE IF EXISTS Departments;
CREATE TABLE Departments (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL
);

-- 2. Ranks
DROP TABLE IF EXISTS Ranks;
CREATE TABLE Ranks (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    RankLevel INT NOT NULL -- Lower number might mean higher rank, or vice versa. Let's assume 0 is System/Admin, 1 is Manager, 2 is Staff
);

-- 3. Permissions
DROP TABLE IF EXISTS Permissions;
CREATE TABLE Permissions (
    PermCode VARCHAR(50) PRIMARY KEY,
    Description VARCHAR(100)
);

-- 4. RolePermissions (Dept + Rank -> Permissions)
DROP TABLE IF EXISTS RolePermissions;
CREATE TABLE RolePermissions (
    DeptID INT NOT NULL,
    RankID INT NOT NULL,
    PermCode VARCHAR(50) NOT NULL,
    PRIMARY KEY (DeptID, RankID, PermCode),
    FOREIGN KEY (DeptID) REFERENCES Departments(ID),
    FOREIGN KEY (RankID) REFERENCES Ranks(ID),
    FOREIGN KEY (PermCode) REFERENCES Permissions(PermCode)
);

-- 5. Employees
DROP TABLE IF EXISTS Employees;
CREATE TABLE Employees (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Account VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL, -- Store plain for demo or hash? Demo usually implies plain or simple hash. Sticking to plain for absolute demo simplicity unless requested otherwise, but plan said password.
    Name VARCHAR(50) NOT NULL,
    Gender VARCHAR(10),
    Birthday DATETIME,
    Email VARCHAR(100),
    Mobile VARCHAR(20),
    JoinDate DATETIME,
    LeaveDate DATETIME,
    IsEmployed BOOLEAN DEFAULT TRUE,
    Note TEXT,
    DeptID INT,
    RankID INT,
    CreateDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (DeptID) REFERENCES Departments(ID),
    FOREIGN KEY (RankID) REFERENCES Ranks(ID)
);

SET FOREIGN_KEY_CHECKS = 1;

-- ==========================================
-- SEED DATA
-- ==========================================

-- Departments
INSERT INTO Departments (ID, Name) VALUES (1, '人事'), (2, '行政');

-- Ranks
INSERT INTO Ranks (ID, Name, RankLevel) VALUES (1, '主管', 1), (2, '職員', 2);

-- Permissions
INSERT INTO Permissions (PermCode, Description) VALUES 
('ADD_USER', '新增員工'),
('DELETE_USER', '刪除員工'),
('EDIT_USER', '修改員工'),
('SET_RESIGNED', '設為離職'),
('VIEW_ALL', '查看所有員工資料'),
('VIEW_SELF', '只能查看自己的資料'),
('VIEW_DEPT', '只能查看自己的部門員工資料'),
('WRITE_NOTE', '修改與檢視員工備註'),
('ADD_DEPT', '新增部門'),
('DELETE_DEPT', '刪除部門'),
('EDIT_DEPT', '修改部門'),
('ADD_RANK', '新增職位'),
('DELETE_RANK', '刪除職位'),
('EDIT_RANK', '修改職位'),
('ADD_ROLE_PERM', '新增角色權限'),
('DELETE_ROLE_PERM', '刪除角色權限'),
('EDIT_ROLE_PERM', '修改角色權限');

-- RolePermissions
-- 1. 人事主管 (Dept=1, Rank=1)
INSERT INTO RolePermissions (DeptID, RankID, PermCode) VALUES 
(1, 1, 'VIEW_ALL'), (1, 1, 'EDIT_USER'), (1, 1, 'SET_RESIGNED'), (1, 1, 'WRITE_NOTE'),
(1, 1, 'ADD_DEPT'), (1, 1, 'EDIT_DEPT'), (1, 1, 'DELETE_DEPT'),
(1, 1, 'ADD_RANK'), (1, 1, 'EDIT_RANK'), (1, 1, 'DELETE_RANK'),
(1, 1, 'ADD_ROLE_PERM'), (1, 1, 'EDIT_ROLE_PERM'), (1, 1, 'DELETE_ROLE_PERM');

-- 2. 人事職員 (Dept=1, Rank=2)
INSERT INTO RolePermissions (DeptID, RankID, PermCode) VALUES 
(1, 2, 'VIEW_ALL'), (1, 2, 'ADD_USER'), (1, 2, 'EDIT_USER'), (1, 2, 'WRITE_NOTE');

-- 3. 行政主管 (Dept=2, Rank=1)
INSERT INTO RolePermissions (DeptID, RankID, PermCode) VALUES 
(2, 1, 'VIEW_DEPT'), (2, 1, 'WRITE_NOTE');

-- 4. 行政職員 (Dept=2, Rank=2)
INSERT INTO RolePermissions (DeptID, RankID, PermCode) VALUES 
(2, 2, 'VIEW_SELF');

-- Employees
-- Password default '1234' for demo
INSERT INTO Employees (Account, Password, Name, Gender, DeptID, RankID, IsEmployed, JoinDate) VALUES 
('hr_manager', '1234', '人事主管A', '男', 1, 1, 1, NOW()),
('hr_staff', '1234', '人事職員B', '女', 1, 2, 1, NOW()),
('admin_manager', '1234', '行政主管C', '男', 2, 1, 1, NOW()),
('admin_staff', '1234', '行政職員D', '女', 2, 2, 1, NOW());
