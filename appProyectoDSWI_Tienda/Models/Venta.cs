using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appProyectoDSWI_Tienda.Models
{
    public class Venta
    {
        public int codventa { get; set; }
        public DateTime fechaventa { get; set; }
        public double subtotalventa { get; set; }
        public double ivaventa { get; set; }
        public double totalventa { get; set; }
    }
}