using Data.Mapping.Clients;
using Data.Mapping.Finance.Loans;
using Data.Mapping.Finance.Payments;
using Data.Mapping.Users;
using Entities.Clients;
using Entities.Finance.Loans;
using Entities.Finance.Payments;
using Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class DbContextAAchallenge : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbContextAAchallenge(DbContextOptions<DbContextAAchallenge> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.client)
                .WithMany(c => c.loans)
                .HasForeignKey(l => l.idclient);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.loan)
                .WithMany(l => l.payments)
                .HasForeignKey(p => p.idloan);
            modelBuilder.ApplyConfiguration(new ClientMap());
            modelBuilder.ApplyConfiguration(new LoanMap());
            modelBuilder.ApplyConfiguration(new PaymentMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
        }

    }
}
