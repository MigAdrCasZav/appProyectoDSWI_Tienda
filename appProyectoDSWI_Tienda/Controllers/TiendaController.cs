using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using appProyectoDSWI_Tienda.Logica;
using appProyectoDSWI_Tienda.Models;
using appProyectoDSWI_Tienda.Permisos;
using System.Web.Security;

namespace appProyectoDSWI_Tienda.Controllers
{
    public class TiendaController : Controller
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        // GET: Tienda

        // Inicio
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        // Login del Proyecto
        public ActionResult Login()
        {
            return View();
        }

        // Login del Proyecto
        [HttpPost]
        public ActionResult Login(string correo, string clave)
        {
            Usuario objU = new LO_Usuario().EncontrarUsuario(correo, clave);

            if (objU.nomUsuario != null)
            {
                FormsAuthentication.SetAuthCookie(objU.emailUsuario, false);
                Session["Usuario"] = objU;
                return RedirectToAction("Index", "Tienda");
            }
            return View();
        }

        // Sin Autorizacion
        [Authorize]
        public ActionResult SinAutorizacion()
        {
            ViewBag.Message = "Usted no cuenta con autorización para ver esta pagina";

            return View();
        }

        // Cerrar Sesion
        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            Session["Usuario"] = null;
            return RedirectToAction("Login", "Tienda");
        }

        // Listado de Productos para el Carrito de Compras
        [Authorize]
        [PermisosRol(Models.Rol.Cliente)]
        List<Producto> ListProductos()
        {
            List<Producto> aProductos = new List<Producto>();
            SqlCommand cmd = new SqlCommand("SP_LISTAPARCIALPRODUCTO", cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aProductos.Add(new Producto()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    nombre = dr[1].ToString(),
                    fabricante = dr[2].ToString(),
                    categoria = dr[3].ToString(),
                    precio = double.Parse(dr[4].ToString()),
                    foto = dr[5].ToString()
                });
            }
            dr.Close();
            cn.Close();
            return aProductos;
        }

        // Carrito de Compras
        [Authorize]
        [PermisosRol(Models.Rol.Cliente)]
        public ActionResult carritoCompras()
        {
            if (Session["carrito"] == null)
            {
                Session["carrito"] = new List<DetalleVenta>();
            }
            return View(ListProductos());
        }

        // Seleccion de Productos
        [Authorize]
        [PermisosRol(Models.Rol.Cliente)]
        public ActionResult seleccionaProducto(int id)
        {
            Producto objP = ListProductos().Where(a => a.codigo == id).FirstOrDefault();
            return View(objP);
        }

        // Adicion de Productos
        [Authorize]
        [PermisosRol(Models.Rol.Cliente)]
        public ActionResult agregarProducto(int id, int cant = 0)
        {
            var miProducto = ListProductos().Where(p => p.codigo == id).FirstOrDefault();
            DetalleVenta objD = new DetalleVenta()
            {
                codproducto = miProducto.codigo,
                nombre = miProducto.nombre,
                fabricante = miProducto.fabricante,
                categoria = miProducto.categoria,
                precio = miProducto.precio,
                cantidad = cant,
                foto = miProducto.foto
            };
            var miCarrito = (List<DetalleVenta>)Session["carrito"];
            miCarrito.Add(objD);
            Session["carrito"] = miCarrito;
            return RedirectToAction("carritoCompras");
        }

        // Compra de Productos
        [Authorize]
        [PermisosRol(Models.Rol.Cliente)]
        public ActionResult Comprar()
        {
            if (Session["carrito"] == null)
            {
                return RedirectToAction("carritoCompras");
            }
            var carrito = (List<DetalleVenta>)Session["carrito"];
            ViewBag.monto = carrito.Sum(p => p.subtotal);
            return View(carrito);
        }

        // Eliminacion de Productos
        [Authorize]
        [PermisosRol(Models.Rol.Cliente)]
        public ActionResult eliminaProducto(int? id = null)
        {
            if (id == null) return RedirectToAction("carritoCompras");
            var carrito = (List<DetalleVenta>)Session["carrito"];
            var item = carrito.Where(i => i.codproducto == id).FirstOrDefault();
            carrito.Remove(item);
            Session["carrito"] = carrito;
            return RedirectToAction("Comprar");
        }

        // Confirmación de Pago
        [Authorize]
        [PermisosRol(Models.Rol.Cliente)]
        public ActionResult Pago()
        {
            List<DetalleVenta> detalle = (List<DetalleVenta>)Session["carrito"];
            double mt = 0;
            foreach (DetalleVenta dt in detalle)
            {
                mt += dt.subtotal;
            }
            ViewBag.mt = mt;
            return View(detalle);
        }

        // Obtencion del Codigo de Venta
        public int obtenercodigoVentaSQL()
        {
            int codven = 0;
            SqlCommand com = new SqlCommand("SELECT IDENT_CURRENT('VENTA')", cn);
            codven = Convert.ToInt32(com.ExecuteScalar());
            return codven;
        }

        // Realización de Venta
        [Authorize]
        [PermisosRol(Models.Rol.Cliente)]
        public ActionResult Final()
        {
            List<DetalleVenta> detalle = (List<DetalleVenta>)Session["carrito"];
            double mt = 0;
            Venta venta = new Venta();
            venta.fechaventa = DateTime.Now;
            venta.subtotalventa = detalle.Sum(x => x.precio * x.cantidad) * 0.84;
            venta.ivaventa = venta.subtotalventa * 0.1904761904761905;
            venta.totalventa = venta.subtotalventa + venta.ivaventa;
            cn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_REGISTROVENTA", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FEC", venta.fechaventa);
                cmd.Parameters.AddWithValue("@SUB", venta.subtotalventa);
                cmd.Parameters.AddWithValue("@IVA", venta.ivaventa);
                cmd.Parameters.AddWithValue("@TOT", venta.totalventa);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            foreach (DetalleVenta detalleVenta in detalle)
            {
                venta.codventa = obtenercodigoVentaSQL();
                SqlCommand cmd = new SqlCommand("SP_REGISTRODETALLEVENTA", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VEN", venta.codventa);
                cmd.Parameters.AddWithValue("@PRO", detalleVenta.codproducto);
                cmd.Parameters.AddWithValue("@CAN", detalleVenta.cantidad);
                cmd.Parameters.AddWithValue("@TOT", detalleVenta.subtotal);
                cmd.ExecuteNonQuery();
                mt += detalleVenta.subtotal;
            }
            ViewBag.mt = mt;
            cn.Close();
            return View();
        }

        // Limpieza de Carrito de Compras
        [Authorize]
        [PermisosRol(Models.Rol.Cliente)]
        public ActionResult limpiarCarrito()
        {
            Session["carrito"] = null;
            return RedirectToAction("carritoCompras");
        }
    }
}