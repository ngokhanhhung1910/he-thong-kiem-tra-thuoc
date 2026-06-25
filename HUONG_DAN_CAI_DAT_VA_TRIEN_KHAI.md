# HƯỚNG DẪN CÀI ĐẶT VÀ TRIỂN KHAI HỆ THỐNG MEDICHECK

## 1. Giới thiệu

**MediCheck** là hệ thống kiểm tra thuốc phù hợp theo độ tuổi bệnh nhân, hỗ trợ bác sĩ và dược sĩ kiểm tra, kê đơn và quản lý thông tin thuốc/bệnh nhân an toàn.

**Công nghệ sử dụng:**
- Backend: ASP.NET Core 8 Web API + Entity Framework Core
- Database: Microsoft SQL Server
- Frontend: HTML/CSS/JavaScript thuần (không dùng framework)
- Xác thực: JWT (JSON Web Token)
- Xuất PDF: QuestPDF
- Quản lý mã nguồn: Git + GitHub
- Quản lý dự án: Jira (Agile Scrum)

---

## 2. Yêu cầu hệ thống

| Thành phần | Phiên bản |
|---|---|
| .NET SDK | 8.0 hoặc mới hơn |
| SQL Server | Express/Developer/Full (local hoặc cloud) |
| Git | Bất kỳ bản mới |
| Trình duyệt | Chrome, Edge, Firefox (bản mới) |

Tải .NET SDK: https://dotnet.microsoft.com/download

---

## 3. Cài đặt và chạy trên máy local (Development)

### Bước 1 — Clone project
```powershell
git clone <link-repo-github>
cd he-thong-kiem-tra-thuoc
```

### Bước 2 — Cấu hình chuỗi kết nối database

Mở file `appsettings.json`, kiểm tra/sửa đúng với SQL Server trên máy bạn:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TEN_MAY\\SQLEXPRESS;Database=MediCheckDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```
> Thay `TEN_MAY\SQLEXPRESS` bằng tên instance SQL Server thật trên máy bạn (xem trong SQL Server Configuration Manager hoặc SSMS).

### Bước 3 — Cài package & tạo database
```powershell
dotnet restore
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Bước 4 — Chạy server
```powershell
dotnet run
```
Terminal sẽ in ra URL đang chạy, ví dụ:
```
Now listening on: http://localhost:5000
```

### Bước 5 — Mở giao diện

Mở trình duyệt vào:
```
http://localhost:5000/
```
Sẽ tự chuyển sang trang đăng nhập.

---

## 4. Tài khoản mẫu (đã có sẵn trong database)

| Vai trò | Email | Mật khẩu |
|---|---|---|
| Admin | admin@medicheck.com | Admin@123 |
| Bác sĩ | bacsi@medicheck.com | BacSi@123 |
| Dược sĩ | duocsi@medicheck.com | DuocSi@123 |

---

## 5. Cấu trúc thư mục chính

```
he-thong-kiem-tra-thuoc/
├── Controllers/        API endpoints (Auth, Thuoc, BenhNhan, DonKeThuoc, ...)
├── Models/              Các entity ánh xạ vào database
├── DTOs/                Đối tượng truyền dữ liệu giữa API và client
├── Data/                AppDbContext (EF Core), seed data
├── Services/            Logic xử lý (sinh PDF, ...)
├── Authorization/       Middleware kiểm tra phân quyền
├── wwwroot/             Toàn bộ giao diện (.html) — phục vụ tĩnh qua UseStaticFiles
├── appsettings.json             Cấu hình môi trường Development (local)
├── appsettings.Production.json  Cấu hình môi trường Production (server thật)
└── Program.cs           Khởi tạo app, đăng ký service, middleware
```

---

## 6. Triển khai (Deploy) lên Internet — miễn phí qua Somee.com

### Bước 1 — Tạo tài khoản & website trên Somee
1. Vào https://somee.com/freeaspnethosting.aspx → đăng ký (không cần thẻ)
2. Dashboard → **Trang web → Tạo trang web** → chọn ASP.NET Core → nhận subdomain miễn phí (vd `tenban.somee.com`)

