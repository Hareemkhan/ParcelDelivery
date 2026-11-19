using ParcelDelivery.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelDelivery.Interfaces
{
    public interface IDeliveryPricingProvider
    {
        DeliveryPricing GetPricing();
    }
}
