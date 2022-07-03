using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace appProyectoDSWI_Tienda.Models
{
    public class Producto
    {
        [DisplayName("CODIGO")]
        public int codigo { get; set; }
        [DisplayName("NOMBRE")]
        public string nombre { get; set; }
        [DisplayName("DESCRIPCION")]
        public string descripcion { get; set; }
        [DisplayName("FABRICANTE")]
        public int fabricante { get; set; }
        [DisplayName("CATEGORIA")]
        public int categoria { get; set; }
        [DisplayName("PRECIO S/")]
        public double precio { get; set; }
        [DisplayName("FOTO")]
        public string foto { get; set; }
    }
}