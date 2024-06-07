using System.ComponentModel.DataAnnotations;

namespace FinalTerm.Models.RequestModel
{
    public class SinhVienMonHocRequestModel
    {
        [Required]
        public int MaMon { get; set; }

        public int MaSinhVien { get; set; }

        [Range(0.0, 10.0, ErrorMessage = "DiemChuyenCan must be between 0 and 10.")]
        public float DiemChuyenCan { get; set; }

        [Range(0.0, 10.0, ErrorMessage = "DiemChuyenCan must be between 0 and 10.")]
        public float DiemGiuaKy { get; set; }

    }
}
