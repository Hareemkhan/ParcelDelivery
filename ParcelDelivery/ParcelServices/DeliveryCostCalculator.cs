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

        public DeliveryQuote CalculateOrderCost(
            IEnumerable<Parcel> parcels, ShippingSpeed shippingSpeed = ShippingSpeed.Standard)
        {
            ArgumentNullException.ThrowIfNull(parcels);

            if (!Enum.IsDefined(shippingSpeed))
                throw new ArgumentOutOfRangeException(nameof(shippingSpeed));

            var parcelList = parcels.ToList();

            var pricing = _pricingProvider.GetPricing();

            // Calculate each parcel's normal delivery cost.
            var parcelItems = parcelList.Select(parcel =>
            {
                var size = _classifier.Classify(parcel);

                var cost = size switch
                {
                    ParcelSize.Small => pricing.Small,
                    ParcelSize.Medium => pricing.Medium,
                    ParcelSize.Large => pricing.Large,
                    ParcelSize.XL => pricing.XL,
                    _ => throw new ArgumentOutOfRangeException()
                };

                return new ParcelDeliveryItem(size, cost);

            }).ToList();

            // Calculate standard delivery subtotal.
            var standardCost = parcelItems.Sum(x => x.DeliveryCost);

            // Speedy shipping is an additional order-level charge.
            var speedyCost = shippingSpeed == ShippingSpeed.Speedy
                ? standardCost
                : 0m;

            // Calculate the final delivery cost.
            var totalCost = standardCost + speedyCost;

            return new DeliveryQuote(
                parcelItems,
                standardCost,
                speedyCost,
                totalCost,
                shippingSpeed
            );
        } 


    }
}
