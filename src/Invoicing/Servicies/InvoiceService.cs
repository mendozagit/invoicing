using System.Text;
using System.Xml;
using Credencials.Common;
using Credencials.Core;
using Invoicing.Base;
using Invoicing.Common.Contracts;
using Invoicing.Common.Enums;
using Invoicing.Common.Exceptions;
using Invoicing.Common.Extensions;
using Invoicing.Common.Serializing;

namespace Invoicing.Servicies;

public class InvoiceService : IComputable
{
    internal readonly Invoice _invoice;


    public InvoiceService()
    {
        _invoice = new Invoice();
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


        _invoice.InvoiceItems.Add(invoiceItem);
    }

    /// <summary>
    /// Add invoiceItem to current invoice object
    /// </summary>
    /// <param name="invoiceItem">invoice item to be added</param>
    public void AddInvoiceItem(InvoiceItem invoiceItem)
    {
        //_invoice.InvoiceItems ??= new List<InvoiceItem>();
        _invoice.InvoiceItems.Add(invoiceItem);
    }

    /// <summary>
    /// Add invoiceItems to current invoice object
    /// </summary>
    /// <param name="invoiceItems"></param>
    /// <exception cref="ArgumentNullException">invoiceItems</exception>
    public void AddInvoiceItems(List<InvoiceItem> invoiceItems)
    {
        if (!invoiceItems.Any())
            throw new ArgumentNullException(nameof(invoiceItems),
                "The list of items on the invoice must contain at least one item");

        _invoice.InvoiceItems.AddRange(invoiceItems);
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
        Serializer<Invoice>.SerializeToFile(_invoice, filePath, SerializerHelper.Namespaces, settings);
    }


    /// <summary>
    /// Serializes the current invoice and writes it to memory stream
    /// Compute()?
    /// </summary>
    /// <returns>xml invoice as string</returns>
    public string SerializeToString()
    {
        switch (InvoiceTypeId)
        {
            case InvoiceType.Ingreso:
                SerializerHelper.ConfigureSettingsForInvoice();
                break;
            case InvoiceType.Egreso:
                SerializerHelper.ConfigureSettingsForInvoice();
                break;
            case InvoiceType.Traslado:
                SerializerHelper.ConfigureSettingsForWaybill();
                break;
            case InvoiceType.Nomina:
                SerializerHelper.ConfigureSettingsForPayroll();
                break;
            case InvoiceType.Pago:
                SerializerHelper.ConfigureSettingsForPayment();
                break;
            default:
                throw new NotSupportedException("Invoice type is not supported");
        }


        _invoice.SchemaLocation = SerializerHelper.SchemaLocation;
        var settings = new XmlWriterSettings
        {
            CloseOutput = true,
            Encoding = Encoding.UTF8,
            Indent = false
        };
        var xml = Serializer<Invoice>.Serialize(_invoice, SerializerHelper.Namespaces, settings);

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
        _invoice.Compute();
    }

    /// <summary>
    /// Nodo requerido para expresar la información del contribuyente emisor del comprobante.
    /// </summary>
    /// <param name="invoiceIssuer">issuer object to be added to the invoice</param>
    public void AddIssuer(InvoiceIssuer invoiceIssuer)
    {
        _invoice.InvoiceIssuer = invoiceIssuer;
    }

    /// <summary>
    /// Nodo requerido para expresar la información del contribuyente emisor del comprobante.
    /// </summary>
    /// <param name="tin">RFC:Atributo requerido para registrar la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente emisor del comprobante.</param>
    /// <param name="legalName">RazonSocial:Atributo requerido para registrar el nombre, denominación o razón social del contribuyente inscrito en el RFC, del emisor del comprobante.</param>
    /// <param name="taxRegimeId">RegimenFiscalId:Atributo requerido para incorporar la clave del régimen del contribuyente emisor al que aplicará el efecto fiscal de este comprobante.</param>
    /// <param name="operationNumber">FacAtrAdquirente:Atributo condicional para expresar el número de operación proporcionado por el SAT cuando se trate de un comprobante a través de un PCECFDI o un PCGCFDISP.</param>
    public void AddIssuer(string tin, string legalName, string taxRegimeId, string? operationNumber = null)
    {
        var invoiceIssuer = new InvoiceIssuer
        {
            Tin = tin,
            LegalName = legalName,
            TaxRegimeId = taxRegimeId,
            OperationNumber = operationNumber
        };

        _invoice.InvoiceIssuer = invoiceIssuer;
    }

