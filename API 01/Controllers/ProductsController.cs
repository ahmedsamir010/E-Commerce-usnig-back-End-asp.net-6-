using API_01.Dtos;
using API_01.Errors;
using API_01.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specification;
using Talabat.Repository;

namespace API_01.Controllers
{
   
    public class ProductsController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(
            IUnitOfWork unitOfWork
            ,IMapper mapper
           
            )
        {
            _unitOfWork=unitOfWork;
            _mapper = mapper;
        }
         

        [HttpGet]
        //[Cached(100)]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductsSpecParams productsSpecParams)
        {
            var spec=new ProductWithBrandTyprSpecification(productsSpecParams);

            var products=await _unitOfWork.Repository<Product>().GetAllWithAsync(spec);

            var data= (_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
            var countSpec = new Product_With_Filteration_For_Count_Spec(productsSpecParams);

            var count=await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(countSpec);


            return Ok(new Pagination<ProductToReturnDto>(productsSpecParams.PageIndex ,productsSpecParams.PageSize,count,data));
        }
       

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ProductToReturnDto), 404)]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProduct(int id)
        {
            var spec = new ProductWithBrandTyprSpecification(id);

            var products = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);

            if(products ==null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product,ProductToReturnDto>(products));
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

            return Ok(brands);
        }

        [HttpGet("types")]  
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();

            return Ok(types);
        }





    }

}
