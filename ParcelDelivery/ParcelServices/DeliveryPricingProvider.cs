using ParcelDelivery.Interfaces;
using ParcelDelivery.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelDelivery.ParcelServices
{
    public class DeliveryPricingProvider : IDeliveryPricingProvider
    {
        private readonly DeliveryPricing _pricing;

        public DeliveryPricingProvider(
            DeliveryPricing pricing)
        {
            _pricing = pricing ??
                throw new ArgumentNullException(nameof(pricing));
        }

        public DeliveryPricing GetPricing()
        {
            return _pricing;
        }
    }
}