    public void AddRecipient(InvoiceRecipient invoiceRecipient)
    {
        _invoice.InvoiceRecipient = invoiceRecipient;
    }

    /// <summary>
    /// Nodo requerido para precisar la información del contribuyente receptor del comprobante
    /// </summary>
    /// <param name="tin">RFC:Atributo requerido para registrar la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente receptor del comprobante.</param>
    /// <param name="legalName">RazonSocial:Atributo requerido para registrar el nombre(s), primer apellido, segundo apellido, según corresponda, denominación o razón social del contribuyente, inscrito en el RFC, del receptor del comprobante.</param>
    /// <param name="zipCode">Atributo requerido para registrar el código postal del domicilio fiscal del receptor del comprobante.</param>
    /// <param name="taxRegimeId">Atributo requerido para incorporar la clave del régimen fiscal del contribuyente receptor al que aplicará el efecto fiscal de este comprobante.</param>
    /// <param name="cfdiUseId">Atributo requerido para expresar la clave del uso que dará a esta factura el receptor del CFDI.</param>
    /// <param name="foreignCountryId">Atributo condicional para registrar la clave del país de residencia para efectos fiscales del receptor del comprobante, cuando se trate de un extranjero, y que es conforme con la especificación ISO 3166-1 alpha-3. Es requerido cuando se incluya el complemento de comercio exterior o se registre el atributo NumRegIdTrib.</param>
    /// <param name="foreignTin">Atributo condicional para expresar el número de registro de identidad fiscal del receptor cuando sea residente en el extranjero. Es requerido cuando se incluya el complemento de comercio exterior.</param>
    public void AddRecipient(string tin,
        string legalName,
        string zipCode,
        string taxRegimeId,
        string cfdiUseId,
        string? foreignCountryId = null,
        string? foreignTin = null)
    {
        var invoiceRecipient = new InvoiceRecipient
        {
            Tin = foreignTin,
            LegalName = legalName,
            ZipCode = zipCode,
            TaxRegimeId = taxRegimeId,
            CfdiUseId = cfdiUseId,
            ForeignCountryId = foreignCountryId,
            ForeignTin = foreignTin
        };
        _invoice.InvoiceRecipient = invoiceRecipient;
    }

    /// <summary>
    /// Nodo condicional para precisar la información relacionada con el comprobante global.
    /// </summary>
    /// <param name="globalInformation"></param>
    public void AddGlobalInformation(InvoiceGlobalInformation globalInformation)
    {
        _invoice.GlobalInformation = globalInformation;
    }

    /// <summary>
    /// Nodo condicional para precisar la información relacionada con el comprobante global.
    /// </summary>
    /// <param name="periodicity">Atributo requerido para expresar el período al que corresponde la información del comprobante global.</param>
    /// <param name="month">Atributo requerido para expresar el mes o los meses al que corresponde la información del comprobante global.</param>
    /// <param name="year">Atributo requerido para expresar el año al que corresponde la información del comprobante global.</param>
    public void AddGlobalInformation(string periodicity, string month, int year)
    {
        var globalInformation = new InvoiceGlobalInformation
        {
            Periodicity = periodicity,
            Month = month,
            Year = year
        };
        _invoice.GlobalInformation = globalInformation;
    }

