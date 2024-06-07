using Microsoft.AspNetCore.Mvc;
using FinalTerm.Models.RequestModel;
using FinalTerm.Services;

namespace FinalTerm.Controllers
{
    public class Controller : ControllerBase
    {
        private readonly IDatabaseService _databaseService;

        public Controller(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        [HttpGet]
        [Route("get-nganh-hoc")]

        public IActionResult GetNganh(int manganh)
        {
            var items = _databaseService.GetNganh(manganh);
            return Ok(items);
        }

        [HttpGet]
        [Route("get-sinh-vien")]

        public IActionResult GetSinhVien()
        {
            var items = _databaseService.GetSinhVien();
            return Ok(items);
        }

        [HttpPost]
        [Route("add-sinh-vien")]

        public IActionResult AddSinhVien([FromBody] SinhVienRequestModel model)
        {
            if(model.NamNhapHoc.Year > DateTime.Now.Year)
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

        [HttpPut]
        [Route("update-sinh-vien-mon-hoc")]
        public IActionResult UpdateSinhVienMonHoc([FromBody] SinhVienMonHocRequestModel model)
        {
            // Check if the student exists
            var sinhVien = _databaseService.GetSinhVienById(model.MaSinhVien);
            var monHoc = _databaseService.GetMonHocById(model.MaMon);

            if (sinhVien == null || monHoc == null)
            {
                ModelState.AddModelError("", "Invalid MaSinhVien or MaMon. Both must be valid values from SinhVien and MonHoc tables respectively.");
                return BadRequest(ModelState);
            }


            // If model state is valid, update the scores
            if (ModelState.IsValid)
            {
                var result = _databaseService.UpdateSinhVienBoMon(model.MaMon, model.MaSinhVien, model);
                if (result > 0)
                {
                    return Ok(new { msg = "Update successful" });
                }
            }

            return BadRequest(ModelState);
        }
    }
}
