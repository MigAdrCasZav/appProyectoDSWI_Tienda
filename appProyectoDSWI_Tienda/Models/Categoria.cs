using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace appProyectoDSWI_Tienda.Models
{
    public class Categoria
    {
        [DisplayName("CODIGO")]
        public int idCateg { get; set; }
        [DisplayName("NOMBRE")]
        public string nomCateg { get; set; }
        [DisplayName("DESCRIPCION")]
        public string desCateg { get; set; }
    }
}