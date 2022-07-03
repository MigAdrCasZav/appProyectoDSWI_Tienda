using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace appProyectoDSWI_Tienda.Models
{
    public class FabricanteA
    {
        [DisplayName("CODIGO")]
        public int idFabr { get; set; }
        [DisplayName("NOMBRE")]
        public string nomFabr { get; set; }
        [DisplayName("PAIS")]
        public int pais { get; set; }
    }
}