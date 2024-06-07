using System.ComponentModel.DataAnnotations;

namespace FinalTerm.Models.RequestModel
{
    public class SinhVienRequestModel
    {
        [Required]
        [StringLength(255)]
        public string? HoTen { get; set; }

        [Required]
        public DateTime NamNhapHoc { get; set; }

    }
}
