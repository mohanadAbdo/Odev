using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebOdev.DataAccess;
using WebOdev.DataAccess.Repository.iRepository;
using WebOdev.Models;
using WebOdev.Models.ViewModels;

namespace Odev.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
            return View(objProductList);
        }
        //GET
       
        //POST
 
   
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i=>new SelectListItem
                {
                    Text = i.Name,
                    Value=i.Id.ToString()
                }),
            };
            
            if (id == null || id == 0)
            {
                //ViewData["CategoryList"] = CategoryList;
                return View(productVM);
            }
            else
            {

            }

            return View(productVM);
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                //_unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
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
            var ProductFromDbFirst = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            if (ProductFromDbFirst == null)
            {
                return NotFound();

            }
            return View(ProductFromDbFirst);
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }


            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Product deleted";
            return RedirectToAction("Index");

        }

    }
}
