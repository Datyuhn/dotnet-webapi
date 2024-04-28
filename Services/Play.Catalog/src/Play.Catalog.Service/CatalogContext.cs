using Microsoft.EntityFrameworkCore;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Context
{
    public class CatalogContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {

        }
    }
}