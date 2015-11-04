using PersonalWebsite.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.Models
{
    public class Translation
    {
        public virtual int Id { get; set; }
        public virtual Language Version { get; set; }
        public virtual DataAvailabilityState State { get; set; }

        public virtual string ContentMarkup { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime UpdatedAt { get; set; }

        public virtual int ContentId { get; set; }
    }
}
