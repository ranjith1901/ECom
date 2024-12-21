using Clarion.Ecom.API.IRepository;
using Clarion.Ecom.API.Repository;

namespace Clarion.Ecom.API.Extensions
{
    public static class BuilderExtensions
    {
        public static IServiceCollection APIServices(this IServiceCollection services)
        {

            services.AddTransient<IEmail, EmailRepo>();
            services.AddTransient<ILoginRepo, LoginRepo>();
            services.AddTransient<ITravelTypeRepo, TravelTypeRepo>();
            services.AddTransient<ITravelDurationRepo, TravelDurationRepo>();
            return services;
        }
    }
}
