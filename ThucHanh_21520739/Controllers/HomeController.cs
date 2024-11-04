using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ThucHanh_21520739.Models;
using ThucHanh_21520739.Models.Authentication;
using X.PagedList;
using X.PagedList.Extensions;

namespace ThucHanh_21520739.Controllers
{
    public class HomeController : Controller
    {
        QlbanVaLiContext db = new QlbanVaLiContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Authentication]

        public IActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page <= 0 ? 1 : page.Value;

            // Fetch the list of products and apply sorting
            var lstsanpham = db.TDanhMucSps.AsNoTracking().OrderBy(x => x.TenSp);

            // Convert the list to a PagedList
            IPagedList<TDanhMucSp> pagedList = lstsanpham.ToPagedList(pageNumber, pageSize);

            // Pass the pagedList to the view
            return View(pagedList);
        }
        public IActionResult SanPhamTheoLoai(String maloai)
        {
            List<TDanhMucSp> lstsanpham = db.TDanhMucSps.Where(x => x.MaLoai == maloai).OrderBy(x=> x.TenSp).ToList();
            return View(lstsanpham);
        }
        [Authentication]
        public IActionResult ChiTietSanPham(string maSp)
        {
            var SanPham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp == maSp);
            var anhSanPham = db.TAnhSps.Where(x => x.MaSp == maSp).ToList();
            ViewBag.anhSanPham = anhSanPham;

            return View(SanPham);
        }
        [Authentication]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
