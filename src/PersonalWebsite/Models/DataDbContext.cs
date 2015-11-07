using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalWebsite.Models
{
    public class DataDbContext : DbContext
    {
        public IList<Content> Contents { get; set; }

        public IList<Translation> Translations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var contentEntity = builder.Entity<Content>();
            contentEntity.HasKey(x => x.ContentGuid);
            contentEntity.Property(x => x.ContentGuid).HasDefaultValueSql("newsequentialid()");
            contentEntity.Property(x => x.InternalCaption).IsRequired().HasMaxLength(255);

            var translationEntity = builder.Entity<Translation>();
            translationEntity.HasKey(x => x.Id);
            translationEntity.Index(x => new { x.Version, x.ContentId }).Unique();
            translationEntity.Property(x => x.ContentId).IsRequired();
            translationEntity.Property(x => x.State).IsRequired();
            translationEntity.Property(x => x.Version).IsRequired();
            translationEntity.Property(x => x.Version).HasMaxLength(10);
 
            translationEntity.Property(x => x.UpdatedAt).IsRequired();
            translationEntity.Property(x => x.Title).IsRequired();
            translationEntity.Property(x => x.ContentMarkup).IsRequired();
            translationEntity.Property(x => x.Description).IsRequired();
            translationEntity.Property(x => x.UrlName).IsRequired();
            translationEntity.Property(x => x.UrlName).HasMaxLength(200);
            translationEntity.Index(x => new { x.Version, x.UrlName }).Unique();
        }
    }
}
