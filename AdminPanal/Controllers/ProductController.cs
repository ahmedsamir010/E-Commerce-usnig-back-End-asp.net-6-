using AdminDashboard.Models;
using AdminPanal.Helpers;
using API_01.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Specification;
using Talabat.Repository;

namespace AdminPanal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProductsSpecParams specParams, string search)
        {
            var pagination = await GetProductPagination(specParams, search);
            return View(pagination);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var specParams = new ProductsSpecParams
            {
                PageIndex = 1,
                PageSize = 5
            };

            var pagination = await GetProductPagination(specParams);
            return View(pagination);
        }

        public async Task<IActionResult> Search(string search, int pageIndex = 1, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                ModelState.AddModelError("search", "Please enter a search term.");
            }

            var specParams = new ProductsSpecParams
            {
                Search = search,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var pagination = await GetProductPagination(specParams, search);
            return View("Index", pagination);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                await UploadAndSaveProduct(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await GetProductById(id);
            var mappedProduct = mapper.Map<Product, ProductViewModel>(product);
            return View(mappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                await UploadAndSaveProduct(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await GetProductById(id);
            var mappedProduct = mapper.Map<Product, ProductViewModel>(product);
            return View(mappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, ProductViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            try
            {
                var product = await GetProductById(id);
                await DeleteProduct(product);
                return RedirectToAction("Index");
            }
            catch
            {
                // Handle exception if needed
                return RedirectToAction("Index");
            }
        }

        private async Task<Pagination<ProductViewModel>> GetProductPagination(ProductsSpecParams specParams, string search = null)
        {
            var spec = new ProductWithBrandTyprSpecification(specParams);
            var products = await unitOfWork.Repository<Product>().GetAllWithAsync(spec);

            var data = mapper.Map<IReadOnlyList<ProductViewModel>>(products);

            var countSpec = new Product_With_Filteration_For_Count_Spec(specParams);
            var count = await unitOfWork.Repository<Product>().GetCountWithSpecAsync(countSpec);

            var pagination = new Pagination<ProductViewModel>(specParams.PageIndex, specParams.PageSize, count, data);

            return pagination;
        }

        private async Task<Product> GetProductById(int id)
        {
            var product = await unitOfWork.Repository<Product>().GetByIdAsync(id);
            return product;
        }

        private async Task UploadAndSaveProduct(ProductViewModel model)
        {
            if (model.Image != null)
                model.PictureUrl = PictureSettings.UploadFile(model.Image, "products");
            else
                model.PictureUrl = "images/products/product-not-found.jpg";

            var mappedProduct = mapper.Map<ProductViewModel, Product>(model);
            await unitOfWork.Repository<Product>().AddAsync(mappedProduct);
            await unitOfWork.Complete();
        }

        private async Task DeleteProduct(Product product)
        {
            if (product.PictureUrl != null)
                PictureSettings.DeleteFile(product.PictureUrl, "products");

            unitOfWork.Repository<Product>().Delete(product);
            await unitOfWork.Complete();
        }
    }
}
