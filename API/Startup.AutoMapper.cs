using AutoMapper;
using Data.Models.Entities;
using Data.ViewModels.Category;
using Data.ViewModels.Product;
using Microsoft.Extensions.DependencyInjection;

namespace API
{
    public partial class Startup
    {
        private static void ConfigureMapper(IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryViewModel>()
                    .ForMember(x => x.CategoryName, opt => opt.MapFrom(y => y.CategoryName.Trim()))
                    .ReverseMap();
                cfg.CreateMap<Product, ProductViewModel>()
                    .ForMember(x => x.ProductName, opt => opt.MapFrom(y => y.ProductName.Trim()))
                    .ReverseMap();
            });

            services.AddSingleton(sp => mapperConfiguration.CreateMapper());
        }
    }
}