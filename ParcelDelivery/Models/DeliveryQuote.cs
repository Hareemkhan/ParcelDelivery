using ParcelDelivery.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelDelivery.Models
{
    public sealed record DeliveryQuote(
        IReadOnlyList<ParcelDeliveryItem> Parcels,
        decimal StandardDeliveryCost,
        decimal SpeedyShippingCost,
        decimal TotalDeliveryCost,
        ShippingSpeed ShippingSpeed
    );
}
