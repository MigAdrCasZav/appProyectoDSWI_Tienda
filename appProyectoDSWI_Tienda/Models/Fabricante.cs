using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace appProyectoDSWI_Tienda.Models
{
    public class Fabricante
    {
        [DisplayName("CODIGO")]
        public int idFabr { get; set; }
        [DisplayName("NOMBRE")]
        public string nomFabr { get; set; }
        [DisplayName("PAIS")]
        public string paisFabr { get; set; }
    }
}