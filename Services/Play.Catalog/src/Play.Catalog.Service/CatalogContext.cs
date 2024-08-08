using Microsoft.EntityFrameworkCore;
using System;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service
{
    public class CatalogContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        public string DbPath { get; }

        public CatalogContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "Catalog.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
