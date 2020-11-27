﻿// <auto-generated />
using System;
using Commerce.Amazon.Web.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Commerce.Amazon.Web.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Commerce.Amazon.Web.Repositories.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CountNotifyPerDay");

                    b.Property<int>("CountUsersCanNotify");

                    b.Property<int>("CoutUsers");

                    b.Property<int>("MaxDays");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("State");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Commerce.Amazon.Web.Repositories.GroupUser", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("GroupId");

                    b.HasKey("UserId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupUsers");
                });

            modelBuilder.Entity("Commerce.Amazon.Web.Repositories.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateCreate")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<int>("GroupId");

                    b.Property<decimal?>("Prix");

                    b.Property<int>("State");

                    b.Property<string>("Url")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Commerce.Amazon.Web.Repositories.PostPlaning", b =>
                {
                    b.Property<int>("IdPost");

                    b.Property<int>("IdUser");

                    b.Property<string>("Comment");

                    b.Property<DateTime?>("DateComment");

                    b.Property<DateTime?>("DateLimite");

                    b.Property<DateTime?>("DateNotified");

                    b.Property<DateTime?>("DatePlanifie");

                    b.Property<string>("PathScreenComment");

                    b.Property<int>("State");

                    b.HasKey("IdPost", "IdUser");

                    b.HasIndex("IdUser");

                    b.ToTable("PostPlanings");
                });

            modelBuilder.Entity("Commerce.Amazon.Web.Repositories.Societe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Logo");

                    b.Property<string>("Name");

                    b.Property<int?>("State");

                    b.HasKey("Id");

                    b.ToTable("Societes");
                });

            modelBuilder.Entity("Commerce.Amazon.Web.Repositories.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<int>("GroupId");

                    b.Property<string>("Nom");

                    b.Property<string>("Password");

                    b.Property<string>("Photo");

                    b.Property<string>("Prenom");

                    b.Property<int?>("Role");

                    b.Property<int>("SocieteId");

                    b.Property<int>("State");

                    b.Property<string>("Telephon");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("SocieteId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Commerce.Amazon.Web.Repositories.GroupUser", b =>
                {
                    b.HasOne("Commerce.Amazon.Web.Repositories.Group", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Commerce.Amazon.Web.Repositories.User", "User")
                        .WithMany("Groups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Commerce.Amazon.Web.Repositories.Post", b =>
                {
                    b.HasOne("Commerce.Amazon.Web.Repositories.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Commerce.Amazon.Web.Repositories.PostPlaning", b =>
                {
                    b.HasOne("Commerce.Amazon.Web.Repositories.Post", "Post")
                        .WithMany("Planings")
                        .HasForeignKey("IdPost")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Commerce.Amazon.Web.Repositories.User", "User")
                        .WithMany("PostsAchat")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Commerce.Amazon.Web.Repositories.User", b =>
                {
                    b.HasOne("Commerce.Amazon.Web.Repositories.Societe", "Societe")
                        .WithMany()
                        .HasForeignKey("SocieteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
