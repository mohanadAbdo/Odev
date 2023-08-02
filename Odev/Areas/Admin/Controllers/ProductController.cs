using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebOdev.DataAccess;
using WebOdev.DataAccess.Repository.iRepository;
using WebOdev.Models;
using WebOdev.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Odev.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;

        }
        public IActionResult Index()
        {
 IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
            return View();
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
                productVM.Product=_unitOfWork.Product.GetFirstOrDefault(u=>u.Id == id);
                return View(productVM);
            }

            
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj,IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(obj.Product.ImageUrl);
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    obj.Product.ImageUrl = @"images\products" + fileName + extension;
                }
                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
                _unitOfWork.Save();
                TempData["Success"] = "Product created";
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


      

        #region API CALLLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties:"Category");
            return Json(new { data = productList });
        }

        //POST
        [HttpDelete]
        
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { seccess = false, message = "Error" });
            }
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath,obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { seccess = true, message = "Success" });

        }
        #endregion

    }

}
