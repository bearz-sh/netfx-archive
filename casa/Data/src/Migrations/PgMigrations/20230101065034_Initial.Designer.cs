﻿// <auto-generated />
using System;
using Casa.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Casa.Data.Migrations.PgMigrations
{
    [DbContext(typeof(PgCasaDbContext))]
    [Migration("20230101065034_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Casa.Data.Model.Environment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("LoweredName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("lowered_name");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_environments");

                    b.ToTable("environments", (string)null);
                });

            modelBuilder.Entity("Casa.Data.Model.EnvironmentVariable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EnvironmentId")
                        .HasColumnType("integer")
                        .HasColumnName("environment_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_environment_variables");

                    b.HasIndex("EnvironmentId")
                        .HasDatabaseName("ix_environment_variables_environment_id");

                    b.ToTable("environment_variables", (string)null);
                });

            modelBuilder.Entity("Casa.Data.Model.Secret", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EnvironmentId")
                        .HasColumnType("integer")
                        .HasColumnName("environment_id");

                    b.Property<DateTime?>("ExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expires_at");

                    b.Property<string>("JsonTags")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("json_tags");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_secrets");

                    b.HasIndex("EnvironmentId")
                        .HasDatabaseName("ix_secrets_environment_id");

                    b.ToTable("secrets", (string)null);
                });

            modelBuilder.Entity("Casa.Data.Model.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_settings");

                    b.ToTable("settings", (string)null);
                });

            modelBuilder.Entity("Casa.Data.Model.EnvironmentVariable", b =>
                {
                    b.HasOne("Casa.Data.Model.Environment", "Environment")
                        .WithMany("Variables")
                        .HasForeignKey("EnvironmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_environment_variables_environments_environment_id");

                    b.Navigation("Environment");
                });

            modelBuilder.Entity("Casa.Data.Model.Secret", b =>
                {
                    b.HasOne("Casa.Data.Model.Environment", "Environment")
                        .WithMany("Secrets")
                        .HasForeignKey("EnvironmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_secrets_environments_environment_id");

                    b.Navigation("Environment");
                });

            modelBuilder.Entity("Casa.Data.Model.Environment", b =>
                {
                    b.Navigation("Secrets");

                    b.Navigation("Variables");
                });
#pragma warning restore 612, 618
        }
    }
}
