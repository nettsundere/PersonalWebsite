using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using PersonalWebsite.Models;

namespace PersonalWebsite.Migrations
{
    [DbContext(typeof(DataDbContext))]
    partial class DataDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-beta8-15964")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PersonalWebsite.Models.Content", b =>
                {
                    b.Property<Guid>("ContentGuid")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:GeneratedValueSql", "newsequentialid()");

                    b.Property<string>("InternalCaption")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("ContentGuid");
                });

            modelBuilder.Entity("PersonalWebsite.Models.Translation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ContentContentGuid");

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

                    b.HasIndex("Version", "ContentId")
                        .IsUnique();

                    b.HasIndex("Version", "UrlName")
                        .IsUnique();
                });

            modelBuilder.Entity("PersonalWebsite.Models.Translation", b =>
                {
                    b.HasOne("PersonalWebsite.Models.Content")
                        .WithMany()
                        .HasForeignKey("ContentContentGuid");
                });
        }
    }
}
