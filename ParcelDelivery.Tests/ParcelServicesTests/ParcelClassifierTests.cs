using ParcelDelivery.Enums;
using ParcelDelivery.Models;
using ParcelDelivery.ParcelServices;

namespace ParcelDelivery.Tests.ParcelServicesTests;

[TestClass]
public sealed class ParcelClassifierTests
{
    [TestMethod]
    public void Classify_WithSmallParcel_ReturnsSmallSize()
    {
        var classifier = new ParcelClassifier();
        var parcel = new Parcel(9.999m, 9.999m, 9.999m);

        var result = classifier.Classify(parcel);

        Assert.AreEqual(ParcelSize.Small, result);
    }

    [TestMethod]
    public void Classify_WithMediumParcel_ReturnsMediumSize()
    {
        var classifier = new ParcelClassifier();
        var parcel = new Parcel(1m, 49.999m, 1m);

        var result = classifier.Classify(parcel);

        Assert.AreEqual(ParcelSize.Medium, result);
    }

    [TestMethod]
    public void Classify_WithLargeParcel_ReturnsLargeSize()
    {
        var classifier = new ParcelClassifier();
        var parcel = new Parcel(99.999m, 1m, 1m);

        var result = classifier.Classify(parcel);

        Assert.AreEqual(ParcelSize.Large, result);
    }

    [TestMethod]
    public void Classify_WithXLParcel_ReturnsXLSize()
    {
        var classifier = new ParcelClassifier();
        var parcel = new Parcel(100m, 100m, 100m);

        var result = classifier.Classify(parcel);

        Assert.AreEqual(ParcelSize.XL, result);
    }

    [TestMethod]
    public void Classify_AtEachBoundary_ReturnsExpectedSize()
    {
        var classifier = new ParcelClassifier();

        Assert.AreEqual(ParcelSize.Medium, classifier.Classify(new Parcel(10m, 10m, 10m)));
        Assert.AreEqual(ParcelSize.Large, classifier.Classify(new Parcel(50m, 50m, 50m)));
        Assert.AreEqual(ParcelSize.XL, classifier.Classify(new Parcel(100m, 100m, 100m)));
    }

    [TestMethod]
    public void Classify_JustBelowBoundary_ReturnsLowerSize()
    {
        var classifier = new ParcelClassifier();

        Assert.AreEqual(ParcelSize.Small, classifier.Classify(new Parcel(9.999m, 9.999m, 9.999m)));
        Assert.AreEqual(ParcelSize.Medium, classifier.Classify(new Parcel(49.999m, 49.999m, 49.999m)));
        Assert.AreEqual(ParcelSize.Large, classifier.Classify(new Parcel(99.999m, 99.999m, 99.999m)));
    }

    [TestMethod]
    public void Classify_JustAboveBoundary_ReturnsHigherSize()
    {
        var classifier = new ParcelClassifier();

        Assert.AreEqual(ParcelSize.Medium, classifier.Classify(new Parcel(10.001m, 10.001m, 10.001m)));
        Assert.AreEqual(ParcelSize.Large, classifier.Classify(new Parcel(50.001m, 50.001m, 50.001m)));
        Assert.AreEqual(ParcelSize.XL, classifier.Classify(new Parcel(100.001m, 100.001m, 100.001m)));
    }

    [TestMethod]
    public void Classify_WithNullParcel_ThrowsNullReferenceException()
    {
        var classifier = new ParcelClassifier();

        Assert.ThrowsExactly<NullReferenceException>(() => classifier.Classify(null!));
    }
}