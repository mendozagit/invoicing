using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoicing.Complements.Payments
{
    /// <summary>
    /// Nodo requerido para especificar el monto total de los pagos y el total de los impuestos, deben ser expresados en MXN.
    /// </summary>
    public class PaymentSummary
    {
        //For withholding taxes, the calculation base is not specified.

        #region Withholding taxes

        /// <summary>
        /// Atributo condicional para expresar el total de los impuestos retenidos de IVA que se desprenden de los pagos. No se permiten valores negativos.
        /// </summary>
        [XmlAttribute("TotalRetencionesIVA")]
        [DefaultValue(0)]
        public decimal WithholdingIva { get; set; }

        /// <summary>
        /// Atributo condicional para expresar el total de los impuestos retenidos de ISR que se desprenden de los pagos. No se permiten valores negativos.
        /// </summary>
        [XmlAttribute("TotalRetencionesISR")]
        [DefaultValue(0)]
        public decimal WithholdingIsr { get; set; }

        /// <summary>
        /// Atributo condicional para expresar el total de los impuestos retenidos de IEPS que se desprenden de los pagos. No se permiten valores negativos.
        /// </summary>
        [XmlAttribute("TotalRetencionesIEPS")]
        [DefaultValue(0)]
        public decimal WithholdingIeps { get; set; }

        #endregion


        //For withholding taxes, the calculation base must be specified.

        #region Transferred taxes

        /// <summary>
        /// Atributo condicional para expresar el total de la base de IVA trasladado a la tasa del 16% que se desprende de los pagos. No se permiten valores negativos.
        /// </summary>
        [XmlAttribute("TotalTrasladosBaseIVA16")]
        [DefaultValue(0)]
        public decimal TransferredIva16Base { get; set; }

        /// <summary>
        /// Atributo condicional para expresar el total de los impuestos de IVA trasladado a la tasa del 16% que se desprenden de los pagos. No se permiten valores negativos.
        /// </summary>
        [XmlAttribute("TotalTrasladosImpuestoIVA16")]
        [DefaultValue(0)]
        public decimal TransferredIva16 { get; set; }

        /// <summary>
        /// Atributo condicional para expresar el total de la base de IVA trasladado a la tasa del 8% que se desprende de los pagos. No se permiten valores negativos.
        /// </summary>
        [XmlAttribute("TotalTrasladosBaseIVA8")]
        [DefaultValue(0)]
        public decimal TransferredIva8Base { get; set; }

        /// <summary>
        /// Atributo condicional para expresar el total de los impuestos de IVA trasladado a la tasa del 8% que se desprenden de los pagos.No se permiten valores negativos.
        /// </summary>
        [XmlAttribute("TotalTrasladosImpuestoIVA8")]
        [DefaultValue(0)]
        public decimal TransferredIva8 { get; set; }

        /// <summary>
        /// Atributo condicional para expresar el total de la base de IVA trasladado a la tasa del 0% que se desprende de los pagos. No se permiten valores negativos.
        /// </summary>
        [XmlAttribute("TotalTrasladosBaseIVA0")]
        [DefaultValue(0)]
        public decimal TransferredIva0Base { get; set; }

        /// <summary>
        /// Atributo condicional para expresar el total de los impuestos de IVA trasladado a la tasa del 0% que se desprenden de los pagos. No se permiten valores negativos.
        /// </summary>
        [XmlAttribute("TotalTrasladosImpuestoIVA0")]
        [DefaultValue(0)]
        public decimal TransferredIva0 { get; set; }

        /// <summary>
        /// Atributo condicional para expresar el total de la base de IVA trasladado exento que se desprende de los pagos. No se permiten valores negativos.
        /// </summary>
        //public decimal TransferredIvaExcentoBase { get; set; }
        [XmlAttribute("TotalTrasladosBaseIVAExento")]
        [DefaultValue(0)]
        public decimal TransferredIvaExcento { get; set; }

        /// <summary>
        /// Atributo requerido para expresar el total de los pagos que se desprenden de los nodos Pago. No se permiten valores negativos.
        /// </summary>
        [XmlAttribute("MontoTotalPagos")]
        [DefaultValue(0)]
        public decimal TotalPaymentAmount { get; set; }

        #endregion
    }
}