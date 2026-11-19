using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelDelivery.Enums
{

    public sealed record ParcelDeliveryItem(
        ParcelSize ParcelSize,
        decimal DeliveryCost
    );
}
