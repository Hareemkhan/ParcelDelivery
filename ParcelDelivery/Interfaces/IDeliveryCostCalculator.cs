using ParcelDelivery.Enums;
using ParcelDelivery.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelDelivery.Interfaces
{
    public interface IDeliveryCostCalculator
    {
        decimal CalculateCost(Parcel parcel);

        DeliveryQuote CalculateOrderCost(
        IEnumerable<Parcel> parcels,
        ShippingSpeed shippingSpeed = ShippingSpeed.Standard);
    }
}
