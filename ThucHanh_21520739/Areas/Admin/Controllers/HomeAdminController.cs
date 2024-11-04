using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ThucHanh_21520739.Models;
using ThucHanh_21520739.Models.Authentication;

namespace ThucHanh_21520739.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminController : Controller
    {
        QlbanVaLiContext db = new QlbanVaLiContext();
        [Route("")]
        [Route("index")]
        [Authentication]
        public IActionResult Index()
        {
            return View();
        }
        [Route("danhmucsanpham")]
        [Authentication]
        public IActionResult DanhMucSanPham()
        {
            var lstSanPham = db.TDanhMucSps.ToList();
            return View(lstSanPham);
        }
        [Route("ThemSanPhamMoi")]
        [HttpGet]
        public IActionResult ThemSanPhamMoi()
        {
            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
            return View();
        }

        [Route("ThemSanPhamMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemSanPhamMoi(TDanhMucSp sanPham)
        {
            if(ModelState.IsValid)
            {
                db.TDanhMucSps.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham");
            }
            return View(sanPham);
        }

        [Route("EditSanPham")]
        [HttpGet]
        public IActionResult EditSanPham(string MaSanPham)
        {
            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");

            var sanPham = db.TDanhMucSps.Find(MaSanPham);
            return View(sanPham);
        }

        [Route("EditSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSanPham(TDanhMucSp sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Update(sanPham);
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham", "HomeAdmin");
            }
            return View(sanPham);
        }

        [Route("XoaSanPham")]
        [HttpGet]
        public IActionResult XoaSanPham(string MaSanPham)
        {
            TempData["Message"] = "";
            var chiTietSanPhams = db.TChiTietSanPhams.Where(x => x.MaSp == MaSanPham).ToList();
            if (chiTietSanPhams.Count() > 0)
            {
                TempData["Message"] = "Không xóa được sản phẩm này";
                return RedirectToAction("DanhMucSanPham", "HomeAdmin");
            }

            var anhSanPhams = db.TAnhSps.Where(x => x.MaSp == MaSanPham);
            if (anhSanPhams.Any()) db.RemoveRange(anhSanPhams);
            db.Remove(db.TDanhMucSps.Find(MaSanPham));
            db.SaveChanges();

            TempData["Message"] = "Sản phẩm đã được xóa";
            return RedirectToAction("DanhMucSanPham", "HomeAdmin");
        }

        [Route("danhmucnguoidung")]
        [Authentication]
        public IActionResult DanhMucNguoiDung()
        {
            var lstUsers = db.TUsers.ToList();
            return View(lstUsers);
        }
        [Route("ThemNguoiDungMoi")]
        [HttpGet]
        public IActionResult ThemNguoiDungMoi()
        {
            return View();
        }

        [Route("ThemNguoiDungMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemNguoiDungMoi(TUser user)
        {
            if (ModelState.IsValid)
            {
                db.TUsers.Add(user);
                db.SaveChanges();
                return RedirectToAction("DanhMucNguoiDung");
            }
            return View(user);
        }

        [Route("EditNguoiDung")]
        [HttpGet]
        public IActionResult EditNguoiDung(string username)
        {
            var user = db.TUsers.Find(username);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [Route("EditNguoiDung")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditNguoiDung(TUser user)
        {
            if (ModelState.IsValid)
            {
                db.Update(user);
                db.SaveChanges();
                return RedirectToAction("DanhMucNguoiDung");
            }
            return View(user);
        }

        [Route("XoaNguoiDung")]
        [HttpGet]
        public IActionResult XoaNguoiDung(string username)
        {
            var user = db.TUsers.Find(username);
            if (user == null)
            {
                TempData["Message"] = "Người dùng không tồn tại.";
                return RedirectToAction("DanhMucNguoiDung");
            }

            db.TUsers.Remove(user);
            db.SaveChanges();
            TempData["Message"] = "Người dùng đã được xóa";
            return RedirectToAction("DanhMucNguoiDung");
        }



        [Route("danhmuckhachhang")]
        public IActionResult DanhMucKhachHang()
        {
            var lstKhachHang = db.TKhachHangs.ToList();
            return View(lstKhachHang);
        }
    }
}
