using ParcelDelivery.Enums;
using ParcelDelivery.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelDelivery.Interfaces
{
    public interface IDeliveryCostCalculator
    {

        DeliveryQuote CalculateOrderCost(
        IEnumerable<Parcel> parcels,
        ShippingSpeed shippingSpeed = ShippingSpeed.Standard);
    }
}
