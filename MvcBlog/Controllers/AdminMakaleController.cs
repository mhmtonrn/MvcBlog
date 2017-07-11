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
    public class AdminMakaleController : Controller
    {
        mvcblogDB db = new mvcblogDB();
        // GET: AdminMakale
        public ActionResult Index()
        {
            var makale = db.Makales.ToList();
            return View(makale);
        }

        // GET: AdminMakale/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminMakale/Create
        public ActionResult Create()
        {
            ViewBag.KategoriID = new SelectList(db.Kategoris, "KategoriId", "KategoriAdi");
            return View();
        }

        // POST: AdminMakale/Create
        [HttpPost]
        public ActionResult Create(Makale makale, string etiketler,HttpPostedFileBase foto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                     if (foto!=null)
                {
                    WebImage img = new WebImage(foto.InputStream);
                    FileInfo fotoInfo = new FileInfo(foto.FileName);
                    string newFoto = Guid.NewGuid().ToString() + fotoInfo.Extension;

                    img.Resize(800, 3500);
                    img.Save("~/Uploads/MakaleFoto/" + newFoto);
                    makale.Foto = "~/Uploads/MakaleFoto/" + newFoto;
                    
                    
                }

                if (etiketler!=null)
                {
                    string[] etiketdizi = etiketler.Split(',');
                    foreach (var item in etiketdizi)
                    {
                        var yenietiket = new Etiket{EtiketAdi=item};
                        db.Etikets.Add(yenietiket);
                        makale.Etikets.Add(yenietiket);
                    }

                }
                db.Makales.Add(makale);
                db.SaveChanges();

                // TODO: Add insert logic here

                return RedirectToAction("Index");
                }
            }
            catch
            {
                return View(makale);
            }
            return View(makale);
        }

        // GET: AdminMakale/Edit/5
        public ActionResult Edit(int id)
        {

            var makale = db.Makales.Where(m => m.MakaleId == id).SingleOrDefault();
            if (makale==null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriId = new SelectList(db.Kategoris, "KategoriId", "KategoriAdi",makale.KategoriId);//son yaptıgımız seçili olarak gelmesi için
            return View(makale);
        }

        // POST: AdminMakale/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, HttpPostedFileBase foto,Makale makale)
        {
            try
            {
                var makales = db.Makales.Where(m => m.MakaleId == id).SingleOrDefault();
                if (foto!=null)
                {
                    if (System.IO.File.Exists(Server.MapPath(makales.Foto)))
                    {
                        System.IO.File.Delete(Server.MapPath(makales.Foto));
                      
                    }
                    WebImage img = new WebImage(foto.InputStream);
                    FileInfo fotoInfo = new FileInfo(foto.FileName);
                    string newFoto = Guid.NewGuid().ToString() + fotoInfo.Extension;

                    img.Resize(800, 3500);
                    img.Save("~/Uploads/MakaleFoto/" + newFoto);
                    makales.Foto = "~/Uploads/MakaleFoto/" + newFoto;
                    makales.Baslik = makale.Baslik;
                    makales.Icerik = makale.Icerik;
                    makales.KategoriId = makale.KategoriId;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(makale);
            }
        }

        // GET: AdminMakale/Delete/5
        public ActionResult Delete(int id)
        {
            var makale = db.Makales.Where(m => m.MakaleId==id).SingleOrDefault();
            if(makale==null){
                return HttpNotFound();
            }
            return View(makale);
        }

        // POST: AdminMakale/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {

            try
            {
                var makales = db.Makales.Where(m => m.MakaleId == id).SingleOrDefault();
                if (makales == null)
                {
                    return HttpNotFound();
                }


                if (System.IO.File.Exists(Server.MapPath(makales.Foto)))
                {
                    System.IO.File.Delete(Server.MapPath(makales.Foto));

                }
                foreach (var item in makales.Yorums.ToList())
                {
                    db.Yorums.Remove(item);
                }
                foreach (var item in makales.Etikets.ToList())
                {
                    db.Etikets.Remove(item);
                }
                db.Makales.Remove(makales);
                db.SaveChanges();

                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
