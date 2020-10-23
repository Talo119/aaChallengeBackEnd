using Data.Mapping.Clients;
using Data.Mapping.Finance.Loans;
using Entities.Clients;
using Entities.Finance.Loans;
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
            modelBuilder.ApplyConfiguration(new ClientMap());
            modelBuilder.ApplyConfiguration(new LoanMap());
        }

    }
}
