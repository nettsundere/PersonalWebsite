using System;
using System.Collections.Generic;

namespace WebsiteContent.Repositories.DTO;

public class ContentPrivateEditListData
{
    /// <summary>
    /// Contents.
    /// </summary>
    public IEnumerable<ContentPrivateLinksData> Contents { get; }

    /// <summary>
    /// Create <see cref="ContentPrivateEditListData" />.
    /// </summary>
    /// <param name="contents">Contents.</param>
    public ContentPrivateEditListData(IEnumerable<ContentPrivateLinksData> contents)
    {
        Contents = contents ?? throw new ArgumentNullException(nameof(contents));
    }
}