    /// <summary>
    /// Nodo opcional para precisar la información de los comprobantes relacionados.
    /// </summary>
    /// <param name="invoiceRelated">invoice related to be added to invoice</param>
    public void AddRelatedCfdi(InvoiceRelated invoiceRelated)
    {
        _invoice.RelatedInvoiceWrapper ??= new InvoiceRelatedWrapper();
        _invoice.RelatedInvoiceWrapper.RelatedInvoices ??= new List<InvoiceRelated>();
        _invoice.RelatedInvoiceWrapper.RelatedInvoices.Add(invoiceRelated);
    }

    /// <summary>
    /// Nodo opcional para precisar la información de los comprobantes relacionados.
    /// Por ejemplo:
    /// Si el CFDI relacionado es un comprobante de traslado que sirve para registrar el movimiento de la mercancía.
    /// Si este comprobante se usa como nota de crédito o nota de débito del comprobante relacionado.
    /// Si este comprobante es una devolución sobre el comprobante relacionado.
    /// Si éste sustituye a una factura cancelada
    /// </summary>
    /// <param name="invoiceUuid">Atributo requerido para registrar el folio fiscal (UUID) de un CFDI relacionado con el presente comprobante</param>
    /// <param name="relationshipTypeId">Atributo requerido para indicar la clave de la relación que existe entre éste que se está generando y el o los CFDI previo.
    /// El ultimo tipo relacion agregado afecta todos los UUID. 
    /// </param>
    public void AddRelatedCfdi(string invoiceUuid, string relationshipTypeId = "01")
    {
        var invoiceRelated = new InvoiceRelated
        {
            InvoiceUuid = invoiceUuid
        };

        _invoice.RelatedInvoiceWrapper ??= new InvoiceRelatedWrapper();
        _invoice.RelatedInvoiceWrapper.RelationshipTypeId = relationshipTypeId;
        _invoice.RelatedInvoiceWrapper.RelatedInvoices ??= new List<InvoiceRelated>();
        _invoice.RelatedInvoiceWrapper.RelatedInvoices.Add(invoiceRelated);
    }


    /// <summary>
    /// Sign the OriginalString and fill the Invoice.SignatureValue property with it..
    /// </summary>
    /// <param name="compute">True to call the Compute() method automatically, otherwise false.</param>
    /// <returns>OriginalString</returns>
    /// <exception cref="CredentialNotFoundException">When the credential property is not established</exception>
    /// <exception cref="CredentialConfigurationException">When the path to the XSLT schemas is not established in CredentialSettings.</exception>
    public string SignInvoice(bool compute = true)
    {
        if (Credential is null)
            throw new CredentialNotFoundException("The credential object has not been set in invoice service.");


        if (string.IsNullOrEmpty(CredentialSettings.OriginalStringPath))
            throw new CredentialConfigurationException(
                "The path to the xslt schemas was not set in CredentialSettings.");


        var originalStr = ComputeOriginalString(compute);
        var signature = Credential.SignData(originalStr);
        _invoice.SignatureValue = signature.ToBase64String();

        return originalStr;
    }

    /// <summary>
    /// 1-Serialize the invoice to xml in memory.
    /// 2-Calculates the original xml string.
    /// </summary>
    /// <param name="compute">True to call the Compute() method automatically, otherwise false.</param>
    /// <returns>OriginalString</returns>
    /// <exception cref="CredentialNotFoundException">When the credential property is not established</exception>
    /// <exception cref="CredentialConfigurationException">When the path to the XSLT schemas is not established in CredentialSettings.</exception>
    public string ComputeOriginalString(bool compute = true)
    {
        if (Credential is null)
            throw new CredentialNotFoundException("The credential object has not been set in invoice service.");


        if (string.IsNullOrEmpty(CredentialSettings.OriginalStringPath))
            throw new CredentialConfigurationException(
                "The path to the xslt schemas was not set in CredentialSettings.");


        if (compute)
            Compute();

        var xml = SerializeToString();
        var originalStr = Credential.GetOriginalStringByXmlString(xml);

        return originalStr;
    }


    #region Properties

