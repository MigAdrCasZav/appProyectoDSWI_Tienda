using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace appProyectoDSWI_Tienda.Models
{
    public class Item
    {
        [DisplayName("CODIGO")]
        public int codigo { get; set; }
        [DisplayName("NOMBRE")]
        public string nombre { get; set; }
        [DisplayName("FABRICANTE")]
        public string fabricante { get; set; }
        [DisplayName("CATEGORIA")]
        public string categoria { get; set; }
        [DisplayName("PRECIO S/")]
        public double precio { get; set; }
        [DisplayName("CANTIDAD")]
        public int cantidad { get; set; }
        [DisplayName("SUBTOTAL")]
        public double subtotal { get { return precio * cantidad; } }
        [DisplayName("FOTO")]
        public string foto { get; set; }
    }
}