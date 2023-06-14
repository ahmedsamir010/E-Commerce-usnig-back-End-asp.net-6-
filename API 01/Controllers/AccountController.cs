using API_01.Dtos;
using API_01.Errors;
using API_01.Extention;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace API_01.Controllers
{
    public class AccountController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("login")] // POST: api/Account/login 
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            // Check if the provided username exists
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                // Delay the response to make it harder for attackers to guess valid usernames
                    await Task.Delay(2000);
                return Unauthorized(new ApiResponse(401, "Invalid credentials."));
            }

            // Attempt to sign in with the provided credentials
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // Generate a token or perform any additional operations upon successful login
                // For example, you can generate a JWT token and return it in the response

                // In this example, we'll return a simplified UserDto as the response
                var userDto = new UserDto
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token =await _tokenService.CreateTokenAsync(user,_userManager)
                    // Include any additional properties you want to return
                };

                return Ok(userDto);
            }
            else if (result.IsLockedOut)
            {
                return Unauthorized(new ApiResponse(401, "Account locked. Please try again later."));
            }

            // Delay the response to make it harder for attackers to guess valid passwords
            await Task.Delay(2000);
            return Unauthorized(new ApiResponse(401, "Invalid credentials."));
        }



        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // Check if the provided email is already registered
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return BadRequest(new ApiResponse(400, "Email is already registered"));
            }

            // Create a new AppUser instance with the provided details
            var newUser = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split("@")[0],
                Address= new Address()
            // Add any additional properties you want to set
        };

            // Attempt to create the user with the provided password
            var result = await _userManager.CreateAsync(newUser, registerDto.Password);
            if (result.Succeeded)
            {
                // Generate a token or perform any additional operations upon successful registration
                // For example, you can generate a JWT token and return it in the response

                // In this example, we'll return a simplified UserDto as the response
                var userDto = new UserDto
                {
                    DisplayName = newUser.DisplayName,
                    Email = newUser.Email,
                    Token =await _tokenService.CreateTokenAsync(newUser, _userManager)
                    
                    // Include any additional properties you want to return
                };

                return Ok(userDto);
            }

            // If user creation failed, return the corresponding error response
            return BadRequest(new ApiResponse(400, "Failed to register user"));
        }
        [Authorize]
        [HttpGet("current-user")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userDto = new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };

            return Ok(userDto);
        }


        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);

            var address=_mapper.Map<Address,AddressDto>(user.Address);

            return Ok(address);
    

        }

        [Authorize]
        [HttpPut("address")]

        public async Task<ActionResult<AddressDto>> updatedUserAddress(AddressDto updatedAddress)
        {

            var address = _mapper.Map<AddressDto, Address>(updatedAddress);



            var user = await _userManager.FindUserWithAddressAsync(User);

            address.Id = user.Address.Id;   
        
            user.Address=address;


            var result=await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(updatedAddress);  

        
        }

        [HttpGet("existemail")]

        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }



    }
}
