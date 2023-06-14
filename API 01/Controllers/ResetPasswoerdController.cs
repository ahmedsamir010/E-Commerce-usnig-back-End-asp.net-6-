using API_01.Dtos;
using API_01.Errors;
using API_01.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
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

                // HTML template for the email body
                var emailBody = $@"
                    <html>
                    <body style='text-align: center;'>
                        <div style='background-color: #f4f4f4; padding: 20px; display: inline-block;'>
                            <h2 style='color: #333; font-size: 18px; margin-bottom: 10px;'>Reset Password</h2>
                            <p style='color: #555; font-size: 16px;'>Hello {user.UserName},</p>
                            <p style='color: #555; font-size: 16px;'>Please click the button below to reset your password:</p>
                            <div style='text-align: center; margin: 20px 0;'>
                                <a href='{confirmLink}' style='background-color: #007bff; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                                    Reset Password
                                </a>
                            </div>
                            <p style='color: #555; font-size: 16px;'>If you didn't request a password reset, please ignore this email.</p>
                            <p style='color: #555; font-size: 16px;'>Thank you,</p>
                            <p style='color: #555; font-size: 16px;'>Market Team</p>
                        </div>
                    </body>
                    </html>
                ";

                var subject = "Reset Password";

                var email = new Email
                {
                    To = user.Email,
                    Subject = subject,
                    Body = emailBody,
                    IsHtml = true // Set IsHtml to true to indicate that the email body contains HTML
                };

                await emailService.SendEmailAsync(email);

                return Ok("Reset Password Email Sent");
            }
            else
            {
                return NotFound(new ApiResponse(404, "Email Doesn't Exist"));
            }
        }

        //[HttpPost("sms")]
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