    private InvoiceVersion _invoiceVersion;
    private string? _invoiceSerie;
    private string? _invoiceNuber;
    private string? _invoiceDate;
    private string? _signatureValue;
    private string? _paymentForm;
    private string? _certificateNumber;
    private string? _certificateB64;
    private string? _paymentConditions;
    private decimal _subtotal;
    private decimal _discount;
    private string? _currency;
    private decimal _exchangeRate;
    private decimal _total;
    private InvoiceType _invoiceTypeId;
    private string? _exportId;
    private string? _paymentMethodId;
    private string? _expeditionZipCode;
    private string? _pacConfirmation;


    /// <summary>
    /// Atributo requerido con valor prefijado a 4.0 que indica la versión del estándar bajo el que se encuentra expresado el comprobante.
    /// </summary>

    public InvoiceVersion InvoiceVersion
    {
        get => _invoiceVersion;
        set => _invoiceVersion = _invoice.InvoiceVersion = value;
    }


    /// <summary>
    /// Atributo opcional para precisar la serie para control interno del contribuyente. Este atributo acepta una cadena de caracteres.
    /// </summary>

    public string? InvoiceSerie
    {
        get => _invoiceSerie;
        set => _invoiceSerie = _invoice.InvoiceSerie = value;
    }

    /// <summary>
    /// Atributo opcional para control interno del contribuyente que expresa el folio del comprobante, acepta una cadena de caracteres.
    /// </summary>

    public string? InvoiceNuber
    {
        get => _invoiceNuber;
        set => _invoiceNuber = _invoice.InvoiceNuber = value;
    }


    /// <summary>
    /// Atributo requerido para la expresión de la fecha y hora de expedición del Comprobante Fiscal Digital por Internet. Se expresa en la forma AAAA-MM-DDThh:mm:ss y debe corresponder con la hora local donde se expide el comprobante.
    /// </summary>

    public string? InvoiceDate
    {
        get => _invoiceDate;
        set => _invoiceDate = _invoice.InvoiceDate = value;
    }

    /// <summary>
    /// Atributo requerido para contener el sello digital del comprobante fiscal, al que hacen referencia las reglas de resolución miscelánea vigente. El sello debe ser expresado como una cadena de texto en formato Base 64.
    /// </summary>

    public string? SignatureValue
    {
        get => _signatureValue;
        set => _signatureValue = _invoice.SignatureValue = value;
    }


    /// <summary>
    /// Atributo condicional para expresar la clave de la forma de pago de los bienes o servicios amparados por el comprobante.
    /// </summary>

    public string? PaymentForm
    {
        get => _paymentForm;
        set => _paymentForm = _invoice.PaymentForm = value;
    }

    /// <summary>
    /// Atributo requerido para expresar el número de serie del certificado de sello digital que ampara al comprobante, de acuerdo con el acuse correspondiente a 20 posiciones otorgado por el sistema del SAT.
    /// </summary>

    public string? CertificateNumber
    {
        get => _certificateNumber;
        set => _certificateNumber = _invoice.CertificateNumber = value;
    }

    /// <summary>
    /// Atributo requerido que sirve para incorporar el certificado de sello digital que ampara al comprobante, como texto en formato base 64.
    /// </summary>
    public string? CertificateB64
    {
        get => _certificateB64;
        set => _certificateB64 = _invoice.CertificateB64 = value;
    }


    /// <summary>
    /// Atributo condicional para expresar las condiciones comerciales aplicables para el pago del comprobante fiscal digital por Internet. Este atributo puede ser condicionado mediante atributos o complementos.
    /// </summary>
    public string? PaymentConditions
    {
        get => _paymentConditions;
        set => _paymentConditions = _invoice.PaymentConditions = value;
    }


    /// <summary>
    /// Atributo requerido para representar la suma de los importes de los conceptos antes de descuentos e impuesto. No se permiten valores negativos.
    /// </summary>
    public decimal Subtotal
    {
        get => _subtotal;
        set => _subtotal = _invoice.Subtotal = value;
    }

