
using Nop.Core.Configuration;

namespace Nop.Plugin.Shipping.Elta;

/// <summary>
/// Represents settings of the UPS shipping plugin
/// </summary>
public class EltaSettings : ISettings
{
    /// <summary>
    /// Gets or sets a value indicating whether the plugin is enabled
    /// </summary>
    public bool Status { get; set; }
    
    /// <summary>
    /// Gets or sets the flat shipping cost
    /// </summary>
    public decimal Cost { get; set; }

    /// <summary>
    /// Gets or sets the text of plugin
    /// </summary>
    public string Title { get; set; }
}
