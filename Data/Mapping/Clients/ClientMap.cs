using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Entities.Clients;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping.Clients
{
    public class ClientMap : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("client")
                .HasKey(c => c.idclient);
            builder.Property(c => c.name)
                .HasMaxLength(50);
            builder.Property(c => c.address)
                .HasMaxLength(250);
            builder.Property(c => c.phone_number)
                .HasMaxLength(20);
        }
    }
}
