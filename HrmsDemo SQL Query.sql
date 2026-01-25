-- Create Database
CREATE schema IF NOT EXISTS HrmsDB;
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


INSERT INTO `hrmsdb`.employees (Account, Password, Name, Gender, Birthday, Mobile, Email, DeptID, RankID, JoinDate, IsEmployed, Note, CreateDate) VALUES 
('user101', '123456789', '陳韋伶', '女', '1990-05-12', '0910000101', 'user101@example.com', 1, 2, '2023-01-15', 1, '測試資料', NOW()),
('user102', '123456789', '林志豪', '男', '1985-08-23', '0910000102', 'user102@example.com', 2, 2, '2022-11-01', 1, '測試資料', NOW()),
('user103', '123456789', '張雅婷', '女', '1992-03-30', '0910000103', 'user103@example.com', 1, 1, '2023-06-20', 1, '測試資料', NOW()),
('user104', '123456789', '王建宏', '男', '1988-12-11', '0910000104', 'user104@example.com', 1, 1, '2021-05-10', 1, '測試資料', NOW()),
('user105', '123456789', '李怡君', '女', '1995-07-04', '0910000105', 'user105@example.com', 2, 2, '2024-02-01', 1, '測試資料', NOW()),
('user106', '123456789', '陳俊傑', '男', '1991-09-18', '0910000106', 'user106@example.com', 1, 2, '2023-03-12', 1, '測試資料', NOW()),
('user107', '123456789', '林淑芬', '女', '1983-04-25', '0910000107', 'user107@example.com', 2, 1, '2020-08-15', 1, '測試資料', NOW()),
('user108', '123456789', '黃欣怡', '女', '1994-11-08', '0910000108', 'user108@example.com', 2, 2, '2023-09-01', 1, '測試資料', NOW()),
('user109', '123456789', '張家豪', '男', '1989-02-14', '0910000109', 'user109@example.com', 1, 1, '2022-01-20', 1, '測試資料', NOW()),
('user110', '123456789', '吳美玲', '女', '1993-06-30', '0910000110', 'user110@example.com', 2, 2, '2023-05-05', 1, '測試資料', NOW()),
('user111', '123456789', '劉志偉', '男', '1987-10-10', '0910000111', 'user111@example.com', 1, 2, '2021-12-12', 1, '測試資料', NOW()),
('user112', '123456789', '楊雅雯', '女', '1996-01-22', '0910000112', 'user112@example.com', 2, 1, '2024-03-15', 1, '測試資料', NOW()),
('user113', '123456789', '陳冠宇', '男', '1990-08-05', '0910000113', 'user113@example.com', 2, 2, '2023-04-01', 1, '測試資料', NOW()),
('user114', '123456789', '林佳儀', '女', '1992-12-28', '0910000114', 'user114@example.com', 1, 2, '2023-07-20', 1, '測試資料', NOW()),
('user115', '123456789', '王志明', '男', '1986-03-17', '0910000115', 'user115@example.com', 2, 1, '2021-06-10', 1, '測試資料', NOW()),
('user116', '123456789', '李佩珊', '女', '1995-09-09', '0910000116', 'user116@example.com', 1, 2, '2023-11-11', 1, '測試資料', NOW()),
('user117', '123456789', '張宗翰', '男', '1988-05-30', '0910000117', 'user117@example.com', 2, 1, '2022-04-25', 1, '測試資料', NOW()),
('user118', '123456789', '陳怡婷', '女', '1993-02-18', '0910000118', 'user118@example.com', 1, 2, '2023-08-30', 1, '測試資料', NOW()),
('user119', '123456789', '黃建銘', '男', '1991-11-25', '0910000119', 'user119@example.com', 2, 2, '2023-01-05', 1, '測試資料', NOW()),
('user120', '123456789', '吳心怡', '女', '1994-07-07', '0910000120', 'user120@example.com', 2, 1, '2023-10-10', 1, '測試資料', NOW()),
('user121', '123456789', '林志宏', '男', '1985-04-12', '0910000121', 'user121@example.com', 1, 1, '2020-09-20', 1, '測試資料', NOW()),
('user122', '123456789', '劉家瑜', '女', '1996-10-01', '0910000122', 'user122@example.com', 1, 2, '2024-04-01', 1, '測試資料', NOW()),
('user123', '123456789', '陳偉倫', '男', '1989-12-15', '0910000123', 'user123@example.com', 1, 2, '2022-02-15', 1, '測試資料', NOW()),
('user124', '123456789', '楊淑君', '女', '1992-06-06', '0910000124', 'user124@example.com', 2, 2, '2023-02-28', 1, '測試資料', NOW()),
('user125', '123456789', '張俊傑', '男', '1990-03-22', '0910000125', 'user125@example.com', 2, 1, '2023-05-15', 1, '測試資料', NOW()),
('user126', '123456789', '王雅惠', '女', '1995-08-20', '0910000126', 'user126@example.com', 1, 2, '2023-12-05', 1, '測試資料', NOW()),
('user127', '123456789', '李建國', '男', '1987-01-30', '0910000127', 'user127@example.com', 1, 1, '2021-03-10', 1, '測試資料', NOW()),
('user128', '123456789', '陳美惠', '女', '1993-11-28', '0910000128', 'user128@example.com', 1, 2, '2023-09-15', 1, '測試資料', NOW()),
('user129', '123456789', '林冠宇', '男', '1991-05-08', '0910000129', 'user129@example.com', 2, 2, '2022-10-20', 1, '測試資料', NOW()),
('user130', '123456789', '黃雅婷', '女', '1994-02-14', '0910000130', 'user130@example.com', 2, 2, '2023-03-30', 1, '測試資料', NOW()),
('user131', '123456789', '吳志明', '男', '1988-09-25', '0910000131', 'user131@example.com', 1, 1, '2022-07-10', 1, '測試資料', NOW()),
('user132', '123456789', '劉欣怡', '女', '1996-06-18', '0910000132', 'user132@example.com', 1, 1, '2024-05-01', 1, '測試資料', NOW()),
('user133', '123456789', '陳家宏', '男', '1990-10-30', '0910000133', 'user133@example.com', 1, 2, '2023-01-10', 1, '測試資料', NOW()),
('user134', '123456789', '楊佩珊', '女', '1992-04-05', '0910000134', 'user134@example.com', 2, 2, '2023-06-01', 1, '測試資料', NOW()),
('user135', '123456789', '張志偉', '男', '1986-12-12', '0910000135', 'user135@example.com', 2, 1, '2021-08-20', 1, '測試資料', NOW()),
('user136', '123456789', '林心怡', '女', '1995-03-15', '0910000136', 'user136@example.com', 1, 2, '2023-11-25', 1, '測試資料', NOW()),
('user137', '123456789', '王建華', '男', '1989-07-22', '0910000137', 'user137@example.com', 1, 1, '2022-05-15', 1, '測試資料', NOW()),
('user138', '123456789', '李淑君', '女', '1993-01-08', '0910000138', 'user138@example.com', 2, 2, '2023-08-05', 1, '測試資料', NOW()),
('user139', '123456789', '陳志宏', '男', '1991-10-18', '0910000139', 'user139@example.com', 2, 2, '2022-11-30', 1, '測試資料', NOW()),
('user140', '123456789', '黃美玲', '女', '1994-05-12', '0910000140', 'user140@example.com', 1, 2, '2023-04-10', 1, '測試資料', NOW()),
('user141', '123456789', '吳冠宇', '男', '1987-08-28', '0910000141', 'user141@example.com', 2, 1, '2021-09-15', 1, '測試資料', NOW()),
('user142', '123456789', '劉雅惠', '女', '1996-02-05', '0910000142', 'user142@example.com', 1, 2, '2024-01-20', 1, '測試資料', NOW()),
('user143', '123456789', '陳建國', '男', '1990-11-30', '0910000143', 'user143@example.com', 1, 2, '2023-06-25', 1, '測試資料', NOW()),
('user144', '123456789', '楊佳儀', '女', '1992-09-14', '0910000144', 'user144@example.com', 2, 2, '2022-12-10', 1, '測試資料', NOW()),
('user145', '123456789', '張偉倫', '男', '1988-03-25', '0910000145', 'user145@example.com', 1, 1, '2022-03-01', 1, '測試資料', NOW()),
('user146', '123456789', '林怡君', '女', '1995-06-18', '0910000146', 'user146@example.com', 2, 2, '2023-10-30', 1, '測試資料', NOW()),
('user147', '123456789', '王志豪', '男', '1985-01-20', '0910000147', 'user147@example.com', 1, 1, '2021-04-15', 1, '測試資料', NOW()),
('user148', '123456789', '李雅雯', '女', '1993-08-08', '0910000148', 'user148@example.com', 2, 2, '2023-09-05', 1, '測試資料', NOW()),
('user149', '123456789', '陳俊宏', '男', '1991-04-02', '0910000149', 'user149@example.com', 1, 2, '2022-07-20', 1, '測試資料', NOW()),
('user150', '123456789', '黃淑芬', '女', '1994-12-12', '0910000150', 'user150@example.com', 2, 2, '2023-12-01', 1, '測試資料', NOW());
