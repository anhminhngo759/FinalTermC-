using Microsoft.AspNetCore.Mvc;
using FinalTerm.Models.RequestModel;
using FinalTerm.Services;

namespace FinalTerm.Controllers
{
    [Route("api/[controller]")]
    public class DaoTaoController : Controller
    {
        private readonly IDatabaseService _databaseService;

        public DaoTaoController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /*Danh sách môn học của 1 ngành (mã ngành được nhập vào). 
         * Thông tin gồm: Mã môn, tên môn, số tín chỉ*/

        [HttpGet]
        [Route("get-nganh-hoc")]

        public IActionResult GetNganh(int manganh)
        {
            var items = _databaseService.GetNganh(manganh);
            return Ok(items);
        }

        /* Danh sách tất cả ngành học cùng với tổng số môn học và tổng số tín chỉ.*/
        [HttpGet]
        [Route("get-tong-nganh-hoc-tin-chi")]

        public IActionResult GetTongNganhHocTinChi()
        {
            var items = _databaseService.GetTongNganhHocTinChi();
            return Ok(items);
        }

        [HttpGet]
        [Route("get-sinh-vien")]

        public IActionResult GetSinhVien()
        {
            var items = _databaseService.GetSinhVien();
            return Ok(items);
        }

        /*Thêm mới 1 sinh viên. Yêu cầu họ tên là bắt buộc, năm nhập học 
          phải bé hơn hoặc bằng năm hiện tại.*/
        [HttpPost]
        [Route("add-sinh-vien")]

        public IActionResult AddSinhVien([FromBody] SinhVienRequestModel model)
        {
            if (model.NamNhapHoc.Year > DateTime.Now.Year)
            {
                ModelState.AddModelError("", "Invalid year");
                return BadRequest(ModelState);
            }
            var items = _databaseService.AddSinhVien(model);
            //return Ok(items);
            if (items > 0)
            {
                return Ok(new { msg = "Insert successful" });
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("get-sinh-vien-mon-hoc")]

        public IActionResult GetSinhVienMonHoc()
        {
            var items = _databaseService.GetSinhVienMonHoc();
            return Ok(items);
        }

        /* Cập nhật điểm chuyên cần, giữa kỳ 1 môn học cho 1 sinh viên. 
         * Yêu cầu điểm trong khoản từ 0 đến 10, cho phép nhập số thập phân */
        [HttpPut]
        [Route("update-sinh-vien-mon-hoc/{maSinhVien}/{maMon}")]
        public IActionResult UpdateSinhVienMonHoc(int maSinhVien, int maMon, [FromBody] SinhVienMonHocRequestModel model)
        {
            var sinhVien = _databaseService.GetSinhVienById(maSinhVien);
            var monHoc = _databaseService.GetMonHocById(maMon);

            if (sinhVien is null || monHoc is null)
            {
                ModelState.AddModelError("", "Invalid MaSinhVien or MaMon. Both must be valid values from SinhVien and MonHoc tables respectively.");
                // 400
                return BadRequest(ModelState);
            }

            // If model state is valid, update the scores
            if (ModelState.IsValid)
            {
                var result = _databaseService.UpdateSinhVienMonHoc(maMon, maSinhVien, model);
                if (result > 0)
                {
                    return Ok(new { msg = "Update successful" });
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        [Route("delete-sinh-vien/{masv}")]
        public IActionResult DeleteSinhVien(int masv)
        {
            var sinhVien = _databaseService.GetSinhVienById(masv);

            if (sinhVien is null)
            {
                //404
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = _databaseService.DeleteSinhVien(masv);

                if (result > 0)
                {
                    return Ok(new { msg = "Delete successful" });
                }
            }

            return BadRequest(ModelState);
        }

    }
}
