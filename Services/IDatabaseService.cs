namespace FinalTerm.Services;
using Dapper;
using Microsoft.Data.SqlClient;
using FinalTerm.Models.Dtos;
using FinalTerm.Models.RequestModel;

public interface IDatabaseService
{
    List<NganhHocDto> GetNganh(int manganh);
    List<TongNganhHocTinChiDto> GetTongNganhHocTinChi();

    List<SinhVienDto> GetSinhVien();
    int AddSinhVien(SinhVienRequestModel requestModel);

    List<SinhVienMonHocDto> GetSinhVienMonHoc();

    MonHocDto? GetMonHocById(int mamon);
    SinhVienDto? GetSinhVienById(int masv);
    int UpdateSinhVienMonHoc(int mamon, int masv, SinhVienMonHocRequestModel requestModel);

    int DeleteSinhVien(int masv);

    
}

public class DatabaseService : IDatabaseService
{
    private readonly string _connectionString;
    public DatabaseService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<NganhHocDto> GetNganh(int manganh)
    {
        string sql = """
            select m.MaMon,m.TenMon,m.SoTinChi
            from Monhoc m join NganhHoc n on m.MaNganh = n.MaNganh
            where m.MaNganh = @manganh
            """;
        using var connection = GetConnection();

        return connection.Query<NganhHocDto>(sql, new { manganh }).ToList();
    }

    public List<TongNganhHocTinChiDto> GetTongNganhHocTinChi()
    {
        string sql = """
            select nh.MaNganh,nh.TenNganh,
            COUNT(mh.MaMon) AS TotalMonHoc,SUM(mh.SoTinChi) AS TotalSoTinChi
            from NganhHoc as nh
            join MonHoc as mh on nh.MaNganh = mh.MaNganh
            group by nh.MaNganh,nh.TenNganh
            """;
        using var connection = GetConnection();

        return connection.Query<TongNganhHocTinChiDto>(sql).ToList();
    }

    public List<SinhVienDto> GetSinhVien()
    {
        string sql = """
            SELECT MaSinhVien, HoTen, NamNhapHoc
            FROM SinhVien
            """;

        using var connection = GetConnection();

        return connection.Query<SinhVienDto>(sql).ToList();
    }

    public int AddSinhVien(SinhVienRequestModel requestModel)
    {
        string sql = """
            INSERT INTO SinhVien(
                HoTen,
                NamNhapHoc)
            VALUES (@hoten, @namnhaphoc)
            """;

        using var connection = GetConnection();

        var result = connection.Execute(sql, new
        {
            hoten = requestModel.HoTen,
            namnhaphoc = requestModel.NamNhapHoc
        });

        return result;
    }

    public List<SinhVienMonHocDto> GetSinhVienMonHoc()
    {
        string sql = """
            SELECT svmh.MaMon, svmh.MaSinhVien, svmh.DiemChuyenCan, svmh.DiemGiuaKy, svmh.DiemCuoiKy
            FROM SinhVien_MonHoc as svmh 
            join SinhVien as sv on svmh.MaSinhVien = sv.MaSinhVien
            join MonHoc as mh on mh.MaMon = svmh.MaMon
            """;

        using var connection = GetConnection();

        return connection.Query<SinhVienMonHocDto>(sql).ToList();
    }

    public MonHocDto? GetMonHocById(int mamon)
    {
        string sql = """
            select * from MonHoc
            WHERE MaMon = @mamon
            """;
        using var connection = GetConnection();

        return connection.Query<MonHocDto>(sql, new { mamon }).FirstOrDefault();
    }

    public SinhVienDto? GetSinhVienById(int masv)
    {
        string sql = """
            select * from SinhVien
            WHERE MaSinhVien = @masv
            """;
        using var connection = GetConnection();

        return connection.Query<SinhVienDto>(sql, new { masv }).FirstOrDefault();
    }

    public int UpdateSinhVienMonHoc(int mamon, int masv, SinhVienMonHocRequestModel requestModel)
    {
        string sql = """
            UPDATE SinhVien_MonHoc
            SET 
                DiemChuyenCan = @diemchuyencan,
                DiemGiuaKy = @diemgiuaky
            WHERE MaMon = @maMon 
            and MaSinhVien = @maSV
            """;

        using var connection = GetConnection();

        var result = connection.Execute(sql, new
        {
            diemchuyencan = requestModel.DiemChuyenCan,
            diemgiuaky = requestModel.DiemGiuaKy,
            maMon = mamon,
            maSV = masv
        });

        return result;
    }

    public int DeleteSinhVien(int masv)
    {
        using var connection = GetConnection();

        try
        {
            // Kiểm tra xem có bản ghi liên quan trong SinhVien_MonHoc không
            string svmh = """
            SELECT COUNT(*) 
            FROM SinhVien_MonHoc
            WHERE MaSinhVien = @maSv
            """;
            var count = connection.ExecuteScalar<int>(svmh, new { maSv = masv });

            if (count > 0)
            {
                // Xóa trước SinhVien_MonHoc
                string deleteSinhVienMonHoc = """
                DELETE FROM SinhVien_MonHoc
                WHERE MaSinhVien = @maSv
                """;
                connection.Execute(deleteSinhVienMonHoc, new { maSv = masv });
            }

            // Sau đó xóa  SinhVien
            string deleteSinhVien = """
            DELETE FROM SinhVien
            WHERE MaSinhVien = @maSv
            """;
            var result = connection.Execute(deleteSinhVien, new { maSv = masv });

            return result;
        }
        catch
        {
            // Nếu có lỗi, xử lý ngoại lệ ở đây
            throw;
        }
    }

    private SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }


}


