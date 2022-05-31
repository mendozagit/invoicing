﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoicing.Base
{
    /// <summary>
    /// Nodo requerido para la información detallada de una retención de impuesto específico.
    /// </summary>
    public class InvoiceWithholdingTax
    {
        /// <summary>
        /// Atributo requerido para señalar la clave del tipo de impuesto retenido.
        /// </summary>
        [XmlAttribute("Impuesto")]
        public string? TaxId { get; set; }



        /// <summary>
        /// Atributo requerido para señalar el monto del impuesto retenido. No se permiten valores negativos.
        /// </summary>
        [XmlAttribute("Importe")]
        [DefaultValue(0)]
        public decimal Amount { get; set; }
    }
}
