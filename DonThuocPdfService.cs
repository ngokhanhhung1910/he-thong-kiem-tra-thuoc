using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using MediCheck.Api.Models;

namespace MediCheck.Api.Services
{
    public class DonThuocPdfService
    {
        public byte[] TaoPdf(DonKeThuoc don)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(35);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("MEDICHECK").FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                        col.Item().Text("Hệ thống kiểm tra thuốc theo độ tuổi bệnh nhân").FontSize(10).FontColor(Colors.Grey.Darken1);
                        col.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                        col.Item().PaddingTop(10).AlignCenter().Text("ĐƠN THUỐC").FontSize(18).Bold();
                        col.Item().AlignCenter().Text($"Mã đơn: {don.MaDon}").FontSize(10);
                    });

                    page.Content().PaddingTop(20).Column(col =>
                    {
                        col.Item().Text($"Bệnh nhân: {don.BenhNhan.HoTen}").Bold();
                        col.Item().Text($"Tuổi: {TinhTuoi(don.BenhNhan.NgaySinh)}    Giới tính: {don.BenhNhan.GioiTinh}");
                        col.Item().Text($"Mã bệnh nhân: {don.BenhNhan.MaBenhNhan}");
                        col.Item().Text($"Bác sĩ kê đơn: {(don.BacSiKe != null ? don.BacSiKe.HoTen : "Không rõ")}");
                        col.Item().Text($"Ngày kê: {don.NgayKe:dd/MM/yyyy HH:mm}");

                        col.Item().PaddingTop(16).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(4);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(6).Text("Tên thuốc").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(6).Text("Kết quả").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(6).Text("Lý do cảnh báo").Bold();
                            });

                            foreach (var item in don.ChiTiets)
                            {
                                var mauChu = item.KetQuaKiemTra == KetQuaKiemTra.AnToan ? Colors.Green.Darken1 : Colors.Red.Darken1;
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6).Text(item.Thuoc.TenThuoc);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6).Text(item.KetQuaKiemTra.ToString()).FontColor(mauChu).Bold();
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6).Text(item.LyDoCanhBao ?? "");
                            }
                        });

                        col.Item().PaddingTop(40).AlignRight().Column(c =>
                        {
                            c.Item().AlignCenter().Text("Bác sĩ kê đơn");
                            c.Item().PaddingTop(50).AlignCenter().Text("(Ký, ghi rõ họ tên)").FontSize(9).FontColor(Colors.Grey.Darken1);
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Đơn thuốc được in từ hệ thống MediCheck - ").FontSize(8).FontColor(Colors.Grey.Darken1);
                        x.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontSize(8).FontColor(Colors.Grey.Darken1);
                    });
                });
            });

            return document.GeneratePdf();
        }

        private static int TinhTuoi(DateTime ngaySinh)
        {
            var today = DateTime.Today;
            int tuoi = today.Year - ngaySinh.Year;
            if (ngaySinh.Date > today.AddYears(-tuoi)) tuoi--;
            return tuoi;
        }
    }
}
