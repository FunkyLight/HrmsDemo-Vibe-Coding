# HrmsDemo - 人力資源管理系統 (HRMS)

這是一個基於 .NET WinForms 與 MySQL 的簡易人力資源管理系統範例，展示了基本 CRUD 操作、虛擬模式列表 (Virtual Mode DataGridView) 以及以角色為基礎的權限控制 (RBAC)。

## 技術堆疊 (Tech Stack)

- **Framework**: .NET 10.0 (Windows)
- **UI**: Windows Forms (WinForms)
- **Database**: MySQL 8.0+
- **ORM**: Dapper (v2.1.66)
- **MySql Library**: MySql.Data (v9.5.0)

## 功能特色

- **員工管理**: 支援搜尋、新增、修改、刪除員工資料。
- **高效列表**: 使用 DataGridView Virtual Mode 處理大量資料載入。
- **部門與職位**: 管理部門與職位結構。
- **權限控制 (RBAC)**: 
  - 透過 `Dept` (部門) + `Rank` (職位) 組合對應 `PermCode` (權限代碼)。
  - 支援不同層級的資料檢視權限 (查看全部、查看部門、查看自己)。
- **權限管理介面**: 可視化設定不同角色的權限。

## 快速開始 (Quick Start)

### 1. 建立資料庫

請使用 MySQL Workbench 或 CLI 執行專案根目錄下的 SQL 腳本：

1.  執行 `db_init.sql` 初始化資料庫結構與基礎數據。
2.  (選用) 執行 `seed_50_employees.sql` 產生測試用的 50 筆員工資料。

### 2. 設定連線字串

打開 `HrmsDemo/Helpers/DbHelper.cs`，修改 `ConnectionString` 屬性以符合您的環境：

```csharp
public static string ConnectionString { get; set; } = "Server=localhost;Database=HrmsDB;Uid=root;Pwd=YOUR_PASSWORD;";
```

### 3. 編譯與執行

使用 Visual Studio 2022+ 或 CLI 執行專案：

```bash
dotnet build
dotnet run --project HrmsDemo/HrmsDemo.csproj
```

## 預設測試帳號 (Default Accounts)

所有預設密碼皆為: `1234`

| 帳號 (Account) | 角色 (Role) | 權限說明 |
| :--- | :--- | :--- |
| `hr_manager` | 人事主管 | 擁有完整管理權限 (新增/刪除員工、管理部門/職位/權限) |
| `hr_staff` | 人事職員 | 可管理員工資料，但無權限設定功能 |
| `admin_manager`| 行政主管 | 僅能查看本部們員工 |
| `admin_staff` | 行政職員 | 僅能查看自己資料 |

## 專案結構

- **Repositories/**: 資料存取層 (Dapper 實作)
- **Services/**: 商業邏輯層
- **Models/**: 資料實體 (POCOs)
- **Helpers/**: 工具類 (DbHelper)
- **Forms**:
  - `MainForm`: 主視窗 (員工列表與功能選單)
  - `LoginForm`: 登入視窗
  - `DeptForm` / `RankForm`: 部門與職位管理
  - `RolePermForm`: 角色權限設定
