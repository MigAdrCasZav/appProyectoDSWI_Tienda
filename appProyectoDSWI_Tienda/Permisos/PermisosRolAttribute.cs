using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using appProyectoDSWI_Tienda.Models;

namespace appProyectoDSWI_Tienda.Permisos
{
    public class PermisosRolAttribute : ActionFilterAttribute
    {
        private Rol idRol;

        public PermisosRolAttribute(Rol _idRol)
        {
            idRol = _idRol;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["Usuario"] != null)
            {
                Usuario usuario = HttpContext.Current.Session["Usuario"] as Usuario;
                if (usuario.idRol != this.idRol)
                {
                    filterContext.Result = new RedirectResult("~/Tienda/SinAutorizacion");
                }
            }
            base.OnActionExecuting(filterContext);
        }











    }
}