using Microsoft.AspNetCore.Mvc;

using WebOdev.DataAccess;
using WebOdev.Models;

namespace Odev.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _db.Products.ToList();
            return View(objProductList);
        }
        //GET
        public IActionResult Create()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product obj)
        {
            
            if (ModelState.IsValid)
            {
                _db.Products.Add(obj);
                _db.SaveChanges();
                TempData["Success"] = "Product created";
                return RedirectToAction("Index");
            }
            return View(obj);



        }  //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {

                return NotFound();
            }
            var categoryFromDb = _db.Products.Find(id);

            if (categoryFromDb == null)
            {
                return NotFound();

            }
            return View(categoryFromDb);
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Update(obj);
                _db.SaveChanges();
                TempData["Success"] = "Product edited";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {

                return NotFound();
            }
            var categoryFromDb = _db.Products.Find(id);

            if (categoryFromDb == null)
            {
                return NotFound();

            }
            return View(categoryFromDb);
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Products.Find(id);
            if (obj == null)
            {
                return NotFound();
            }


            _db.Products.Remove(obj);
            _db.SaveChanges();
            TempData["Success"] = "Product deleted";
            return RedirectToAction("Index");

        }

    }
}
