﻿using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public class ApplicationService
    {
        public ApplicationService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProductImportService, ProductImportService>();
        }

    }
}
