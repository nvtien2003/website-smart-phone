using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ictshop.Models;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using PagedList;
using System.Data.Entity;

namespace Ictshop.Controllers
{
    public class SanphamController : Controller
    {
        QLdienthoaiEntities db = new QLdienthoaiEntities();
        // Action để hiển thị form tìm kiếm

        public ActionResult Index(int? page)
        {
            int pageSize = 6; // Số lượng items mỗi trang
            int pageNumber = (page ?? 1);

            var sanphams = db.Sanphams.OrderBy(s => s.Tensp).ToPagedList(pageNumber, pageSize);
            return View(sanphams);
        }
        // GET: Sanpham
        public ActionResult dtIphonePartial()
        {
            var ip = db.Sanphams.Where(n => n.Mahang == 2).Take(4).ToList();
            return PartialView(ip);
        }
        public ActionResult dtSamSungPartial()
        {
            var ss = db.Sanphams.Where(n => n.Mahang == 1).Take(4).ToList();
            return PartialView(ss);
        }
        public ActionResult dtXiaomiPartial()
        {
            var mi = db.Sanphams.Where(n => n.Mahang == 3).Take(4).ToList();
            return PartialView(mi);
        }
        //public ActionResult dttheohang()
        //{
        //    var mi = db.Sanphams.Where(n => n.Mahang == 5).Take(4).ToList();
        //    return PartialView(mi);
        //}
        public ActionResult xemchitiet(int Masp = 0)
        {
            var chitiet = db.Sanphams.SingleOrDefault(n => n.Masp == Masp);
            if (chitiet == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chitiet);
        }

        public ActionResult Search(int masp = 0, string keyword = "")

        {
            IQueryable<Sanpham> result;

            if (!string.IsNullOrEmpty(keyword))
            {
                result = db.Sanphams.Include(s => s.Hangsanxuat)
                                    .Where(s => s.Tensp.ToUpper().Contains(keyword.ToUpper()));
            }
            else if (masp > 0)
            {
                result = db.Sanphams.Include(s => s.Hangsanxuat)
                                    .Where(s => s.Mahang == masp);
            }
            else
            {
                // Nếu không có từ khóa và mã sản phẩm, trả về toàn bộ sản phẩm
                result = db.Sanphams.Include(s => s.Hangsanxuat);
            }

            // Trả về kết quả tìm kiếm
            var searchResults = result.ToList();

            // Nếu không có sản phẩm nào được tìm thấy, có thể thông báo cho người dùng
            if (!searchResults.Any())
            {
                ViewBag.Message = "Không tìm thấy sản phẩm nào.";
            }

            return View(searchResults);

        }
     
        public ActionResult ShowIphone(string category)
        {
            var Iphone = string.IsNullOrEmpty(category)
                  ? db.Sanphams.Where(n => n.Mahang == 2).Take(4).ToList()
                  : db.Sanphams.Where(n => n.Mahang == 2 && n.Tensp == category).Take(4).ToList();

            return PartialView(Iphone);

        }
        public ActionResult ShowSamsung(string category)
        {
            var Samsung = string.IsNullOrEmpty(category)
                  ? db.Sanphams.Where(n => n.Mahang == 1).Take(4).ToList()
                  : db.Sanphams.Where(n => n.Mahang == 1 && n.Tensp == category).Take(4).ToList();

            return PartialView(Samsung);
        }
        public ActionResult ShowXiaomi(string category)
        {
            var Xiaomi = string.IsNullOrEmpty(category)
                  ? db.Sanphams.Where(n => n.Mahang == 3).Take(4).ToList()
                  : db.Sanphams.Where(n => n.Mahang == 3 && n.Tensp == category).Take(4).ToList();

            return PartialView(Xiaomi);
        }
    }
}