### Bước 2 — Tạo Database
1. Dashboard → **MS SQL → Cơ sở dữ liệu** → tạo database mới
2. Ghi lại: Server address, Database name, Login name, Login password (bấm "Sao chép vào Clipboard")

### Bước 3 — Cấu hình `appsettings.Production.json`
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TEN_SERVER_SOMEE;Database=TEN_DATABASE;User Id=TEN_LOGIN_SOMEE;Password=MAT_KHAU_DATABASE;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```
> ⚠️ Lưu ý: "Login name" của Somee có hậu tố riêng (ví dụ `TenBan_SQLLogin_1`) — không phải tên đăng nhập Dashboard. Mật khẩu database cũng khác mật khẩu đăng nhập Dashboard — phải bấm "Sao chép vào Clipboard" để lấy đúng.

Cũng nên đổi `Jwt.Key` sang 1 chuỗi bí mật khác (tự nghĩ, dài ~32 ký tự) để bảo mật hơn.

### Bước 4 — Tạo bảng trong Database Somee

Trên máy local, tạo file script SQL từ migration:
```powershell
dotnet ef migrations script -o migration.sql
```

Vào Dashboard Somee → Database → tab **"Run scripts"** → dán nội dung file `migration.sql` vào → **Execute**.

### Bước 5 — Build và Publish
```powershell
dotnet publish -c Release -o ./publish-output
```

### Bước 6 — Upload qua FTP
1. Cài **FileZilla**: https://filezilla-project.org
2. Site Manager → nhập Host/User/Password FTP (lấy ở Dashboard Somee, mục FTP)
3. Kéo toàn bộ nội dung `publish-output` lên thư mục gốc website trên server

### Bước 7 — Kiểm tra
Mở: `http://tenban.somee.com/` — sẽ tự chuyển sang trang đăng nhập.
Mở: `http://tenban.somee.com/swagger` — để xem/test danh sách API.

---

## 7. Quy trình Git khi phát triển thêm tính năng

```bash
git checkout develop
git pull origin develop
git checkout -b feature/ten-chuc-nang

# ... code, test ...

git add .
git commit -m "SCRUM-XX Mo ta ngan gon thay doi"
git push origin feature/ten-chuc-nang
# Tạo Pull Request feature/ten-chuc-nang -> develop trên GitHub
# Sau khi merge, cuối Sprint merge develop -> main + tạo Release
```

---

## 8. Xử lý lỗi thường gặp

| Lỗi | Nguyên nhân | Cách sửa |
|---|---|---|
| `address already in use` | Port đang bị chiếm bởi tiến trình cũ | `taskkill /PID <so> /F` hoặc đổi port trong `launchSettings.json` |
| `/swagger` báo 404 | Thiếu `launchSettings.json` hoặc môi trường không phải Development | Đảm bảo Swagger luôn bật trong `Program.cs` (`app.UseSwagger()` không điều kiện) |
| `File is locked by another process` khi build | Server cũ vẫn đang chạy ngầm | Tắt qua Task Manager hoặc `taskkill /PID <so> /F` |
| `ERR_SSL_PROTOCOL_ERROR` | Gọi `https` vào port chỉ chạy `http` | Đổi đúng giao thức (`http`) hoặc đúng port (5001 cho https) |
| API trả về 500 khi deploy | Database trên server chưa có bảng | Chạy migration script qua "Run scripts" trên Somee |
| `Failed to fetch` / "Không kết nối được tới server" | Sai port/domain trong file `.html`, hoặc server chưa chạy | Kiểm tra `API_BASE` trong file JS đúng domain đang chạy |
| Lỗi build do tên class trùng | Có 2 file định nghĩa cùng 1 class | Tìm và xoá file trùng: `Get-ChildItem -Recurse -Filter "*.cs" | Select-String "class TenClass"` |

---

## 9. Liên hệ / Ghi chú thêm

Tài liệu này tổng hợp từ quá trình xây dựng đồ án **Hệ thống kiểm tra thuốc phù hợp theo độ tuổi bệnh nhân** — Đề tài 53, Nhóm 3, môn Công nghệ Phần mềm.
