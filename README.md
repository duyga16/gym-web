Cài đặt
Prerequisites
Đảm bảo bạn đã cài đặt:

.NET SDK 6.0 trở lên
SQL Server (hoặc cơ sở dữ liệu bạn sử dụng)
Cài đặt và chạy ứng dụng
Clone repository:

bash
Sao chép mã
git clone https://github.com/duyga16/gym-web.git
Chuyển đến thư mục dự án:

bash
Sao chép mã
cd gym-web-application
Cài đặt các gói NuGet:

bash
Sao chép mã
dotnet restore
Tạo cơ sở dữ liệu và di chuyển dữ liệu:

bash
Sao chép mã
dotnet ef database update
Chạy ứng dụng:

bash
Sao chép mã
dotnet run
Truy cập vào ứng dụng ở địa chỉ https://localhost:5001.

Các công nghệ sử dụng
ASP.NET Core MVC: Để xây dựng ứng dụng web.
Entity Framework Core: Để tương tác với cơ sở dữ liệu.
SQL Server: Hệ quản trị cơ sở dữ liệu.
Bootstrap: Để tạo giao diện người dùng.
CKEditor & CKFinder: Để quản lý và chỉnh sửa nội dung trong Admin Pages.
Cấu trúc dự án
Controllers: Chứa các controller để xử lý logic cho các trang web.
Views: Chứa các view cho từng trang web.
Models: Định nghĩa các model cho cơ sở dữ liệu.
wwwroot: Chứa các tệp tĩnh như CSS, JS, hình ảnh.
Hướng dẫn phát triển
Tạo controller mới: Sử dụng câu lệnh:

bash
Sao chép mã
dotnet aspnet-codegenerator controller -name [TênController] -m [TênModel] -dc [TênDbContext] --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
Tạo migration mới:

bash
Sao chép mã
dotnet ef migrations add [TênMigration]
Cập nhật cơ sở dữ liệu:

bash
Sao chép mã
dotnet ef database update
