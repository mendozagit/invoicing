using System.Text;
using System.Xml;
using Invoicing.Base;
using Invoicing.Common.Contracts;
using Invoicing.Common.Extensions;
using Invoicing.Common.Serializing;

namespace Invoicing.Servicies;

public class InvoiceService : IComputable
{
    private readonly Invoice invoice;

    public InvoiceService()
    {
        invoice = new Invoice();
    }


    /// <summary>
    /// Add invoiceItem to current invoice object
    /// </summary>
    /// <param name="invoiceItem">invoice item to be added</param>
    public void AddInvoiceItem(InvoiceItem invoiceItem)
    {
        //_invoice.InvoiceItems ??= new List<InvoiceItem>();
        invoice.InvoiceItems.Add(invoiceItem);
    }

    /// <summary>
    /// Add invoiceItem to current invoice object
    /// </summary>
    /// <param name="satItemId">ClaveProdServ:Atributo requerido para expresar la clave del producto o del servicio amparado por el presente concepto. Es requerido y deben utilizar las claves del catálogo de productos y servicios, cuando los conceptos que registren por sus actividades correspondan con dichos conceptos.</param>
    /// <param name="itemId">NoIdentificacion:Atributo opcional para expresar el número de parte, identificador del producto o del servicio, la clave de producto o servicio, SKU o equivalente, propia de la operación del emisor, amparado por el presente concepto. Opcionalmente se puede utilizar claves del estándar GTIN.</param>
    /// <param name="quantity">Cantidad:Atributo requerido para precisar la cantidad de bienes o servicios del tipo particular definido por el presente concepto.</param>
    /// <param name="unitOfMeasureId">ClaveUnidad:Atributo requerido para precisar la clave de unidad de medida estandarizada aplicable para la cantidad expresada en el concepto. La unidad debe corresponder con la descripción del concepto.</param>
    /// <param name="unitOfMeasure">Unidad:Atributo opcional para precisar la unidad de medida propia de la operación del emisor, aplicable para la cantidad expresada en el concepto. La unidad debe corresponder con la descripción del concepto.</param>
    /// <param name="description">Descripción:Atributo requerido para precisar la descripción del bien o servicio cubierto por el presente concepto.</param>
    /// <param name="unitCost">ValorUnitario:Atributo requerido para precisar el valor o precio unitario del bien o servicio cubierto por el presente concepto.</param>
    /// <param name="amount">Importe:Atributo requerido para precisar el importe total de los bienes o servicios del presente concepto. Debe ser equivalente al resultado de multiplicar la cantidad por el valor unitario expresado en el concepto. No se permiten valores negativos.</param>
    /// <param name="discount">Descuento:Atributo opcional para representar el importe de los descuentos aplicables al concepto. No se permiten valores negativos.</param>
    /// <param name="taxObjectId">ObjetoImp:Atributo requerido para expresar si la operación comercial es objeto o no de impuesto.</param>
    public void AddInvoiceItem(
        string satItemId,
        string itemId,
        decimal quantity,
        string unitOfMeasureId,
        string unitOfMeasure,
        string description,
        decimal unitCost,
        decimal amount,
        decimal discount,
        string taxObjectId)
    {
        var invoiceItem = new InvoiceItem
        {
            SatItemId = satItemId,
            ItemId = satItemId,
            Quantity = quantity,
            UnitOfMeasureId = unitOfMeasureId,
            UnitOfMeasure = unitOfMeasure,
            Description = description,
            UnitCost = unitCost,
            Amount = amount,
            Discount = discount,
            TaxObjectId = taxObjectId
        };
        invoice.InvoiceItems.Add(invoiceItem);
    }


    /// <summary>
    /// Serializes the current invoice and writes it to disk
    /// </summary>
    /// <param name="filePath">Path of the xml invoice to be written</param>
    /// <exception cref="ArgumentNullException">filePath</exception>
    public void SerializeToFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentNullException(nameof(filePath), "The path to write the invoice must not be null");


        SerializerHelper.ConfigureSettingsForInvoice();

        var settings = new XmlWriterSettings
        {
            CloseOutput = true,
            Encoding = Encoding.UTF8,
            Indent = true
        };
        Serializer<Invoice>.SerializeToFile(invoice, filePath, SerializerHelper.Namespaces, settings);
    }


    /// <summary>
    /// Serializes the current invoice and writes it to memory stream
    /// </summary>
    /// <returns>xml invoice as string</returns>
    public string SerializeToString()
    {
        SerializerHelper.ConfigureSettingsForInvoice();

        var settings = new XmlWriterSettings
        {
            CloseOutput = true,
            Encoding = Encoding.UTF8,
            Indent = false
        };
        var xml = Serializer<Invoice>.Serialize(invoice, SerializerHelper.Namespaces, settings);

        return xml.Clean();
    }

    /// <summary>
    /// This method makes the calculation of all invoice values, it is recommended to use it instead of doing the calculations yourself.
    /// If you use this method, make sure that the amounts generated by the ComputeInvoice() method are the same as the ones you did internally,
    /// this ensures that the administrative records in your systems are consistent with the fiscal records in the SAT.
    ///
    /// *Disclaimer*
    /// This is a helper method, feel free to implement your own calculation method, values, or instead make them as part of your business logic,
    /// i.e.build the invoice object from previously calculated sub-objects.
    /// *Disclaimer*
    /// </summary>
    public void Compute()
    {
        invoice.Compute();
    }
}