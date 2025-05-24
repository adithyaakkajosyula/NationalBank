using AdithyaBank.BackEnd.Entities;
using AdithyaBank.BackEnd.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdithyaBank.BackEnd.DataContext
{
    public class AdithyaBankDatabaseContext : DbContext
    {
        public AdithyaBankDatabaseContext(DbContextOptions<AdithyaBankDatabaseContext> options) : base(options)
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
            modelBuilder.ConfigureAdithyaBankEntities();
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
                    else
                    {
                        entity.Rowstate = Convert.ToByte(entry.State == EntityState.Added ? 1 : 2);
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
