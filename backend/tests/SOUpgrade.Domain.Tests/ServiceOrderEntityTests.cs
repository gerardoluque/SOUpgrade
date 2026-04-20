namespace SOUpgrade.Domain.Tests;

public class ServiceOrderEntityTests
{
    [Fact]
    public void ServiceOrder_DefaultStatus_ShouldBePending()
    {
        var order = new SOUpgrade.Domain.Entities.ServiceOrder();
        Assert.Equal(SOUpgrade.Domain.Enums.ServiceOrderStatus.Pending, order.Status);
    }

    [Fact]
    public void ServiceOrder_DefaultPriority_ShouldBeMedium()
    {
        var order = new SOUpgrade.Domain.Entities.ServiceOrder();
        Assert.Equal(SOUpgrade.Domain.Enums.Priority.Medium, order.Priority);
    }
}
