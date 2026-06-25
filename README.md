# MediCheck

**Hệ thống kiểm tra thuốc phù hợp theo độ tuổi bệnh nhân**

Đề tài 53 — Nhóm 3 — Môn Công nghệ Phần mềm

Hỗ trợ bác sĩ và dược sĩ kiểm tra sự phù hợp của thuốc theo độ tuổi bệnh nhân, kê đơn tự động cảnh báo chống chỉ định, và quản lý hồ sơ bệnh nhân/danh mục thuốc.

---

## Demo

- Trang web: `http://KhanhHung.somee.com/`
- API Swagger: `http://KhanhHung.somee.com/swagger`

**Tài khoản demo:**

| Vai trò | Email | Mật khẩu |
|---|---|---|
| Admin | admin@medicheck.com | Admin@123 |
| Bác sĩ | bacsi@medicheck.com | BacSi@123 |
| Dược sĩ | duocsi@medicheck.com | DuocSi@123 |

---

## Tính năng chính

- Đăng nhập / đăng ký / phân quyền theo vai trò (Admin, Bác sĩ, Dược sĩ)
- Quản lý danh mục thuốc: thêm/sửa/xoá, tìm kiếm, lọc theo nhóm thuốc/dạng bào chế
- Thiết lập quy tắc giới hạn độ tuổi và liều lượng theo từng thuốc
- Kiểm tra thuốc theo độ tuổi bệnh nhân — tự động cảnh báo nếu không phù hợp
- Quản lý hồ sơ bệnh nhân — tự tính tuổi từ ngày sinh
- Kê đơn thuốc — hệ thống tự kiểm tra độ tuổi ngay khi thêm thuốc vào đơn
- Xuất đơn thuốc ra file PDF
- Quản lý tài khoản người dùng và ma trận phân quyền
- Trang Tổng quan (Dashboard) thống kê hệ thống và cảnh báo gần đây

---

## Công nghệ sử dụng

| Thành phần | Công nghệ |
|---|---|
| Backend | ASP.NET Core 8 Web API |
| ORM | Entity Framework Core |
| Database | Microsoft SQL Server |
| Frontend | HTML / CSS / JavaScript thuần |
| Xác thực | JWT (JSON Web Token) |
| Xuất PDF | QuestPDF |
| Quản lý dự án | Jira (Agile Scrum) |
| Quản lý mã nguồn | Git / GitHub |

---

## Cấu trúc thư mục

```
├── Controllers/        API endpoints
├── Models/              Entity ánh xạ database
├── DTOs/                Đối tượng truyền dữ liệu
├── Data/                AppDbContext, seed data
├── Services/            Xử lý nghiệp vụ (sinh PDF...)
├── Authorization/       Middleware kiểm tra phân quyền
├── wwwroot/              Giao diện (.html, phục vụ tĩnh)
├── appsettings.json
├── appsettings.Production.json
└── Program.cs
```

---

## Cài đặt chạy local

```bash
git clone <https://github.com/ngokhanhhung1910/he-thong-kiem-tra-thuoc.git>
cd he-thong-kiem-tra-thuoc

dotnet restore
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

Mở trình duyệt: `http://localhost:5000/`

> Hướng dẫn cài đặt & deploy chi tiết: xem file [`HUONG_DAN_CAI_DAT_VA_TRIEN_KHAI.md`](./HUONG_DAN_CAI_DAT_VA_TRIEN_KHAI.md)

## Quy trình phát triển (Git Flow)

```bash
git checkout develop
git checkout -b feature/ten-chuc-nang
# code, commit theo mã Jira (vd: "SCRUM-12 Them API dang nhap")
git push origin feature/ten-chuc-nang
# Tạo Pull Request -> develop -> review -> merge
```

---

## Thành viên nhóm

Ngô Khánh Hưng 2380603903 
Thân Hoàng Gia Long 2380601255
Phạm Ngọc Tình 2380602256
Nguyễn Ngọc Ngân Hậu 2380600616
Trần Phước Hưng 2380603922

## Giấy phép

Đồ án học tập — Môn Công nghệ Phần mềm.
