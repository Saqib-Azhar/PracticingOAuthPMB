using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Practicing_OAuth.Models;
using System.IO;
using PagedList;
using PagedList.Mvc;

namespace Practicing_OAuth.Controllers
{
    public class ProductsController : Controller
    {
        class searchClass
        {
            public List<Product> searchedProdList { get; set; }
            public string searchedQuery { get; set; }
        }
        private Practicing_OAuthEntities db = new Practicing_OAuthEntities();
        private static List<Product> ProductsList = new List<Product>();
        private static searchClass searchObjectsList = new searchClass();

        [Authorize(Roles = "Admin")]
        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

          Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var Categories = db.Categories.Where(s => s.Id == product.CategoryId).ToList();
            ViewBag.CategoriesList = Categories;
            var Products = db.Products.Where(s => s.CategoryId == product.CategoryId).ToList();
            ViewBag.ProductsList = Products;
            return View(product);
        }

        public ActionResult Search(string query, int? pageNo = 1, int? pageSize = 16)
        {
            try
            {
                if (searchObjectsList.searchedProdList == null || (searchObjectsList.searchedProdList.Count == 0 || searchObjectsList.searchedQuery != query))
                {
                    searchObjectsList.searchedProdList = db.Products.Where(s => s.Name.Contains(query) && s.IsEnabled == true).ToList();
                    searchObjectsList.searchedQuery = query;
                }
                ViewBag.QueryString = query;
                return View(searchObjectsList.searchedProdList.ToPagedList(Convert.ToInt32(pageNo), Convert.ToInt32(pageSize)));


            }
            catch (Exception ex)
            {
                return RedirectToAction("NoResultFound","Products",query);
            }
        }
        public ActionResult NoResultFound(string query)
        {
            ViewBag.QueryString = query;
            return View();
        }
        [Authorize(Roles = "Admin")]
        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName");
            return View();
        }
        [Authorize(Roles = "Admin")]
        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,SlugURL,Description,IsEnabled,Image1,Image2,Image3,Image4,Image5,UploadedDate,CategoryId")] Product product, HttpPostedFileBase Image1, HttpPostedFileBase Image2, HttpPostedFileBase Image3, HttpPostedFileBase Image4, HttpPostedFileBase Image5)
        {
            if (Image1 != null)
            {
                string pic = System.IO.Path.GetFileName(Image1.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/UploadedProductImages"), pic);
                // file is uploaded
                Image1.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    Image1.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
                product.Image1 = Image1.FileName;

            }
            if (Image2 != null)
            {
                string pic = System.IO.Path.GetFileName(Image2.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/UploadedProductImages"), pic);
                // Image2 is uploaded
                Image2.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    Image2.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
                product.Image2 = Image2.FileName;

            }
            if (Image3 != null)
            {
                string pic = System.IO.Path.GetFileName(Image3.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/UploadedProductImages"), pic);
                // Image3 is uploaded
                Image3.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    Image3.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
                product.Image3 = Image3.FileName;

            }
            if (Image4 != null)
            {
                string pic = System.IO.Path.GetFileName(Image4.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/UploadedProductImages"), pic);
                // Image4 is uploaded
                Image4.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    Image4.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

                product.Image4 = Image4.FileName;
            }
            if (Image5 != null)
            {
                string pic = System.IO.Path.GetFileName(Image5.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/UploadedProductImages"), pic);
                // Image5 is uploaded
                Image5.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    Image5.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

                product.Image5 = Image5.FileName;
            }
            if (ModelState.IsValid)
            {
                product.SlugURL = product.Name.Replace(" ", "_").ToLower();
                product.UploadedDate = DateTime.Now;
                db.Products.Add(product);
                db.SaveChanges();
                return Redirect("/Products/Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", product.CategoryId);
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,SlugURL,Description,IsEnabled,Image1,Image2,Image3,Image4,Image5,UploadedDate,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("/Products/Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", product.CategoryId);
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return Redirect("/Products/Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult productsByCategoryType(int id, int? pageNo = 1, int? pageSize = 16)
        {
            if (ProductsList.Count == 0)
            {
                var products = db.Products.ToList();
                ProductsList = products;
            }
            ViewBag.Id = id;
            var ProdSubList = ProductsList.Where(s => s.Category.CategoryTypeId == id && s.IsEnabled == true);
            return View(ProdSubList.ToPagedList(Convert.ToInt32(pageNo), Convert.ToInt32(pageSize)));

        }
        public ActionResult productsByCategory(int id, int? pageNo = 1, int? pageSize = 16)
        {
            if (ProductsList.Count == 0)
            {
                var products = db.Products.ToList();
                ProductsList = products;
            }
            ViewBag.Id = id;
            var ProdSubList = ProductsList.Where(s => s.CategoryId == id && s.IsEnabled == true);
            return View(ProdSubList.ToPagedList(Convert.ToInt32(pageNo), Convert.ToInt32(pageSize)));
        }

        public ActionResult AllProducts(int? pageNo = 1,int? pageSize = 16)
        {
            if (ProductsList.Count == 0)
            {
                var products = db.Products.ToList();
                ProductsList = products;
            }
            return View(ProductsList.ToPagedList(Convert.ToInt32(pageNo), Convert.ToInt32(pageSize)));
        }
    }
}
