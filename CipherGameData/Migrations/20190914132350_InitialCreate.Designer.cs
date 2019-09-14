﻿// <auto-generated />
using CipherGameData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CipherGameData.Migrations
{
    [DbContext(typeof(CipherGameContext))]
    [Migration("20190914132350_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("CipherGameData.Cipher", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer");

                    b.Property<string>("Place");

                    b.HasKey("Code");

                    b.ToTable("Ciphers");
                });

            modelBuilder.Entity("CipherGameData.GameState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CipherCode");

                    b.Property<bool>("IsAnswerFound");

                    b.Property<bool>("IsPlaceFound");

                    b.Property<int>("Order");

                    b.Property<string>("TeamCode");

                    b.HasKey("Id");

                    b.HasIndex("CipherCode");

                    b.HasIndex("TeamCode");

                    b.ToTable("GameStates");
                });

            modelBuilder.Entity("CipherGameData.Team", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Code");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("CipherGameData.GameState", b =>
                {
                    b.HasOne("CipherGameData.Cipher", "Cipher")
                        .WithMany()
                        .HasForeignKey("CipherCode");

                    b.HasOne("CipherGameData.Team", "Team")
                        .WithMany("GameStates")
                        .HasForeignKey("TeamCode");
                });
#pragma warning restore 612, 618
        }
    }
}
