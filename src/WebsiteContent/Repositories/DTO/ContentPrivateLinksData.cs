using System;

namespace WebsiteContent.Repositories.DTO;

public class ContentPrivateLinksData
{
    public int Id { get; }

    public string InternalCaption { get; }

    /// <summary>
    /// Create <see cref="ContentPrivateLinksData"/>.
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="internalCaption">Internal caption.</param>
    public ContentPrivateLinksData(int id, string internalCaption)
    {
        Id = id;
        InternalCaption = internalCaption ?? throw new ArgumentNullException(nameof(internalCaption));
    } 
}