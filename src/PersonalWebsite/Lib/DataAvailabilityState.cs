using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.Lib
{
    public enum DataAvailabilityState
    {
        /// <summary>
        /// Published content - visible by all.
        /// </summary>
        published,

        /// <summary>
        /// Draft content - hidden.
        /// </summary>
        draft
    }
}
