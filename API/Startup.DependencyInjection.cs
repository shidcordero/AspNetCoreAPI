using Data.Contracts;
using Data.Repositories;
using Domain.Contracts;
using Domain.Handlers;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace API
{
    public partial class Startup
    {
        private static void InjectDependencies(IServiceCollection services)
        {
            #region unit of work

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion unit of work


            #region Handlers

            services.AddScoped<ICategoryHandler, CategoryHandler>();
            services.AddScoped<IProductHandler, ProductHandler>();

            #endregion Handlers

            #region Services

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();

            #endregion Services

            #region Repositories

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            #endregion Repositories
        }
    }
}