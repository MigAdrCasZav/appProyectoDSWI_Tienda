using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using appProyectoDSWI_Tienda.Models;

namespace appProyectoDSWI_Tienda.Controllers
{
    public class ProductoController : Controller
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        // GET: Producto
        public ActionResult Index()
        {
            return View();
        }

        List<Producto> listarProductos()
        {
            List<Producto> aProductos = new List<Producto>();
            SqlCommand cmd = new SqlCommand("SP_LISTAPRODUCTO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Producto objP = new Producto()
                {
                    codigo = int.Parse(dr[0].ToString()),
                    nombre = dr[1].ToString(),
                    descripcion = dr[2].ToString(),
                    fabricante = dr[3].ToString(),
                    categoria = dr[4].ToString(),
                    precio = double.Parse(dr[5].ToString()),
                    foto = dr[6].ToString()
                };
                aProductos.Add(objP);
            }
            dr.Close();
            cn.Close();
            return aProductos;
        }

        List<Fabricante> listFabricanteAs()
        {
            List<Fabricante> aFabricantes = new List<Fabricante>();
            SqlCommand cmd = new SqlCommand("SP_FABRICANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Fabricante objF = new Fabricante()
                {
                    idFabr = int.Parse(dr[0].ToString()),
                    nomFabr = dr[1].ToString(),
                    paisFabr = dr[2].ToString()
                };
                aFabricantes.Add(objF);
            }
            cn.Close();
            return aFabricantes;
        }
        List<Categoria> listCategoria()
        {
            List<Categoria> aCategorias = new List<Categoria>();
            SqlCommand cmd = new SqlCommand("SP_CATEGORIA", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Categoria objC = new Categoria()
                {
                    idCateg = int.Parse(dr[0].ToString()),
                    nomCateg = dr[1].ToString(),
                    desCateg = dr[2].ToString()
                };
                aCategorias.Add(objC);
            }
            cn.Close();
            return aCategorias;
        }

        public ActionResult listadoProductos()
        {
            return View(listarProductos());
        }

        void CRUD(String proceso, List<SqlParameter> pars)
        {
            cn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(proceso, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(pars.ToArray());
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
            }
            cn.Close();
        }

        public ActionResult nuevoProducto()
        {
            ViewBag.fabricante = new SelectList(listFabricanteAs(), "codigo", "nombre");
            ViewBag.categoria = new SelectList(listCategoria(), "codigo", "nombre");
            return View(new ProductoA());
        }

        [HttpPost]
        public ActionResult nuevoProducto(ProductoA objP, HttpPostedFileBase f)
        {
            if (f == null)
            {
                ViewBag.mensaje = "Seleccione una imagen";
                return View(objP);
            }
            if (Path.GetExtension(f.FileName) != ".jpg")
            {
                ViewBag.mensaje = "Debe ser .JPG";
                return View(objP);
            }
            List<SqlParameter> lista = new List<SqlParameter>()
            {
                new SqlParameter(){ParameterName="@NOM",SqlDbType=SqlDbType.VarChar,Value=objP.nombre},
                new SqlParameter(){ParameterName="@DES",SqlDbType=SqlDbType.VarChar,Value=objP.descripcion},
                new SqlParameter(){ParameterName="@FAB",SqlDbType=SqlDbType.Int,Value=objP.fabricante},
                new SqlParameter(){ParameterName="@CAT",SqlDbType=SqlDbType.Int,Value=objP.categoria },
                new SqlParameter(){ParameterName="@PRE",SqlDbType=SqlDbType.Decimal,Value=objP.precio },
                new SqlParameter(){ParameterName="@FOT",SqlDbType=SqlDbType.VarChar,Value="~/fotos_productos/"+Path.GetFileName(f.FileName) }
            };
            ViewBag.fabricante = new SelectList(listFabricanteAs(), "codigo", "nombre");
            ViewBag.categoria = new SelectList(listCategoria(), "codigo", "nombre");
            CRUD("SP_REGISTROPRODUCTO", lista);
            f.SaveAs(Path.Combine(Server.MapPath("~/fotos_productos/"),
            Path.GetFileName(f.FileName)));
            return RedirectToAction("listadoProductos");
        }





    }
}