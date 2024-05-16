using Microsoft.EntityFrameworkCore;
using WonderFood.MySql.Context;

namespace WonderFood.BDD.Tests.Common;

public sealed class InMemoryDbContextFactory : IDisposable
{
    private WonderfoodContext _context;

    public WonderfoodContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<WonderfoodContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new WonderfoodContext(options);
        _context.Database.EnsureCreated();
        return _context;
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}