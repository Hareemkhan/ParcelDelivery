using Microsoft.Extensions.DependencyInjection;
using ParcelDelivery.Interfaces;
using ParcelDelivery.Models;
using ParcelDelivery.ParcelServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelDelivery.DependencyInjections
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddParcelDelivery(
            this IServiceCollection services,
            DeliveryPricing pricing)
        {
            ArgumentNullException.ThrowIfNull(pricing);

            services.AddSingleton<IParcelClassifier,
                ParcelClassifier>();

            services.AddSingleton<IDeliveryPricingProvider>(
                new DeliveryPricingProvider(pricing));

            services.AddSingleton<IDeliveryCostCalculator,
                DeliveryCostCalculator>();

            return services;
        }
    }
}
