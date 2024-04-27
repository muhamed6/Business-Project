using Microsoft.Extensions.DependencyInjection;
using Route.C41.G02.BLL.Interfaces;
using Route.C41.G02.BLL.Repositories;
using Route.C41.G02.PL.Services.EmailSender;

namespace Route.C41.G02.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {



            
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            return services;

        }
    }
}
