using AdithyaBank.BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdithyaBank.BackEnd.Extensions
{
    public static class EntityConfiguration
    {
        public static void ConfigureAdithyaBankEntities(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationRegister>().ToTable("ApplicationRegister");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.ApplicationName).IsRequired().HasColumnName("Application_name");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.Applicationfathername).IsRequired().HasColumnName("Application_fathername");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.Applicationmothername).IsRequired().HasColumnName("Application_mothername");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.Applicationdob).IsRequired().HasColumnName("Application_dob");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.Applicationgender).IsRequired().HasColumnName("Application_gender");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.Applicationqualification).IsRequired().HasColumnName("Application_qualification");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.ApplicationMartialStatus).IsRequired().HasColumnName("Application_MartialStatus");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.Applicationmobile).IsRequired().HasColumnName("Application_mobile");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.Applicationemail).IsRequired().HasColumnName("Application_email");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.ApplicationRequestedAmount).IsRequired().HasColumnName("Application_RequestedAmount");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.ApplicationHobbies).IsRequired().HasColumnName("Application_Hobbies");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.ApplicationRegisterdate).IsRequired().HasColumnName("Application_Registerdate");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.ApplicationIsAcceptedTermsandConditions).IsRequired().HasColumnName("Application_IsAcceptedTermsandConditions");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.ApplicationAddress).IsRequired().HasColumnName("Application_Address");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.ApplicationDistrictId).IsRequired().HasColumnName("Application_DistrictId");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.ApplicationStateId).IsRequired().HasColumnName("Application_StateId");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.ApplicationCountryId).IsRequired().HasColumnName("Application_CountryId");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.ApplicationIsApproved).HasColumnName("Application_IsApproved");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.ApplicationApprovedBy).HasColumnName("Application_ApprovedBy");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.ApplicationApprovedOn).HasColumnName("Application_ApprovedOn");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.ApplicationApprovedAmount).HasColumnName("Application_ApprovedAmount");
            modelBuilder.Entity<ApplicationRegister>().Property(c => c.ApplicationStatus).IsRequired().HasColumnName("Application_Status");

            // Countries
            modelBuilder.Entity<Countries>().ToTable("Countries");
            modelBuilder.Entity<Countries>().Property(a => a.CountryCode).HasColumnName("countryCode");
            modelBuilder.Entity<Countries>().Property(a => a.Name).HasColumnName("name");

            //States
            modelBuilder.Entity<States>().ToTable("States");
            modelBuilder.Entity<States>().Property(c => c.Name).HasColumnName("name");
            modelBuilder.Entity<States>().Property(c => c.CountryId).HasColumnName("country_id");

            //Districts
            modelBuilder.Entity<Districts>().ToTable("Districts");
            modelBuilder.Entity<Districts>().Property(c => c.Name).HasColumnName("Name");
            modelBuilder.Entity<Districts>().Property(c => c.StateId).HasColumnName("stateid");

            modelBuilder.Entity<ApplicationRegister>(entity =>
            {

                entity.HasOne(a => a.States)
               .WithMany(a => a.ApplicationRegister)
               .HasForeignKey(a => a.ApplicationStateId)
               .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(a => a.Districts)
               .WithMany(a => a.ApplicationRegister)
               .HasForeignKey(a => a.ApplicationDistrictId)
               .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(a => a.Countries)
              .WithMany(a => a.ApplicationRegister)
              .HasForeignKey(a => a.ApplicationCountryId)
              .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(a => a.ApplicationDocumentUploads)
                .WithOne(a => a.ApplicationRegister)
                   .HasForeignKey<ApplicationDocumentUploads>(a => a.ApplicationId) // Assuming ApplicationRegisterId is the foreign key in ApplicationDocumentUploads
                .OnDelete(DeleteBehavior.ClientSetNull);
                // by this clientset null child that is foreign key table row is not deleted but the Foreign key value become null in child table
                // To delete permanet on both tables    .OnDelete(DeleteBehavior.Cascade); 
                // This is used in dynamic adding deleting AppraisalList screen 
            });
            modelBuilder.Entity<DocumentTypes>(entity =>
            {
                entity.HasMany(a => a.ApplicationDocumentUploads)
                .WithOne(b => b.DocumentTypes)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationDocumentUploads_DocumentTypes");

            });

            modelBuilder.Entity<ApplicationDocumentUploads>(entity => {
                entity.HasOne(a => a.ApplicationRegister)
                .WithOne(b => b.ApplicationDocumentUploads)
                .HasForeignKey<ApplicationDocumentUploads>(c=>c.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationDocumentUploads_ApplicationRegister");

            });
            modelBuilder.Entity<Countries>(entity =>
            {
                entity.HasMany(a => a.States)
                .WithOne(b => b.Countries)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<States>(entity => {

                entity.HasMany(a => a.Districts)
                .WithOne(b => b.States)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Districts>(entity => {
                entity.HasOne(a => a.States)
                .WithMany(a => a.Districts)
                .HasForeignKey(a => a.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}
