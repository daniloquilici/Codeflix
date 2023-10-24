﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using quilici.Codeflix.Domain.Entity;

namespace quilici.Codeflix.Infra.Data.EF.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(255);
            builder.Property(x => x.Description).HasMaxLength(10_000);
        }
    }
}
