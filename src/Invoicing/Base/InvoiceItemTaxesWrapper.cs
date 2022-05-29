﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoicing.Base
{
   
    public class InvoiceItemTaxesWrapper
    {
        /// <summary>
        /// Nodo opcional para asentar los impuestos trasladados aplicables al presente concepto.
        /// </summary>
        [XmlArray(ElementName = "Traslados")]
        [XmlArrayItem(ElementName = "Traslado")]
        public List<InvoiceItemTax>? TransferredTaxes { get; set; }


        /// <summary>
        /// Nodo opcional para asentar los impuestos retenidos aplicables al presente concepto.
        /// </summary>
        [XmlArray(ElementName = "Retenciones")]
        [XmlArrayItem(ElementName = "Retencion")]
        public List<InvoiceItemTax>? WithholdingTaxes { get; set; }
    }
}