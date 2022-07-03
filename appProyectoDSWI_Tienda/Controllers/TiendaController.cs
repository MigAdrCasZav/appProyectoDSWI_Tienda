using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using appProyectoDSWI_Tienda.Models;
using System.Configuration;

namespace appProyectoDSWI_Tienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        List<ProductoA> ListProductos()
        {
            List<ProductoA> aProductos = new List<ProductoA>();
            SqlCommand cmd = new SqlCommand("SP_LISTAPARCIALPRODUCTO", cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aProductos.Add(new ProductoA()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    nombre = dr[1].ToString(),
                    precio = double.Parse(dr[2].ToString()),
                    foto = dr[3].ToString()
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
                Session["carrito"] = new List<Item>();
            }
            return View(ListProductos());
        }

        public ActionResult seleccionaProducto(int id)
        {
            ProductoA objP = ListProductos().Where(a => a.codigo == id).FirstOrDefault();
            return View(objP);
        }

        public ActionResult agregarProducto(int id, int cant = 0)
        {
            var miProducto = ListProductos().Where(p => p.codigo == id).FirstOrDefault();
            Item objI = new Item()
            {
                codigo = miProducto.codigo,
                nombre = miProducto.nombre,
                precio = miProducto.precio,
                cantidad = cant,
                foto = miProducto.foto
            };
            var miCarrito = (List<Item>)Session["carrito"];
            miCarrito.Add(objI);
            Session["carrito"] = miCarrito;
            return RedirectToAction("carritoCompras");
        }

        public ActionResult Comprar()
        {
            if (Session["carrito"] == null)
            {
                return RedirectToAction("carritoCompras");
            }
            var carrito = (List<Item>)Session["carrito"];
            ViewBag.monto = carrito.Sum(p => p.subtotal);
            return View(carrito);
        }

        public ActionResult eliminaProducto(int? id = null)
        {
            if (id == null) return RedirectToAction("carritoCompras");
            var carrito = (List<Item>)Session["carrito"];
            var item = carrito.Where(i => i.codigo == id).FirstOrDefault();
            carrito.Remove(item);
            Session["carrito"] = carrito;
            return RedirectToAction("Comprar");
        }
        //Metodo para pagar
        public ActionResult Pago()
        {
            List<Item> detalle = (List<Item>)Session["carrito"];
            double mt = 0;
            foreach (Item it in detalle)
            {
                mt += it.subtotal;
            }
            ViewBag.mt = mt;
            return View(detalle);
        }
        public ActionResult Final()
        {
            List<Item> detalle = (List<Item>)Session["carrito"];
            double mt = 0;
            foreach (Item it in detalle)
            {
                mt += it.subtotal;

            }
            ViewBag.mt = mt;
            return View();
        }
    }
}