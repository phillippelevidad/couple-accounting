﻿// <auto-generated />
using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.Data;

namespace Persistence.Data.Migrations
{
    [DbContext(typeof(AccountingContext))]
    [Migration("20190817230048_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("Persistence.Data.DbCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Persistence.Data.DbPayment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Money>("Amount")
                        .HasColumnType("decimal(11,2)");

                    b.Property<Guid>("CategoryId");

                    b.Property<DateTimeOffset>("DateTime");

                    b.Property<Guid>("SourceId");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("SourceId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Persistence.Data.DbPaymentSource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("PaymentSources");
                });

            modelBuilder.Entity("Persistence.Data.DbPayment", b =>
                {
                    b.HasOne("Persistence.Data.DbCategory", "Category")
                        .WithMany("Payments")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Persistence.Data.DbPaymentSource", "Source")
                        .WithMany("Payments")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
