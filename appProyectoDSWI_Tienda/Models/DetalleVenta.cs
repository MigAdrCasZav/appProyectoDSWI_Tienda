using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appProyectoDSWI_Tienda.Models
{
    public class DetalleVenta
    {
        public int coddetalle { get; set; }
        public int codventa { get; set; }
        public int codproducto { get; set; }
        public int cantidad { get; set; }
        public double total { get; set; }
    }
}