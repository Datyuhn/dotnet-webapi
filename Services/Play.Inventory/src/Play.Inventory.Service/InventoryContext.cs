using Microsoft.EntityFrameworkCore;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Context
{
    public class InventoryContext : DbContext
    {
        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }

        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {

        }
    }
}