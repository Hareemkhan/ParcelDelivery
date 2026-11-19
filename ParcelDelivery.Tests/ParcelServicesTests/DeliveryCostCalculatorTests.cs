using ParcelDelivery.Enums;
using ParcelDelivery.Interfaces;
using ParcelDelivery.Models;
using ParcelDelivery.ParcelServices;

namespace ParcelDelivery.Tests.ParcelServicesTests;

[TestClass]
public sealed class DeliveryCostCalculatorTests
{
    [TestMethod]
    public void Constructor_WithNullClassifier_ThrowsArgumentNullException()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
            new DeliveryCostCalculator(null!, new FakeDeliveryPricingProvider()));
    }

    [TestMethod]
    public void Constructor_WithNullPricingProvider_ThrowsArgumentNullException()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
            new DeliveryCostCalculator(new FakeParcelClassifier(), null!));
    }

    [TestMethod]
    public void CalculateCost_WithSmallParcel_ReturnsSmallPrice()
    {
        AssertCalculateCostForSize(ParcelSize.Small, 3.25m);
    }

    [TestMethod]
    public void CalculateCost_WithMediumParcel_ReturnsMediumPrice()
    {
        AssertCalculateCostForSize(ParcelSize.Medium, 7.50m);
    }

    [TestMethod]
    public void CalculateCost_WithLargeParcel_ReturnsLargePrice()
    {
        AssertCalculateCostForSize(ParcelSize.Large, 12.75m);
    }

    [TestMethod]
    public void CalculateCost_WithXLParcel_ReturnsXLPrice()
    {
        AssertCalculateCostForSize(ParcelSize.XL, 21.00m);
    }

    [TestMethod]
    public void CalculateCost_WithInvalidParcelSize_ThrowsArgumentOutOfRangeException()
    {
        var classifier = new FakeParcelClassifier { SizeToReturn = (ParcelSize)999 };
        var provider = new FakeDeliveryPricingProvider();
        var calculator = new DeliveryCostCalculator(classifier, provider);

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            calculator.CalculateCost(new Parcel(1m, 1m, 1m)));
    }

    [TestMethod]
    public void CalculateCost_WithNullParcel_ThrowsArgumentNullException()
    {
        var calculator = new DeliveryCostCalculator(
            new FakeParcelClassifier(),
            new FakeDeliveryPricingProvider());

        Assert.ThrowsExactly<ArgumentNullException>(() =>
            calculator.CalculateCost(null!));
    }

    [TestMethod]
    public void CalculateCost_WithNullPricingResult_ThrowsNullReferenceException()
    {
        var classifier = new FakeParcelClassifier { SizeToReturn = ParcelSize.Small };
        var provider = new FakeDeliveryPricingProvider { PricingToReturn = null };
        var calculator = new DeliveryCostCalculator(classifier, provider);

        Assert.ThrowsExactly<NullReferenceException>(() =>
            calculator.CalculateCost(new Parcel(1m, 1m, 1m)));
    }

    [TestMethod]
    public void CalculateOrderCost_WithEmptyCollection_ReturnsZero()
    {
        var classifier = new FakeParcelClassifier();
        var calculator = new DeliveryCostCalculator(
            classifier,
            new FakeDeliveryPricingProvider());

        var result = calculator.CalculateOrderCost(Array.Empty<Parcel>());

        Assert.AreEqual(0m, result);
        Assert.AreEqual(0, classifier.CallCount);
    }

    [TestMethod]
    public void CalculateOrderCost_WithSingleParcel_ReturnsParcelCost()
    {
        var classifier = new FakeParcelClassifier { SizeToReturn = ParcelSize.Medium };
        var calculator = new DeliveryCostCalculator(
            classifier,
            new FakeDeliveryPricingProvider());

        var result = calculator.CalculateOrderCost(
            new[] { new Parcel(10m, 10m, 10m) });

        Assert.AreEqual(7.50m, result);
        Assert.AreEqual(1, classifier.CallCount);
    }

    [TestMethod]
    public void CalculateOrderCost_WithMixedParcels_ReturnsAggregatedCost()
    {
        var classifier = new SequenceParcelClassifier(ParcelSize.Small, ParcelSize.Medium, ParcelSize.XL);
        var calculator = new DeliveryCostCalculator(
            classifier,
            new FakeDeliveryPricingProvider());
        var parcels = new[]
        {
            new Parcel(1m, 1m, 1m),
            new Parcel(10m, 10m, 10m),
            new Parcel(100m, 100m, 100m)
        };

        var result = calculator.CalculateOrderCost(parcels);

        Assert.AreEqual(31.75m, result);
        Assert.AreEqual(3, classifier.CallCount);
        CollectionAssert.AreEquivalent(parcels, classifier.ClassifiedParcels);
    }

    [TestMethod]
    public void CalculateOrderCost_WithNullCollection_ThrowsArgumentNullException()
    {
        var calculator = new DeliveryCostCalculator(
            new FakeParcelClassifier(),
            new FakeDeliveryPricingProvider());

        Assert.ThrowsExactly<ArgumentNullException>(() =>
            calculator.CalculateOrderCost(null!));
    }

    private static void AssertCalculateCostForSize(ParcelSize size, decimal expected)
    {
        var parcel = new Parcel(1m, 1m, 1m);
        var classifier = new FakeParcelClassifier { SizeToReturn = size };
        var provider = new FakeDeliveryPricingProvider();
        var calculator = new DeliveryCostCalculator(classifier, provider);

        var result = calculator.CalculateCost(parcel);

        Assert.AreEqual(expected, result);
        Assert.AreEqual(1, classifier.CallCount);
        Assert.AreSame(parcel, classifier.LastParcel);
    }

    private sealed class FakeParcelClassifier : IParcelClassifier
    {
        public ParcelSize SizeToReturn { get; set; } = ParcelSize.Small;
        public int CallCount { get; private set; }
        public Parcel? LastParcel { get; private set; }

        public ParcelSize Classify(Parcel parcel)
        {
            CallCount++;
            LastParcel = parcel;
            return SizeToReturn;
        }
    }

    private sealed class SequenceParcelClassifier : IParcelClassifier
    {
        private readonly Queue<ParcelSize> _sizes;

        public SequenceParcelClassifier(params ParcelSize[] sizes)
        {
            _sizes = new Queue<ParcelSize>(sizes);
        }

        public int CallCount { get; private set; }
        public List<Parcel> ClassifiedParcels { get; } = new();

        public ParcelSize Classify(Parcel parcel)
        {
            CallCount++;
            ClassifiedParcels.Add(parcel);
            return _sizes.Dequeue();
        }
    }

    private sealed class FakeDeliveryPricingProvider : IDeliveryPricingProvider
    {
        public DeliveryPricing? PricingToReturn { get; set; } = new DeliveryPricing
        {
            Small = 3.25m,
            Medium = 7.50m,
            Large = 12.75m,
            XL = 21.00m
        };

        public DeliveryPricing GetPricing()
        {
            return PricingToReturn!;
        }
    }
}