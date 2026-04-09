namespace SOUpgrade.Application.Tests;

public class ServiceOrderHandlerTests
{
    [Fact]
    public void GetAllServiceOrdersQuery_ShouldBeCreatable()
    {
        var query = new SOUpgrade.Application.Features.ServiceOrders.Queries.GetAllServiceOrders.GetAllServiceOrdersQuery();
        Assert.NotNull(query);
    }

    [Fact]
    public void GetServiceOrderByIdQuery_ShouldStoreId()
    {
        var id = Guid.NewGuid();
        var query = new SOUpgrade.Application.Features.ServiceOrders.Queries.GetServiceOrderById.GetServiceOrderByIdQuery(id);
        Assert.Equal(id, query.Id);
    }
}
