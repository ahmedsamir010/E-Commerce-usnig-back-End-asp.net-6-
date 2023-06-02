using API_01.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Repository.Data;

namespace API_01.Controllers
{
    public class BuggyController : ApiBaseController
    {
        private readonly StoreContext _dbContext;

        public BuggyController(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet("notfound")]

        public ActionResult GetNotFoundRequest()
        {
            var product = _dbContext.Products.Find(900);
            if(product == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(product); 
        }

        [HttpGet("servererror")]

        public ActionResult GetServerError()
        {
            var product = _dbContext.Products.Find(9000);
            var productToReturn = product.ToString();
                
            return Ok(productToReturn);
        }

        [HttpGet("badrequest")]

        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }


        [HttpGet("badrequest/{id}")]

        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }



    }
}
