using API_01.Errors;
using API_01.Helpers;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Service;

namespace API_01.Extention
{
    public static class ApplicationServicesExtention
    {
        public static IServiceCollection AddApplicationSevices(this IServiceCollection services)
        {   


            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            // builder.Services.AddScoped<IGenericRepository<Product>, IGenericRepository<Product>>();


            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped(typeof(IResponseCacheService), typeof(ResponseCacheService));

            //builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            services.AddAutoMapper(typeof(MappingProfiles));


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                       .SelectMany(P => P.Value.Errors)
                                                       .Select(E => E.ErrorMessage)
                                                       .ToList();

                    var validationErroreResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors

                    };

                    return new BadRequestObjectResult(validationErroreResponse);
                };

            });



            return services;
        }
    }
}
