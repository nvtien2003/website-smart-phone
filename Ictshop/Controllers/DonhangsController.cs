using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ictshop.Models;

namespace Ictshop.Controllers
{
    public class DonhangsController : Controller
    {
        private QLdienthoaiEntities db = new QLdienthoaiEntities();

        // GET: Donhangs
        // Hiển thị danh sách đơn hàng
        public ActionResult Index()
        {
            //Kiểm tra đang đăng nhập
           if (Session["use"] == null || Session["use"].ToString() == "")
            {
                return RedirectToAction("GuestOrder");
           }
            Nguoidung kh = (Nguoidung)Session["use"];
            int maND = kh.MaNguoiDung;
            var donhangs = db.Donhangs.Include(d => d.Nguoidung).Where(d=>d.MaNguoidung == maND);
            return View(donhangs.ToList());
        }

        // GET: Donhangs/Details/5
        //Hiển thị chi tiết đơn hàng
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donhang donhang = db.Donhangs.Find(id);
            var chitiet = db.Chitietdonhangs.Include(d => d.Sanpham).Where(d=> d.Madon == id).ToList();
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return View(chitiet);
        }
        // GET: Donhangs/Create
        // Tạo đơn hàng cho người dùng không đăng nhập
        public ActionResult Create()
        {
            ViewBag.MaNguoiDung = new SelectList(db.Nguoidungs, "MaNguoiDung", "Hoten");
            return View();
        }

        // POST: Donhangs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Madon,Ngaydat,Tinhtrang,MaNguoidung,Thanhtoan,Diachinhanhang,Tongtien,GuestEmail")] Donhang donhang)
        {
            if (ModelState.IsValid)
            {
                db.Donhangs.Add(donhang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNguoiDung = new SelectList(db.Nguoidungs, "MaNguoiDung", "Hoten", donhang.MaNguoidung);
            return View(donhang);
        }

        // GET: Donhangs/GuestOrder
        // Trang để người dùng không đăng nhập xem đơn hàng của họ
        public ActionResult GuestOrder()
        {
            return View();
        }

        // POST: Donhangs/GuestOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuestOrder(string email)
        {
            var donhangs = db.Donhangs.Include(d => d.Nguoidung).Where(d => d.Nguoidung.Email == email);
            return View("GuestOrderList", donhangs.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
