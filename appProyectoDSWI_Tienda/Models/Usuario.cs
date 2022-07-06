using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace appProyectoDSWI_Tienda.Models
{
    public class Usuario
    {
        public int codUsuario { get; set; }
        public string nomUsuario { get; set; }
        public string apeUsuario { get; set; }
        public int dni { get; set; }
        public string dirUsuario { get; set; }
        public string emailUsuario { get; set; }
        public string claveUsuario { get; set; }
        public Rol idRol { get; set; }
    }
}