﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RichnessSoft.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichnessSoft.Entity.Configuration
{
    internal class SegmentationConfiguration : IEntityTypeConfiguration<Segmentation>
    {
        public void Configure(EntityTypeBuilder<Segmentation> builder)
        {
            builder.ToTable("segmentation");
            builder.HasKey(m => m.id);
            builder.Property(e => e.code).HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.name1).HasColumnType("varchar(150)").IsRequired();
            builder.Property(e => e.name2).HasColumnType("varchar(150)");
            builder.Property(e => e.active).HasColumnType("varchar(1)").HasDefaultValue("Y").HasComment("active");

            builder.HasOne(p => p.Company).WithMany(p => p.Segmentations).HasForeignKey(e => e.companyid).OnDelete(DeleteBehavior.Restrict);
        }
    }
}