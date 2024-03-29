﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Migrations
{
    [DbContext(typeof(RivaIdentityDbContext))]
    [Migration("20191104153631_Add_Roles_Table")]
    partial class Add_Roles_Table
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Riva.Identity.Infrastructure.DataAccess.EntityFramework.Entities.RoleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("6c26269b-ba53-4f41-93c3-f7b23f9e77e0"),
                            Name = "User"
                        },
                        new
                        {
                            Id = new Guid("d41a1ec6-cf79-4f24-99de-d146f0d6369f"),
                            Name = "Administrator"
                        },
                        new
                        {
                            Id = new Guid("34c01e87-f0c1-45f9-b78e-f812f40c023c"),
                            Name = "System"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
