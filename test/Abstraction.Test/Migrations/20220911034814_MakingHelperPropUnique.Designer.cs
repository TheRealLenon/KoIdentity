﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tekoding.KoIdentity.Abstraction.Test.Helper;

#nullable disable

namespace Tekoding.KoIdentity.Abstraction.Test.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20220911034814_MakingHelperPropUnique")]
    partial class MakingHelperPropUnique
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("KoIdentity.Abstraction.Tests")
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Tekoding.KoIdentity.Abstraction.Test.Helper.BaseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TestProp")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("TestProp")
                        .IsUnique()
                        .HasFilter("[TestProp] IS NOT NULL");

                    b.ToTable("BaseEntity", "KoIdentity.Abstraction.Tests");
                });
#pragma warning restore 612, 618
        }
    }
}
