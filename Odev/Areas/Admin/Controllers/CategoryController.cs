using Microsoft.AspNetCore.Mvc;

using WebOdev.DataAccess;
using WebOdev.DataAccess.Repository.iRepository;
using WebOdev.Models;

namespace Odev.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _db;
        public CategoryController(ICategoryRepository db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.GetAll();
            return View(objCategoryList);
        }
        //GET
        public IActionResult Create()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Cant match The name");


            }
            if (ModelState.IsValid)
            {
                _db.Add(obj);
                _db.Save();
                TempData["Success"] = "Category created";
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
            //var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDbFirst = _db.GetFirstOrDefault(u=>u.Id==id);
            if (categoryFromDbFirst == null)
            {
                return NotFound();

            }
            return View(categoryFromDbFirst);
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Cant match The name");


            }
            if (ModelState.IsValid)
            {
                _db.Update(obj);
                _db.Save();
                TempData["Success"] = "Category edited";
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
            //var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDbFirst = _db.GetFirstOrDefault(u=>u.Id==id);

            if (categoryFromDbFirst == null)
            {
                return NotFound();

            }
            return View(categoryFromDbFirst);
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }


            _db.Remove(obj);
            _db.Save();
            TempData["Success"] = "Category deleted";
            return RedirectToAction("Index");

        }

    }
}
