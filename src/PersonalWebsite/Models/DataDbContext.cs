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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var contentEntity = builder.Entity<Content>();
            contentEntity.Key(x => x.ContentGuid);
            contentEntity.Property(x => x.ContentGuid).DefaultExpression("newsequentialid()");
            contentEntity.Property(x => x.InternalCaption).Required().MaxLength(255);

            var translationEntity = builder.Entity<Translation>();
            translationEntity.Key(x => x.Id);
            translationEntity.Index(x => new { x.Version, x.ContentId });
            translationEntity.Property(x => x.ContentId).Required();
            translationEntity.Property(x => x.State).Required();
            translationEntity.Property(x => x.Version).Required();
            translationEntity.Property(x => x.UpdatedAt).Required();
            translationEntity.Property(x => x.Title).Required();
            translationEntity.Property(x => x.ContentMarkup).Required();
            translationEntity.Property(x => x.Description).Required();
        }
    }
}
