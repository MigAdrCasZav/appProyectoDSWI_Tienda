using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using appProyectoDSWI_Tienda.Models;

namespace appProyectoDSWI_Tienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult carritoCompras()
        {
            if (Session["carrito"] == null)
            {
                Session["carrito"] = new List<DetalleVenta>();
            }
            return View(ListProductos());
        }

        public ActionResult seleccionaProducto(int id)
        {
            Producto objP = ListProductos().Where(a => a.codigo == id).FirstOrDefault();
            return View(objP);
        }

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

        public ActionResult eliminaProducto(int? id = null)
        {
            if (id == null) return RedirectToAction("carritoCompras");
            var carrito = (List<DetalleVenta>)Session["carrito"];
            var item = carrito.Where(i => i.codproducto == id).FirstOrDefault();
            carrito.Remove(item);
            Session["carrito"] = carrito;
            return RedirectToAction("Comprar");
        }
        //Metodo para pagar
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
        public ActionResult Final()
        {
            List<DetalleVenta> detalle = (List<DetalleVenta>)Session["carrito"];
            double mt = 0;
            foreach (DetalleVenta dt in detalle)
            {
                mt += dt.subtotal;

            }
            ViewBag.mt = mt;
            return View();
        }
    }
}