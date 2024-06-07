namespace FinalTerm.Services;
using Dapper;
using Microsoft.Data.SqlClient;
using FinalTerm.Models.Dtos;
using FinalTerm.Models.RequestModel;

public interface IDatabaseService
{
    List<NganhHocDto> GetNganh(int manganh);
    int AddSinhVien(SinhVienRequestModel requestModel);

    List<SinhVienDto> GetSinhVien();
    List<SinhVienMonHocDto> GetSinhVienMonHoc();

    MonHocDto? GetMonHocById(int mamon);
    SinhVienDto? GetSinhVienById(int masv);
    int UpdateSinhVienBoMon(int mamon, int masv, SinhVienMonHocRequestModel requestModel);
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

    public List<SinhVienDto> GetSinhVien()
    {
        string sql = """
            SELECT MaSinhVien, HoTen, NamNhapHoc
            FROM SinhVien
            """;

        using var connection = GetConnection();

        return connection.Query<SinhVienDto>(sql).ToList();
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

    public int UpdateSinhVienBoMon(int mamon, int masv, SinhVienMonHocRequestModel requestModel)
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



    private SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }


}


