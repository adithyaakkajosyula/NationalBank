﻿using NationalBank.BackEnd.Entities;
using NationalBank.BackEnd.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NationalBank.BackEnd.DataContext
{
    public class NationalBankIdentityDbContext : IdentityDbContext<User>
    {
        public NationalBankIdentityDbContext(DbContextOptions<NationalBankIdentityDbContext> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("AspNetUsers");
            modelBuilder.Entity<Role>().ToTable("AspNetRoles");
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasMany(a => a.Users)
                .WithOne(b => b.Role)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            });
            base.OnModelCreating(modelBuilder);

        }
    }
}
