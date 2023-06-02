using API_01.Dtos;
using API_01.Errors;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace API_01.Controllers
{
    public class BasketController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            try
            {
                var customerBasket = await _basketRepository.GetBasketAsync(id);
                if (customerBasket == null)
                {
                    return NotFound(new ApiResponse(404, "Basket not found."));
                }
                return Ok(customerBasket);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(500, "An error occurred."));
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);

            try
            {   
                var CreateOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(mappedBasket);
                if (CreateOrUpdatedBasket == null)
                {
                    return BadRequest(new ApiResponse(400, "Failed to update the basket."));
                }
                return Ok(CreateOrUpdatedBasket);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(500, "An error occurred."));
            }
        }

        [HttpDelete("{basketId}")]
        public async Task<ActionResult<bool>> DeleteBasket(string basketId)
        {
            try
            {
                if (string.IsNullOrEmpty(basketId))
                {
                    return BadRequest(new ApiResponse(400, "Invalid basket ID."));
                }

                var isDeleted = await _basketRepository.DeleteBasketAsync(basketId);
                if (!isDeleted)
                {
                    return NotFound(new ApiResponse(404, "Basket not found."));
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(500, "An error occurred."));
            }
        }
    }
}
