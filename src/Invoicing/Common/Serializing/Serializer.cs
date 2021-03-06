using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Invoicing.Common.Extensions;

namespace Invoicing.Common.Serializing;

/// <summary>
/// XML serializer helper class. Serializes and deserializes objects from/to XML
/// </summary>
/// <typeparam name="T">The type of the object to serialize/deserialize.
/// Must have a parameterless constructor and implement <see cref="Serializable"/></typeparam>
public class Serializer<T> where T : class, new()
{
    /// <summary>
    /// Deserializes a XML string into an object
    /// </summary>
    /// <param name="xml">The XML string to deserialize</param>
    /// <param name="encoding">The encoding</param>
    /// <param name="settings">XML serialization settings. <see cref="System.Xml.XmlReaderSettings"/></param>
    /// <returns>An object of type <c>T</c></returns>
    public static T Deserialize(string xml, Encoding encoding, XmlReaderSettings settings)
    {
        if (string.IsNullOrEmpty(xml))
            throw new ArgumentException("XML cannot be null or empty", nameof(xml));

        var xmlSerializer = new XmlSerializer(typeof(T));

        using var memoryStream = new MemoryStream(encoding.GetBytes(xml));
        using var xmlReader = XmlReader.Create(memoryStream, settings);
        return (T) xmlSerializer.Deserialize(xmlReader);
    }


    /// <summary>
    /// Deserializes a XML file.
    /// </summary>
    /// <param name="filename">The filename of the XML file to deserialize</param>
    /// <param name="settings">XML serialization settings. <see cref="System.Xml.XmlReaderSettings"/></param>
    /// <returns>An object of type <c>T</c></returns>
    public static T DeserializeFromFile(string filename, XmlReaderSettings settings)
    {
        if (string.IsNullOrEmpty(filename))
            throw new ArgumentException("filename", "XML filename cannot be null or empty");

        if (!File.Exists(filename))
            throw new FileNotFoundException("Cannot find XML file to deserialize", filename);

        // Create the stream writer with the specified encoding
        using var reader = XmlReader.Create(filename, settings);
        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        return (T) xmlSerializer.Deserialize(reader);
    }


    /// <summary>
    /// Serialize an object
    /// </summary>
    /// <param name="source">The object to serialize</param>
    /// <param name="namespaces">Namespaces to include in serialization</param>
    /// <param name="settings">XML serialization settings. <see cref="System.Xml.XmlWriterSettings"/></param>
    /// <returns>A XML string that represents the object to be serialized</returns>
    public static string Serialize(T source, XmlSerializerNamespaces namespaces, XmlWriterSettings settings)
    {
        if (source == null)
            throw new ArgumentNullException("source", "Object to serialize cannot be null");

        var serializer = new XmlSerializer(source.GetType());
        using var memoryStream = new MemoryStream();
        using var xmlWriter = XmlWriter.Create(memoryStream, settings);
        var xmlSerializer = new XmlSerializer(typeof(T));

        xmlSerializer.Serialize(xmlWriter, source, namespaces);
        memoryStream.Position = 0; // rewind the stream before reading back.

        using var sr = new StreamReader(memoryStream);
        var xml = sr.ReadToEnd();

        return xml;
    }

    /// <summary>
    /// Used to serialize in memory the invoice complements.
    /// </summary>
    /// <param name="source">objectComplement</param>
    /// <param name="namespaces">complement namespaces</param>
    /// <returns>complement as XmlElement</returns>
    public static XElement? SerializeElement(object source, XmlSerializerNamespaces namespaces)
    {
        var doc = new XDocument();
        var serializer = new XmlSerializer(source.GetType());

        using (var writer = doc.CreateWriter())
            serializer.Serialize(writer, source, namespaces);


        return doc.Root?.RemoveNamespaceDeclaration();
    }


    //Implemented based on interface, not part of algorithm
    public static string RemoveAllNamespaces(string xmlDocument)
    {
        XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

        return xmlDocumentWithoutNs.ToString();
    }

    //Core recursion function
    private static XElement RemoveAllNamespaces(XElement xmlDocument)
    {
        if (!xmlDocument.HasElements)
        {
            XElement xElement = new XElement(xmlDocument.Name.LocalName);
            xElement.Value = xmlDocument.Value;

            foreach (XAttribute attribute in xmlDocument.Attributes())
                xElement.Add(attribute);

            return xElement;
        }

        return new XElement(xmlDocument.Name.LocalName,
            xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
    }


    private XElement? clenXmlDocument(XDocument doc)
    {
        // All elements with an empty namespace...
        foreach (var node in doc.Root.Descendants()
                     .Where(n => n.Name.NamespaceName == ""))
        {
            // Remove the xmlns='' attribute. Note the use of
            // Attributes rather than Attribute, in case the
            // attribute doesn't exist (which it might not if we'd
            // created the document "manually" instead of loading
            // it from a file.)
            node.Attributes("xmlns").Remove();
            // Inherit the parent namespace instead
            node.Name = node.Parent.Name.Namespace + node.Name.LocalName;
        }

        return doc.Root;
    }


    /// <summary>
    /// Serialize an object to a XML file
    /// </summary>
    /// <param name="source">The object to serialize</param>
    /// <param name="filename">The file to generate</param>
    /// <param name="namespaces">Namespaces to include in serialization</param>
    /// <param name="settings">XML serialization settings. <see cref="System.Xml.XmlWriterSettings"/></param>
    public static void SerializeToFile(T source, string filename, XmlSerializerNamespaces namespaces,
        XmlWriterSettings settings)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source), "Object to serialize cannot be null");

        var serializer = new XmlSerializer(source.GetType());
        using var xmlWriter = XmlWriter.Create(filename, settings);
        var xmlSerializer = new XmlSerializer(typeof(T));
        xmlSerializer.Serialize(xmlWriter, source, namespaces);
    }


    /// <summary>
    /// Used to deserialize the invoice complements
    /// </summary>
    /// <typeparam name="T">target type</typeparam>
    /// <param name="element">complement as XmlElement</param>
    /// <returns></returns>
    public static T? DeserializeFromXmlElement<T>(XmlElement element)
    {
        var serializer = new XmlSerializer(typeof(T));

        return (T) serializer.Deserialize(new XmlNodeReader(element))!;
    }
}