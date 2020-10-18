﻿using Data.Mapping.Clients;
using Entities.Clients;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class DbContextAAchallenge : DbContext
    {
        public DbSet<Client> Clients { get; set; }

        public DbContextAAchallenge(DbContextOptions<DbContextAAchallenge> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ClientMap());
        }

    }
}
