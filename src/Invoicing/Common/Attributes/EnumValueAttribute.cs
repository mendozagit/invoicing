using System.Diagnostics.CodeAnalysis;

namespace Invoicing.Common.Attributes;

/// <summary>
/// Specifies a value for a enum value.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class EnumValueAttribute : Attribute
{
    /// <summary>
    /// Specifies the default value for the <see cref='Invoicing.Common.Attributes.EnumValueAttribute'/>,
    /// which is an empty string (""). This <see langword='static'/> field is read-only.
    /// </summary>
    public static readonly EnumValueAttribute Default = new EnumValueAttribute();

    public EnumValueAttribute() : this(string.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref='Invoicing.Common.Attributes.EnumValueAttribute'/> class.
    /// The parameter value will be returned when the ToValue() method is called. 
    /// </summary>
    public EnumValueAttribute(string value)
    {
        DescriptionValue = value;
    }

    /// <summary>
    /// Gets the description stored in this attribute.
    /// </summary>
    public virtual string Value => DescriptionValue;

    /// <summary>
    /// Read/Write property that directly modifies the string stored in the description
    /// attribute. The default implementation of the <see cref="Value"/> property
    /// simply returns this value.
    /// </summary>
    protected string DescriptionValue { get; set; }

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is EnumValueAttribute other && other.Value == Value;

    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    public override bool IsDefaultAttribute() => Equals(Default);
}