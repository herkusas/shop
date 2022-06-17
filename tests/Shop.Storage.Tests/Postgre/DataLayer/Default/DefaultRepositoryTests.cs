using System.Data;
using FluentAssertions;
using Npgsql;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shop.Storage.Postgre.DataLayer.Connection;
using Shop.Storage.Postgre.DataLayer.Default;
using Shop.Storage.Wrappers;
using Xunit;

namespace Shop.Storage.Tests.Postgre.DataLayer.Default;

public class DefaultRepositoryTests
{
    [Fact]
    public async void GetAllClients_WhenThereIsClient_ShouldReturnThem()
    {
        //arrange
        var dapperWrapper = Substitute.For<IQueryExecutor>();
        const string ConnectionString = "HOST=::1;PORT=5432;DATABASE=shop;Uid=postgres;Pwd=admin;";
        var cnn = Substitute.For<IConnectionFactory>();
        cnn.Create().Returns(new NpgsqlConnection(ConnectionString));
        const string ExpectedQuery = @"SELECT * FROM clients";
        const string ExpectedClientName = "Bob";
        var expectedClient = new ClientRecord { Name = ExpectedClientName };
        var expectedClients = new List<ClientRecord> { expectedClient };
        dapperWrapper.QueryAsync<ClientRecord?>(Arg.Is<IDbConnection>(x => x.ConnectionString == ConnectionString), ExpectedQuery)
            .Returns(expectedClients);
        var repo = new DefaultRepository(cnn, dapperWrapper);
        
        //act
        var result = await repo.GetAllClients();
        
        //assert
        result!.FirstOrDefault().Should().BeOfType<Client>().Which.Name.Should().Be(ExpectedClientName);
    }

    [Fact]
    public async void GetAllClients_WhenThereIsNoClients_ShouldReturnNull()
    {
        //arrange
        var dapperWrapper = Substitute.For<IQueryExecutor>();
        const string ConnectionString = "HOST=::1;PORT=5432;DATABASE=shop;Uid=postgres;Pwd=admin;";
        var cnn = Substitute.For<IConnectionFactory>();
        cnn.Create().Returns(new NpgsqlConnection(ConnectionString));
        const string ExpectedQuery = @"SELECT * FROM clients";
        const string ExpectedClientName = "Bob";
        dapperWrapper.QueryAsync<ClientRecord?>(Arg.Is<IDbConnection>(x => x.ConnectionString == ConnectionString), ExpectedQuery)
            .ReturnsNull();
        var repo = new DefaultRepository(cnn, dapperWrapper);
        
        //act
        var result = await repo.GetAllClients();
        
        //assert
        result.Should().BeNull();
    }
}
