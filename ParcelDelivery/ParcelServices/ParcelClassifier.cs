using ParcelDelivery.Enums;
using ParcelDelivery.Interfaces;
using ParcelDelivery.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelDelivery.ParcelServices
{
    public class ParcelClassifier : IParcelClassifier
    {
        public ParcelSize Classify(Parcel parcel)
        {
            var largestDimension = Math.Max(
                parcel.Length,
                Math.Max(parcel.Width, parcel.Height));

            if (largestDimension < 10)
                return ParcelSize.Small;

            if (largestDimension < 50)
                return ParcelSize.Medium;

            if (largestDimension < 100)
                return ParcelSize.Large;

            return ParcelSize.XL;
        }
    }
}
