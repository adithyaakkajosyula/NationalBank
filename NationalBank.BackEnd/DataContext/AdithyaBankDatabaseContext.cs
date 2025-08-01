using NationalBank.BackEnd.Entities;
using NationalBank.BackEnd.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NationalBank.BackEnd.DataContext
{
    public class NationalBankDatabaseContext : DbContext
    {
        public NationalBankDatabaseContext(DbContextOptions<NationalBankDatabaseContext> options) : base(options)
        {

        }
        public DbSet<ApplicationRegister> ApplicationRegister { get; set; }
        public DbSet<DocumentTypes> DocumentTypes { get; set; }
        public DbSet<ApplicationDocumentUploads> ApplicationDocumentUploads { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<Districts> Districts { get; set; }
        public DbSet<Product> Product { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureNationalBankEntities();
            base.OnModelCreating(modelBuilder);
            
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = this.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged);
            foreach (var entry in this.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged))
            {
                var type = entry.Entity.GetType().BaseType;
                if (entry.Entity.GetType().BaseType == typeof(BaseEntity))
                {
                    var entity = (BaseEntity)entry.Entity;
                    if (entry.State == EntityState.Deleted)
                    {
                        entity.Rowstate = 3;
                        this.Entry(entity).State = EntityState.Modified;
                    }
                    entity.ModifiedOn = DateTime.Now;
                    entity.ModifiedBy = 1;
                }
                else
                {
                    throw new NotImplementedException($"{entry.Entity.GetType().Name} not derived from base.");
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
