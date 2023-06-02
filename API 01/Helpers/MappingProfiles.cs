using API_01.Dtos;
using AutoMapper;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregrate;

namespace API_01.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>())
                .ForMember(p => p.productRatings, o => o.MapFrom(P => P.productRatings))
                .ForMember(a => a.AverageRating, o => o.MapFrom(a => a.productRatings.Any() ? a.productRatings.Average(p => p.RatingValue) : 0));

            CreateMap<ProductRating, ProductRatingDto>().ReverseMap();

            CreateMap<AddressDto, Talabat.Core.Entities.Order_Aggregrate.Address>().ReverseMap();

            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();

            CreateMap<BasketItemDto, BasketItem>().ReverseMap();

            CreateMap<Talabat.Core.Entities.Identity.Address, AddressDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItems, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl));



        }
    }
}
