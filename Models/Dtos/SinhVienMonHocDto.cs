using System.ComponentModel.DataAnnotations;

namespace FinalTerm.Models.Dtos
{
    public class SinhVienMonHocDto
    {
        public int MaMon { get; set; }

        public int MaSinhVien { get; set; }

        public float DiemChuyenCan { get; set; }

        public float DiemGiuaKy { get; set; }

        public float DiemCuoiKy { get; set; }
    }
}
