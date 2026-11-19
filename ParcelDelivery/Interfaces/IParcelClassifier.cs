using ParcelDelivery.Enums;
using ParcelDelivery.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelDelivery.Interfaces
{
    public interface IParcelClassifier
    {
        ParcelSize Classify(Parcel parcel);
    }
}
