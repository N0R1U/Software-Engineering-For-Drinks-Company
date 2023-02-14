<<Login Credentials>>
[Admin]
Username: admin
Password: 123
[User]
Username: tri
Password: 123

<<Installation>>
External tools: 
- LINQ to SQL tools
NuGet Package:
- Microsoft.ReportViewer.Common
- Microsoft.ReportViewer.WinForms

<<How to run>>
- First create the initial database according to attached .sql file.
- Change Data Source to your SQL Server Name in both App.config and Connection.cs (Example: Data Source=DESKTOP-74B7VKK\SQLEXPRESS).
- Change pathDefault in Model.cs to the path of the Resources folder (Example: C:\Users\Admin\Desktop\test\SE_FP_G9\Source\QuanLyBanHang\Resources\).
- Start the program.
