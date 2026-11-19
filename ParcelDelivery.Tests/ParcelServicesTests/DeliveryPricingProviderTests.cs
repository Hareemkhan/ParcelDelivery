using ParcelDelivery.Models;
using ParcelDelivery.ParcelServices;

namespace ParcelDelivery.Tests.ParcelServicesTests;

[TestClass]
public sealed class DeliveryPricingProviderTests
{
    [TestMethod]
    public void Constructor_WithValidPricing_Succeeds()
    {
        var pricing = new DeliveryPricing { Small = 3.25m };

        var provider = new DeliveryPricingProvider(pricing);

        Assert.IsNotNull(provider);
    }

    [TestMethod]
    public void Constructor_WithNullPricing_ThrowsArgumentNullException()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() =>
            new DeliveryPricingProvider(null!));
    }

    [TestMethod]
    public void GetPricing_WithConfiguredPricing_ReturnsSuppliedInstance()
    {
        var pricing = new DeliveryPricing
        {
            Small = 3.25m,
            Medium = 7.50m,
            Large = 12.75m,
            XL = 21.00m
        };
        var provider = new DeliveryPricingProvider(pricing);

        var result = provider.GetPricing();

        Assert.AreEqual(pricing, result);
    }

    [TestMethod]
    public void GetPricing_ReturnedInstance_IsSameReference()
    {
        var pricing = new DeliveryPricing();
        var provider = new DeliveryPricingProvider(pricing);

        var result = provider.GetPricing();

        Assert.AreSame(pricing, result);
    }
}