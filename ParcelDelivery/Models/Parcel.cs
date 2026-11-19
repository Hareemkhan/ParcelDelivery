using System;
using System.Collections.Generic;
using System.Text;

namespace ParcelDelivery.Models
{
    public class Parcel
    {
        public decimal Length { get; }
        public decimal Width { get; }
        public decimal Height { get; }

        public Parcel(decimal length, decimal width, decimal height)
        {
            if (length <= 0 || width <= 0 || height <= 0)
                throw new ArgumentException(
                    "Parcel dimensions must be greater than zero.");

            Length = length;
            Width = width;
            Height = height;
        }
    }
}
