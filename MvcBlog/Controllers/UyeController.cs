using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBlog.Models;
using System.Web.Helpers;
using System.IO;

namespace MvcBlog.Controllers
{
    public class UyeController : Controller
    {
        mvcblogDB db = new mvcblogDB();
        // GET: Uye
        public ActionResult Index(int id)
        {
            Uye uye = db.Uyes.Where(m => m.UyeId == id).SingleOrDefault();
            if (Convert.ToInt32(Session["uyeId"])!=uye.UyeId)
            {
                return HttpNotFound();

            }
            return View(uye);
        }

        [HttpPost]
        public ActionResult Login(Uye uye)
        {
            try 
            {
                var login = db.Uyes.Where(m => m.KullaniciAdi == uye.KullaniciAdi).SingleOrDefault();
                if (login.KullaniciAdi == uye.KullaniciAdi && login.Email == uye.Email && login.Sifre == uye.Sifre)
                {
                    Session["uyeId"] = login.UyeId;
                    Session["kullaniciAdi"] = login.KullaniciAdi;
                    Session["yetkiId"] = login.Yetki;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Uyari = "Şifrelerinizi kontrol edin";
                }
            }
            catch {
                ViewBag.Uyari = "Kullanıcı Mevcut değil";
            }
            
            
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["uyeId"] = null;
            Session["kullaniciAdi"] = null;
            Session["yetkiId"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Uye uye, HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                if (Foto!=null)
                {
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoInfo = new FileInfo(Foto.FileName);
                    string newFoto = Guid.NewGuid().ToString() + fotoInfo.Extension;

                    img.Resize(150, 150);
                    img.Save("~/Uploads/UyeFoto/" + newFoto);
                    uye.Foto = "~/Uploads/UyeFoto/" + newFoto;
                    uye.Yetki= 2;
                    db.Uyes.Add(uye);
                    db.SaveChanges();
                    Session["uyeId"] = uye.UyeId;
                    Session["kullaniciAdi"] = uye.KullaniciAdi;
                    return RedirectToAction("Index","Home");
                }
            }
            else
            {
                ModelState.AddModelError("Fotoğraf", "Fotoğraf Seçiniz");
            }
            return View(uye);
        }

        [HttpPost]
        public ActionResult Edit(Uye uye,int id,HttpPostedFileBase Foto) {
            Uye uyes = db.Uyes.Where(m => m.UyeId == id).SingleOrDefault();
            if (ModelState.IsValid)
            {

                if (Foto != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(uyes.Foto)))
                    {
                        System.IO.File.Delete(Server.MapPath(uyes.Foto));

                    }
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoInfo = new FileInfo(Foto.FileName);
                    string newFoto = Guid.NewGuid().ToString() + fotoInfo.Extension;

                    img.Resize(150, 150);
                    img.Save("~/Uploads/UyeFoto/" + newFoto);
                    uyes.Foto = "~/Uploads/UyeFoto/" + newFoto;

                }

                uyes.AdSoyad = uye.AdSoyad;
                uyes.KullaniciAdi = uye.KullaniciAdi;
                uyes.Sifre = uye.Sifre;
                uyes.Email = uye.Email;
                db.SaveChanges();
                Session["kullaniciAdi"] = uye.KullaniciAdi;
                return RedirectToAction("Index", "Home", new { id = uyes.UyeId });
            }
            return View();

        }

        
        public ActionResult Edit(int id)
        {
            Uye uye = db.Uyes.Where(m => m.UyeId == id).SingleOrDefault();
            if (Convert.ToInt32(Session["uyeId"])!=uye.UyeId)
            {
                return HttpNotFound();
            }
            return View(uye);

        }
    }
}