using ParcelDelivery.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelDelivery.Interfaces
{
    public interface IDeliveryCostCalculator
    {
        decimal CalculateCost(Parcel parcel);

        decimal CalculateOrderCost(
            IEnumerable<Parcel> parcels);
    }
}
