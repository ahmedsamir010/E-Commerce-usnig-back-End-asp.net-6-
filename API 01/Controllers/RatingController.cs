using API_01.Dtos;
using API_01.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Specification;

namespace API_01.Controllers
{
    public class RatingController : ApiBaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        public RatingController(IUnitOfWork unitOfWork, IMapper mapper , UserManager<AppUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpPost("AddRating")]
        public async Task<ActionResult<ProductRatingDto>> AddRating(ProductRatingDto productRatingDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            var spec = new ProductWithRatingSpecification(productRatingDto.ProductId, user.Email);
            var existingRating = await unitOfWork.Repository<ProductRating>().GetEntityWithSpecAsync(spec);
            if (existingRating is not null)
            {
                existingRating.RatingValue = productRatingDto.RatingValue;                
                    existingRating.Message = productRatingDto.Message;
                
                unitOfWork.Repository<ProductRating>().Update(existingRating);
            }
            else
            {
                var product = await unitOfWork.Repository<Product>().GetByIdAsync(productRatingDto.ProductId);
                if (product is null) return BadRequest(new ApiResponse(404, "product Not found"));
                var rating = new ProductRating()
                {
                    Email = user.Email,
                    ProductId = productRatingDto.ProductId,
                    RatingValue = productRatingDto.RatingValue,
                    Message = productRatingDto.Message,
                    UserName = user.UserName
                };
                await unitOfWork.Repository<ProductRating>().AddAsync(rating);
            }
            await unitOfWork.Complete();
            return Ok(productRatingDto);
        }

    }
}
