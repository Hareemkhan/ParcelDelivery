using ParcelDelivery.Enums;
using ParcelDelivery.Interfaces;
using ParcelDelivery.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelDelivery.ParcelServices
{
    public class DeliveryCostCalculator : IDeliveryCostCalculator
    {
        private readonly IParcelClassifier _classifier;
        private readonly IDeliveryPricingProvider _pricingProvider;

        public DeliveryCostCalculator(
            IParcelClassifier classifier,
            IDeliveryPricingProvider pricingProvider)
        {
            _classifier = classifier ??
                throw new ArgumentNullException(nameof(classifier));

            _pricingProvider = pricingProvider ??
                throw new ArgumentNullException(nameof(pricingProvider));
        }

        public decimal CalculateCost(Parcel parcel)
        {
            ArgumentNullException.ThrowIfNull(parcel);

            var size = _classifier.Classify(parcel);
            var pricing = _pricingProvider.GetPricing();

            return size switch
            {
                ParcelSize.Small => pricing.Small,
                ParcelSize.Medium => pricing.Medium,
                ParcelSize.Large => pricing.Large,
                ParcelSize.XL => pricing.XL,
                _ => throw new ArgumentOutOfRangeException(nameof(size))
            };
        }

        public decimal CalculateOrderCost(
            IEnumerable<Parcel> parcels)
        {
            ArgumentNullException.ThrowIfNull(parcels);

            return parcels.Sum(CalculateCost);
        }
    }
}
