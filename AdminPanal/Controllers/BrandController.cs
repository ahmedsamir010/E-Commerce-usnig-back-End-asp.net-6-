using AdminPanal.Helpers;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace AdminPanal.Controllers
{
    public class BrandController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public BrandController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var brands = await unitOfWork.Repository<ProductBrand>().GetAllAsync();

            return View(brands);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductBrand brand)
        {
            try
            {
                // Check if the brand name already exists
                var existingBrand = await unitOfWork.Repository<ProductBrand>()
                    .GetEntityWithSpecAsync(new BrandByNameSpecification(brand.Name));

                if (existingBrand != null)
                {
                    ModelState.AddModelError("Name", "Brand name already exists.");
                    return View("Index", await unitOfWork.Repository<ProductBrand>().GetAllAsync());
                }

                await unitOfWork.Repository<ProductBrand>().AddAsync(brand);
                await unitOfWork.Complete();

                return RedirectToAction("Index");


            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the brand.");
                return View(brand);
            }
        }




        public async Task<IActionResult> Delete(int id)
        {
            var brand = await unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);

            if (brand == null)
            {
                return NotFound(); // Return 404 Not Found if the brand doesn't exist
            }

            unitOfWork.Repository<ProductBrand>().Delete(brand);
            await unitOfWork.Complete();

            return RedirectToAction("Index");
        }


    }
}
