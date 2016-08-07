using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using PersonalWebsite.Models;

namespace PersonalWebsite.Migrations.DataDb
{
    [DbContext(typeof(DataDbContext))]
    [Migration("20160704015003_RenameTablesToFollowNewConvention")]
    partial class RenameTablesToFollowNewConvention
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PersonalWebsite.Models.Content", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("InternalCaption")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Id");

                    b.ToTable("Content");
                });

            modelBuilder.Entity("PersonalWebsite.Models.Translation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ContentId");

                    b.Property<string>("ContentMarkup")
                        .IsRequired();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("State");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("UrlName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200);

                    b.Property<int>("Version")
                        .HasAnnotation("MaxLength", 10);

                    b.HasKey("Id");

                    b.HasIndex("ContentId");

                    b.HasIndex("Version", "ContentId")
                        .IsUnique();

                    b.HasIndex("Version", "UrlName")
                        .IsUnique();

                    b.ToTable("Translation");
                });

            modelBuilder.Entity("PersonalWebsite.Models.Translation", b =>
                {
                    b.HasOne("PersonalWebsite.Models.Content", "Content")
                        .WithMany("Translation")
                        .HasForeignKey("ContentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
