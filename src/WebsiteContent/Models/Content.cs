using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebsiteContent.Models;

/// <summary>
/// The content.
/// </summary>
public class Content
{
    /// <summary>
    /// Content unique id.
    /// </summary>
    public int Id { get; set; }
        
    /// <summary>
    /// Available content translations.
    /// </summary>
    public IList<Translation> Translations { get; set; } = null!;

    /// <summary>
    /// Content non-translated caption.
    /// </summary>
    [Required]
    public string InternalCaption { get; set; } = null!;
}