    /// <summary>
    /// Atributo condicional para representar el importe total de los descuentos aplicables antes de impuestos. No se permiten valores negativos. Se debe registrar cuando existan conceptos con descuento.
    /// </summary>
    public decimal Discount
    {
        get => _discount;
        set => _discount = _invoice.Discount = value;
    }

    /// <summary>
    /// Atributo requerido para identificar la clave de la moneda utilizada para expresar los montos, cuando se usa moneda nacional se registra MXN. Conforme con la especificación ISO 4217.
    /// </summary>

    public string? Currency
    {
        get => _currency;
        set => _currency = _invoice.Currency = value;
    }

    /// <summary>
    /// Atributo condicional para representar el tipo de cambio FIX conforme con la moneda usada. Es requerido cuando la clave de moneda es distinta de MXN y de XXX. El valor debe reflejar el número de pesos mexicanos que equivalen a una unidad de la divisa señalada en el atributo moneda. Si el valor está fuera del porcentaje aplicable a la moneda tomado del catálogo c_Moneda, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera no automática, una clave de confirmación para ratificar que el valor es correcto e integrar dicha clave en el atributo Confirmacion.
    /// </summary>

    public decimal ExchangeRate
    {
        get => _exchangeRate;
        set => _exchangeRate = _invoice.ExchangeRate = value;
    }

    /// <summary>
    /// Atributo requerido para representar la suma del subtotal, menos los descuentos aplicables, más las contribuciones recibidas (impuestos trasladados - federales y/o locales, derechos, productos, aprovechamientos, aportaciones de seguridad social, contribuciones de mejoras) menos los impuestos retenidos federales y/o locales. Si el valor es superior al límite que establezca el SAT en la Resolución Miscelánea Fiscal vigente, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera no automática, una clave de confirmación para ratificar que el valor es correcto e integrar dicha clave en el atributo Confirmacion. No se permiten valores negativos.
    /// </summary>

    public decimal Total
    {
        get => _total;
        set => _total = _invoice.Total = value;
    }

    /// <summary>
    /// Atributo requerido para expresar la clave del efecto del comprobante fiscal para el contribuyente emisor.
    /// </summary>

    public InvoiceType InvoiceTypeId
    {
        get => _invoiceTypeId;
        set => _invoiceTypeId = _invoice.InvoiceTypeId = value;
    }


    /// <summary>
    /// Atributo requerido para expresar si el comprobante ampara una operación de exportación.
    /// </summary>

    public string? ExportId
    {
        get => _exportId;
        set => _exportId = _invoice.ExportId = value;
    }


    /// <summary>
    /// Atributo condicional para precisar la clave del método de pago que aplica para este comprobante fiscal digital por Internet, conforme al Artículo 29-A fracción VII incisos a y b del CFF.
    /// </summary>

    public string? PaymentMethodId
    {
        get => _paymentMethodId;
        set => _paymentMethodId = _invoice.PaymentMethodId = value;
    }


    /// <summary>
    /// Atributo requerido para incorporar el código postal del lugar de expedición del comprobante (domicilio de la matriz o de la sucursal).
    /// </summary>

    public string? ExpeditionZipCode
    {
        get => _expeditionZipCode;
        set => _expeditionZipCode = _invoice.ExpeditionZipCode = value;
    }


    /// <summary>
    /// Atributo condicional para registrar la clave de confirmación que entregue el PAC para expedir el comprobante con importes grandes, con un tipo de cambio fuera del rango establecido o con ambos casos. Es requerido cuando se registra un tipo de cambio o un total fuera del rango establecido.
    /// </summary>

    public string? PacConfirmation
    {
        get => _pacConfirmation;
        set => _pacConfirmation = _invoice.PacConfirmation = value;
    }


    /// <summary>
    /// Object to sign invoices and manage CSD (cer, key and pass)
    /// <see>
    ///     <cref>https://github.com/dotnetcfdi/credentials</cref>
    /// </see>
    /// </summary>
    public ICredential? Credential { get; set; }

    #endregion
}