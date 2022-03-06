using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Contracts.Persistence.Base;
using Ordering.Application.Models;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;
using Ordering.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure
{
    public static class InfraStructureServiceRegisteration
    {
        public static IServiceCollection AddInfrstructureServices(this IServiceCollection services,IConfiguration configurtion)
        {
            services.AddDbContext<OrderContext>(options =>
                                                options.UseSqlServer(configurtion.GetConnectionString("OrderingConnectionString")));

            services.AddScoped(typeof(IDeletableAsyncRepository<>),typeof(RepositoryBase<>));
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.Configure<EmailSettings>(c => configurtion.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}
