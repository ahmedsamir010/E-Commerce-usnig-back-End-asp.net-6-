﻿using API_01.Dtos;
using API_01.Errors;
using API_01.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.EmailSettings;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.Service;

namespace API_01.Controllers
{
    public class ResetPasswordController : ApiBaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly ISmsServeice smsService;

        public ResetPasswordController(UserManager<AppUser> userManager, IEmailService emailService, ISmsServeice smsService)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.smsService = smsService;
        }

        [HttpPost]
        public async Task<ActionResult> ForgetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user is not null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var encodeToken = Encoding.UTF8.GetBytes(token);
                var newToken = WebEncoders.Base64UrlEncode(encodeToken);
                var confirmLink = $"http://localhost:4200/passwordconfirm?ID={user.Id}&Token={newToken}";
                var emailBody = $"Please click the following link to reset your password: {confirmLink}";
                var subject = "Reset Password";

                try
                {
                    var email = new Email
                    {
                        To = user.Email,
                        Subject = subject,
                        Body = emailBody
                    };

                    await emailService.SendEmailAsync(email);

                    return Ok("Reset Password Email Sent");
                }
                catch (Exception ex)
                {
                    // Handle exception occurred during email sending
                    // You can log the exception or return an appropriate error message
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while sending the reset password email.");
                }
            }
            else
            {
                return NotFound(new ApiResponse(404, "Email Doesn't Exist"));
            }
        }


        //[HttpPost]
        //public async Task<IActionResult> SendSmsAsync(ResetPasswordDto resetPasswordDto)
        //{
        //    var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);
        //    if (user is not null)
        //    {
        //        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        //        var resetPasswordLink = Url.Action("ResetPassword", "Account", new { Email = resetPasswordDto.Email, Token = token }, Request.Scheme);
        //        var sms = new SmsMessages
        //        {
        //            PhoneNumber = user.PhoneNumber,
        //            Body = resetPasswordLink
        //        };
        //        smsService.Send(sms);
        //    }
        //    else
        //    {
        //        return NotFound(new ApiResponse(404, "Email Doesn't Exist"));
        //    }

        //    return Ok("Check Your Phone");
        //}
    }
}