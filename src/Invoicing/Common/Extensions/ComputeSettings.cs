using System.Xml.Serialization;
namespace Invoicing.Common.Extensions;

public abstract class ComputeSettings
{
    /// <summary>
    /// Number of decimal places in header fields
    /// </summary>
    [XmlIgnore]
    public int HeaderDecimals { get; set; } = 2;

    /// <summary>
    /// Number of decimal places in items fields
    /// </summary>
    [XmlIgnore]
    public int ItemsDecimals { get; set; } = 6;

    /// <summary>
    /// Rounding strategy used in invoice computation.
    /// </summary>
    [XmlIgnore]
    public MidpointRounding RoundingStrategy { get; set; } = MidpointRounding.AwayFromZero;
}